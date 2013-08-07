using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native.Builder {
    [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
    public class TrivialExprBuilder : ExprBuilder {
        //public override Context Context { get { return Context.Empty; } }
        public override Expr Parameter(string parameterName) {
            return Expr.Parameter(parameterName);
        }
        public override Expr Add(Expr left, Expr right) {
            return Expr.Add(left, right);
        }
        public override Expr Multiply(Expr left, Expr right) {
            return Expr.Multiply(left, right);
        }
        public override Expr Power(Expr left, Expr right) {
            return Expr.Power(left, right);
        }
        public override Expr Function(string functionName, IEnumerable<Expr> arguments) {
            return Expr.Function(functionName, arguments);
        }
    }
}
