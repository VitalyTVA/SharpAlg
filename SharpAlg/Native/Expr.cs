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
        public abstract bool ExprEquals(Expr expr); //TODO rewrite using visitors (not logic in expressions) and go back to Prototype mode

        internal abstract T Visit<T>(IExpressionVisitor<T> visitor);
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
        public override bool ExprEquals(Expr expr) {
            var other = expr as ParameterExpr;
            return other != null && other.ParameterName == ParameterName;
        }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            throw new NotImplementedException();
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
            var other = expr as BinaryExpr;
            return other != null && other.Left.ExprEquals(Left) && other.Right.ExprEquals(Right) && other.Operation == Operation;
        }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            switch(Operation) {
                case BinaryOperation.Add:
                    return visitor.Add(Left, Right);
                case BinaryOperation.Subtract:
                    return visitor.Subtract(Left, Right);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}