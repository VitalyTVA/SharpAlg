using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native.Builder {
    [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
    public abstract class ExprBuilder {
        //public abstract Expr Binary(Expr left, Expr right, BinaryOperation operation);
        //public abstract Expr Unary(Expr expr, UnaryOperation operation);
        public abstract Expr Power(Expr left, Expr right);
        public abstract Expr Add(Expr left, Expr right);
        public abstract Expr Multiply(Expr left, Expr right);
        public Expr Subtract(Expr left, Expr right) {
            return Add(left, Minus(right));
        }
        public Expr Divide(Expr left, Expr right) {
            return Multiply(left, Inverse(right));
        }
        public Expr Minus(Expr expr) {
            return Multiply(Expr.MinusOne, expr);
        }
        public Expr Inverse(Expr expr) {
            return Power(expr, Expr.MinusOne);
        }
    }
}