using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native.Builder {
    [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
    public class PowerExpressionExtractor : DefaultExpressionVisitor<PowerExpr> {
        public static PowerExpr ExtractPower(Expr expr) {
            return expr.Visit(new PowerExpressionExtractor()); //TODO singleton
        }
        PowerExpressionExtractor() { }
        public override PowerExpr Power(PowerExpr power) {
            return power;
        }
        protected override PowerExpr GetDefault(Expr expr) {
            return Expr.Power(expr, Expr.One);
        }
    }
}
