using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class ExpressionExtensions {
        public static double Evaluate(this Expr expr, Context context = null) {
            return expr.Visit(new ExpressionEvaluator(context ?? new Context()));
        }
        public static bool ExprEquals(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionComparer(expr2));
        }
    }
}