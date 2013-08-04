using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    [DebuggerDisplay("Expr: {Print}")]
    public abstract class Expr {
        public const string STR_Factorial = "factorial";
        public static readonly ConstantExpr Zero = new ConstantExpr(Number.Zero);
        public static readonly ConstantExpr One = new ConstantExpr(Number.One);
        public static readonly ConstantExpr MinusOne = new ConstantExpr(Number.MinusOne);
        public static ConstantExpr Constant(Number constant) {
            return new ConstantExpr(constant);
        }
        public static ParameterExpr Parameter(string parameterName) {
            return new ParameterExpr(parameterName);
        }
        public static Expr Binary(Expr left, Expr right, BinaryOperation type) {
            return Multi(new Expr[] { left, right }, type);
        }
        public static Expr Multi(IEnumerable<Expr> args, BinaryOperation type) {
            return args.Count() > 1 ? (type == BinaryOperation.Add ? (MultiExpr)new AddExpr(args) : new MultiplyExpr(args)) : args.First();
        }
        public static Expr Add(IEnumerable<Expr> args) {
            return Multi(args, BinaryOperation.Add);
        }
        public static Expr Multiply(IEnumerable<Expr> args) {
            return Multi(args, BinaryOperation.Multiply);
        }
        public static Expr Add(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Add);
        }
        public static Expr Subtract(Expr left, Expr right) {
            return Add(left, Minus(right));
        }
        public static Expr Multiply(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Multiply);
        }
        public static Expr Divide(Expr left, Expr right) {
            return Multiply(left, Inverse(right));
        }
        public static PowerExpr Power(Expr left, Expr right) {
            return new PowerExpr(left, right);
        }
        public static Expr Minus(Expr expr) {
            return Multiply(Expr.MinusOne, expr);
        }
        public static Expr Inverse(Expr expr) {
            return Power(expr, Expr.MinusOne);
        }
        public static FunctionExpr Function(string functionName, Expr argument) {
            return Function(functionName, new Expr[] { argument });
        }
        public static FunctionExpr Function(string functionName, IEnumerable<Expr> argument) {
            return new FunctionExpr(functionName, argument);
        }
        public static FunctionExpr Factorial(Expr argument) {
            return Function(STR_Factorial, argument);
        }
        internal abstract T Visit<T>(IExpressionVisitor<T> visitor);
#if DEBUG
        public string Print { get { return this.Print(); } }
#endif
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ConstantExpr : Expr {
        internal ConstantExpr(Number value) {
            Value = value;
        }
        public Number Value { get; private set; }
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
        None, Add, Multiply, Power, Factorial 
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public abstract class MultiExpr : Expr {
        internal MultiExpr(IEnumerable<Expr> args) {
            Args = args;
        }
        public IEnumerable<Expr> Args { get; private set; }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class AddExpr : MultiExpr {
        internal AddExpr(IEnumerable<Expr> args) 
            : base(args) {
        }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Add(this);
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class MultiplyExpr : MultiExpr {
        internal MultiplyExpr(IEnumerable<Expr> args)
            : base(args) {
        }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Multiply(this);
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
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class FunctionExpr : Expr {
        internal FunctionExpr(string functionName, IEnumerable<Expr> arguments) {
            Arguments = arguments;
            FunctionName = functionName;
        }
        public string FunctionName { get; private set; }
        public IEnumerable<Expr> Arguments { get; private set; }
        internal override T Visit<T>(IExpressionVisitor<T> visitor) {
            return visitor.Function(this);
        }
    }
}