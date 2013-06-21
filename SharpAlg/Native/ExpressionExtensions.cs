using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class ExpressionExtensions {
        public static double Evaluate(this Expr expr) {
            return expr.Visit(new ExpressionEvaluator());
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
    }
    //[JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public interface IExpressionVisitor<T> {
        T Constant(ConstantExpr constant);
        T Add(Expr left, Expr right);
        T Subtract(Expr left, Expr right);
        T Multiply(Expr left, Expr right);
        T Divide(Expr left, Expr right);
    }
}
