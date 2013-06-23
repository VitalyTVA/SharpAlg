using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class ExpressionExtensions {
        public static double Evaluate(this Expr expr) {
            return expr.Visit(new ExpressionEvaluator());
        }
        public static bool ExprEquals(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionComparer(expr2));
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionComparer : IExpressionVisitor<bool> {
        readonly Expr expr;
        public ExpressionComparer(Expr expr) {
            this.expr = expr;
        }
        public bool Constant(ConstantExpr constant) {
            var other = expr as ConstantExpr;
            return other != null && other.Value == constant.Value;
        }
        public bool Binary(BinaryExpr binary) {
            var other = expr as BinaryExpr;
            return other != null && other.Left.ExprEquals(binary.Left) && other.Right.ExprEquals(binary.Right) && other.Operation == binary.Operation;
        }
        public bool Parameter(ParameterExpr parameter) {
            var other = expr as ParameterExpr;
            return other != null && other.ParameterName == parameter.ParameterName;
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionEvaluator : IExpressionVisitor<double> {
        public double Constant(ConstantExpr constant) {
            return constant.Value;
        }
        public double Binary(BinaryExpr binary) {
            return GetBinaryOperationEvaluator(binary.Operation)(binary.Left.Visit(this), binary.Right.Visit(this));
        }
        public double Parameter(ParameterExpr parameter) {
            throw new ExpressionEvaluationException(string.Format("{0} value is undefined", parameter.ParameterName));
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
    //[JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public interface IExpressionVisitor<T> {
        T Constant(ConstantExpr constant);
        T Parameter(ParameterExpr parameter);
        T Binary(BinaryExpr binary);
    }
}
