using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionEvaluator : IExpressionVisitor<double> {
        readonly Context context;
        public ExpressionEvaluator(Context context) {
            this.context = context;
        }
        public double Constant(ConstantExpr constant) {
            return constant.Value;
        }
        public double Binary(BinaryExpr binary) {
            var enumerator = binary.Args.GetEnumerator();
            double result = 0;
            if(enumerator.MoveNext())
                result = enumerator.Current.Visit(this);
            while(enumerator.MoveNext()) {
                result = GetBinaryOperationEvaluator(binary.Operation)(result, enumerator.Current.Visit(this));
            }
            return result;
        }
        public double Power(PowerExpr power) {
            return Math.Pow(power.Left.Visit(this), power.Right.Visit(this));
        }

        public double Unary(UnaryExpr unary) {
            switch(unary.Operation) {
                case UnaryOperation.Minus:
                    return -unary.Expr.Visit(this);
                case UnaryOperation.Inverse:
                    return 1 / unary.Expr.Visit(this);
                default:
                    throw new NotImplementedException();
            }
        }
        public double Parameter(ParameterExpr parameter) {
            var parameterValue = context.GetValue(parameter.ParameterName);
            if(parameterValue == null)
                throw new ExpressionEvaluationException(string.Format("{0} value is undefined", parameter.ParameterName));
            return parameterValue.Visit(this); //TODO recursion
        }
        public static Func<double, double, double> GetBinaryOperationEvaluator(BinaryOperation operation) {
            return GetBinaryOperationEvaluatorEx(GetBinaryOperationEx(operation));
        }
        static Func<double, double, double> GetBinaryOperationEvaluatorEx(BinaryOperationEx operation) {
            switch(operation) {
                case BinaryOperationEx.Add:
                    return (x1, x2) => x1 + x2;
                case BinaryOperationEx.Subtract:
                    return (x1, x2) => x1 - x2;
                case BinaryOperationEx.Multiply:
                    return (x1, x2) => x1 * x2;
                case BinaryOperationEx.Divide:
                    return (x1, x2) => x1 / x2;
                default:
                    throw new NotImplementedException();
            }
        }
        public static BinaryOperationEx GetBinaryOperationEx(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return BinaryOperationEx.Add;
                case BinaryOperation.Multiply:
                    return BinaryOperationEx.Multiply;
                default:
                    throw new NotImplementedException();
            }
        }
        public static bool IsInvertedOperation(BinaryOperationEx operation) {
            switch(operation) {
                case BinaryOperationEx.Subtract:
                case BinaryOperationEx.Divide:
                    return true;
            }
            return false;
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionEvaluationException : Exception {
        public ExpressionEvaluationException()
            : base() {
        }
        public ExpressionEvaluationException(string message)
            : base(message) {
        }
    }
}
