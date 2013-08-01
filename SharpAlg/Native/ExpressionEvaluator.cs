using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionEvaluator : IExpressionVisitor<Number> {
        readonly Context context;
        public ExpressionEvaluator(Context context) {
            this.context = context;
        }
        public Number Constant(ConstantExpr constant) {
            return constant.Value;
        }
        public Number Multi(MultiExpr multi) {
            Number result = Number.Zero;
            multi.Args.Accumulate(x => {
                result = x.Visit(this);
            }, x => {
                result = GetBinaryOperationEvaluator(multi.Operation)(result, x.Visit(this));
            });
            return result;
        }
        public Number Power(PowerExpr power) {
            return power.Left.Visit(this) ^ power.Right.Visit(this);
        }
        public Number Parameter(ParameterExpr parameter) {
            var parameterValue = context.GetValue(parameter.ParameterName);
            if(parameterValue == null)
                throw new ExpressionEvaluationException(string.Format("{0} value is undefined", parameter.ParameterName));
            return parameterValue.Visit(this); //TODO recursion
        }
        public static Func<Number, Number, Number> GetBinaryOperationEvaluator(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return (x1, x2) => x1 + x2;
                case BinaryOperation.Multiply:
                    return (x1, x2) => x1 * x2;
                default:
                    throw new NotImplementedException();
            }
        }
        //static Func<double, double, double> GetBinaryOperationEvaluatorEx(BinaryOperationEx operation) {
        //    switch(operation) {
        //        case BinaryOperationEx.Add:
        //            return (x1, x2) => x1 + x2;
        //        case BinaryOperationEx.Subtract:
        //            return (x1, x2) => x1 - x2;
        //        case BinaryOperationEx.Multiply:
        //            return (x1, x2) => x1 * x2;
        //        case BinaryOperationEx.Divide:
        //            return (x1, x2) => x1 / x2;
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
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
