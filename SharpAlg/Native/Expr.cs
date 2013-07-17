using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public abstract class Expr {
        public static readonly ConstantExpr Zero = new ConstantExpr(0);
        public static readonly ConstantExpr One = new ConstantExpr(1);
        public static readonly ConstantExpr MinusOne = new ConstantExpr(-1);
        public static ConstantExpr Constant(double constant) {
            return new ConstantExpr(constant);
        }
        public static ParameterExpr Parameter(string parameterName) {
            return new ParameterExpr(parameterName);
        }
        public static MultiExpr Binary(Expr left, Expr right, BinaryOperation type) {
            return Multi(new Expr[] { left, right }, type);
        }
        public static MultiExpr Multi(IEnumerable<Expr> args, BinaryOperation type) {
            return new MultiExpr(args, type);
        }
        public static MultiExpr Add(Expr left, Expr right) { //TODO use builder everywhere
            return Binary(left, right, BinaryOperation.Add);
        }
        public static MultiExpr Subtract(Expr left, Expr right) {
            return Add(left, Minus(right));
        }
        public static MultiExpr Multiply(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Multiply);
        }
        public static MultiExpr Divide(Expr left, Expr right) {
            return Multiply(left, Inverse(right));
        }
        public static PowerExpr Power(Expr left, Expr right) {
            return new PowerExpr(left, right);
        }
        //public static Expr Unary(Expr expr, UnaryOperation operation) {
        //    return new UnaryExpr(expr, operation);
        //}
        public static Expr Minus(Expr expr) {
            return Multiply(Expr.MinusOne, expr);
        }
        public static Expr Inverse(Expr expr) {
            return Power(expr, Expr.MinusOne);
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
        Add, Multiply
    }
    public enum BinaryOperationEx {
        Add, Subtract, Multiply, Divide
    }
    public enum OperationPriority { 
        None, Add, Multiply, Power 
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class MultiExpr : Expr {
        internal MultiExpr(IEnumerable<Expr> args, BinaryOperation operation) {
            Args = args;
            Operation = operation;
        }
        public BinaryOperation Operation { get; private set; }
        public IEnumerable<Expr> Args { get; private set; }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Multi(this);
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class PowerExpr : Expr {
        internal PowerExpr(Expr left, Expr right) {
            Right = right;
            Left = left;
        }
        public Expr Left { get; private set; }
        public Expr Right { get; private set; }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Power(this);
        }
    }
    //public enum UnaryOperation {
    //    Minus, Inverse
    //}
    //[JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    //public class UnaryExpr : Expr {
    //    internal UnaryExpr(Expr expr, UnaryOperation operation) {
    //        Expr = expr;
    //        Operation = operation;
    //    }
    //    public Expr Expr { get; private set; }
    //    public UnaryOperation Operation { get; private set; }
    //    internal override T Visit<T>(IExpressionVisitor<T> visitor) {
    //        return visitor.Unary(this);
    //    }
    //}
}