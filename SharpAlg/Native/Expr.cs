using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public abstract class Expr {
        public static ConstantExpr Constant(double constant) {
            return new ConstantExpr(constant);
        }
        public static ParameterExpr Parameter(string parameterName) {
            return new ParameterExpr(parameterName);
        }
        public static BinaryExpr Binary(Expr left, Expr right, BinaryOperation type) {
            return new BinaryExpr(left, right, type);
        }
        public abstract bool ExprEquals(Expr expr); //TODO rewrite using visitors (not logic in expressions)
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ConstantExpr : Expr {
        internal ConstantExpr(double value) {
            Value = value;
        }
        public double Value { get; private set; }
        public override bool ExprEquals(Expr expr) {
            var other = expr as ConstantExpr;
            return other != null && other.Value == Value;
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ParameterExpr : Expr {
        internal ParameterExpr(string parameterName) {
            ParameterName = parameterName;
        }
        public string ParameterName { get; private set; }
        public override bool ExprEquals(Expr expr) {
            var other = expr as ParameterExpr;
            return other != null && other.ParameterName == ParameterName;
        }
    }
    public enum BinaryOperation {
        Add, Subtract, Multiply, Divide
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class BinaryExpr : Expr {
        internal BinaryExpr(Expr left, Expr right, BinaryOperation operation) {
            Operation = operation;
            Right = right;
            Left = left;
        }
        public Expr Left { get; private set; }
        public Expr Right { get; private set; }
        public BinaryOperation Operation { get; private set; }
        public override bool ExprEquals(Expr expr) {
            throw new NotImplementedException();
        }
    }
}
