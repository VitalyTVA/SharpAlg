using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public abstract class Expr {
        public static readonly ConstantExpr Zero = new ConstantExpr(0);
        public static readonly ConstantExpr One = new ConstantExpr(1);
        public static ConstantExpr Constant(double constant) {
            return new ConstantExpr(constant);
        }
        public static ParameterExpr Parameter(string parameterName) {
            return new ParameterExpr(parameterName);
        }
        public static BinaryExpr Binary(Expr left, Expr right, BinaryOperation type) {
            return new BinaryExpr(left, right, type);
        }
        public static BinaryExpr Add(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Add);
        }
        public static BinaryExpr Subtract(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Subtract);
        }
        public static BinaryExpr Multiply(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Multiply);
        }
        public static BinaryExpr Divide(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Divide);
        }
        public static BinaryExpr Power(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Power);
        }
        internal abstract T Visit<T>(IExpressionVisitor<T> visitor);
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ConstantExpr : Expr {
        internal ConstantExpr(double value) {
            Value = value;
        }
        public double Value { get; private set; }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Constant(this);
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ParameterExpr : Expr {
        internal ParameterExpr(string parameterName) {
            ParameterName = parameterName;
        }
        public string ParameterName { get; private set; }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Parameter(this);
        }
    }
    public enum BinaryOperation {
        Add, Subtract, Multiply, Divide, Power
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
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Binary(this);
        }
    }
}