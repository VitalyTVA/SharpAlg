using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class ExpressionExtensions {
        public static double Evaluate(this Expr expr, Context context = null) {
            return expr.Visit(new ExpressionEvaluator(context ?? new Context()));
        }
        public static bool ExprEquals(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionComparer(expr2));
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionComparer : IExpressionVisitor<bool> {
        readonly Expr expr;
        public ExpressionComparer(Expr expr) {
            this.expr = expr;
        }
        public bool Constant(ConstantExpr constant) {
            return DoEqualityCheck(constant, (x1, x2) => x1.Value == x2.Value);
        }
        public bool Binary(BinaryExpr binary) {
            return DoEqualityCheck(binary, (x1, x2) => x1.Left.ExprEquals(x2.Left) && x1.Right.ExprEquals(x2.Right) && x1.Operation == x2.Operation);
        }
        public bool Parameter(ParameterExpr parameter) {
            return DoEqualityCheck(parameter, (x1, x2) => x1.ParameterName == x2.ParameterName);
        }
        bool DoEqualityCheck<T>(T expr2, Func<T, T, bool> equalityCheck) where T : Expr {
            var other = expr as T;
            return other != null && equalityCheck(other, expr2);
        }
    }
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
            return GetBinaryOperationEvaluator(binary.Operation)(binary.Left.Visit(this), binary.Right.Visit(this));
        }
        public double Parameter(ParameterExpr parameter) {
            var parameterValue = context.GetValue(parameter.ParameterName);
            if(parameterValue == null)
                throw new ExpressionEvaluationException(string.Format("{0} value is undefined", parameter.ParameterName));
            return parameterValue.Visit(this); //TODO recursion
        }
        static Func<double, double, double> GetBinaryOperationEvaluator(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return (x1, x2) => x1 + x2; 
                case BinaryOperation.Subtract:
                    return (x1, x2) => x1 - x2;
                case BinaryOperation.Multiply:
                    return (x1, x2) => x1 * x2;
                case BinaryOperation.Divide:
                    return (x1, x2) => x1 / x2;
                default:
                    throw new NotImplementedException();
            }

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
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public interface IExpressionVisitor<T> {
        T Constant(ConstantExpr constant);
        T Parameter(ParameterExpr parameter);
        T Binary(BinaryExpr binary);
    }
}
