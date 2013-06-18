using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = "../res/SharpAlg.Native.js")]
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
    }
    [JsType(JsMode.Prototype, Filename = "../res/SharpAlg.Native.js")]
    public class ConstantExpr : Expr {
        internal ConstantExpr(double value) {
            Value = value;
        }
        public double Value { get; private set; }
    }
    [JsType(JsMode.Prototype, Filename = "../res/SharpAlg.Native.js")]
    public class ParameterExpr : Expr {
        internal ParameterExpr(string parameterName) {
            ParameterName = parameterName;
        }
        public string ParameterName { get; private set; }
    }
    public enum BinaryOperation {
        Add, Subtract, Multiply, Divide
    }
    [JsType(JsMode.Prototype, Filename = "../res/SharpAlg.Native.js")]
    public class BinaryExpr : Expr {
        internal BinaryExpr(Expr left, Expr right, BinaryOperation operation) {
            Operation = operation;
            Right = right;
            Left = left;
        }
        public Expr Left { get; private set; }
        public Expr Right { get; private set; }
        public BinaryOperation Operation { get; private set; }
    }
}
