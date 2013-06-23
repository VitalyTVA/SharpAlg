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
        public bool Add(Expr left, Expr right) {
            var other = expr as BinaryExpr;
            return other != null && other.Left.ExprEquals(left) && other.Right.ExprEquals(right) && other.Operation == BinaryOperation.Add;
        }
        public bool Subtract(Expr left, Expr right) {
            var other = expr as BinaryExpr;
            return other != null && other.Left.ExprEquals(left) && other.Right.ExprEquals(right) && other.Operation == BinaryOperation.Subtract;
        }
        public bool Multiply(Expr left, Expr right) {
            var other = expr as BinaryExpr;
            return other != null && other.Left.ExprEquals(left) && other.Right.ExprEquals(right) && other.Operation == BinaryOperation.Multiply;
        }
        public bool Divide(Expr left, Expr right) {
            var other = expr as BinaryExpr;
            return other != null && other.Left.ExprEquals(left) && other.Right.ExprEquals(right) && other.Operation == BinaryOperation.Divide;
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
        public double Add(Expr left, Expr right) {
            return left.Visit(this) + right.Visit(this);
        }
        public double Subtract(Expr left, Expr right) {
            return left.Visit(this) - right.Visit(this);
        }
        public double Multiply(Expr left, Expr right) {
            return left.Visit(this) * right.Visit(this);
        }
        public double Divide(Expr left, Expr right) {
            return left.Visit(this) / right.Visit(this);
        }
        public double Parameter(ParameterExpr parameter) {
            throw new ExpressionEvaluationException(string.Format("{0} value is undefined", parameter.ParameterName));
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
        T Add(Expr left, Expr right);
        T Subtract(Expr left, Expr right);
        T Multiply(Expr left, Expr right);
        T Divide(Expr left, Expr right);
    }
}
