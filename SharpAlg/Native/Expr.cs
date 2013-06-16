using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = "../res/SharpAlg.Native.js")]
    public abstract class Expr {
        public static ConstExpr Const(double constant) {
            return new ConstExpr(constant);
        }
        public static ParameterExpr Parameter(string parameterName) {
            return new ParameterExpr(parameterName);
        }
    }
    [JsType(JsMode.Prototype, Filename = "../res/SharpAlg.Native.js")]
    public class ConstExpr : Expr {
        internal ConstExpr(double value) {
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
}
