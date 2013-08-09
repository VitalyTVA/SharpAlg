using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class ExprFactory {
        public static FunctionExpr Factorial(Expr argument) {
            return Expr.Function(Functions.Factorial.Name, argument);
        }
        public static FunctionExpr Ln(Expr argument) {
            return Expr.Function(Functions.Ln.Name, argument);
        }
    }
}