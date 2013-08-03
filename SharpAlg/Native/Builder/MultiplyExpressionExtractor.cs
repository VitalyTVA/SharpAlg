using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native.Builder {
    [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
    public class MultiplyExpressionExtractor : DefaultExpressionVisitor<Tuple<Expr, Expr>> {
        public static Tuple<Expr, Expr> ExtractMultiply(Expr expr) {
            return expr.Visit(new MultiplyExpressionExtractor()); //TODO singleton
        }
        MultiplyExpressionExtractor() { }
        public override Tuple<Expr, Expr> Multiply(MultiplyExpr multi) {
            if(multi.Args.First() is ConstantExpr)
                return new Tuple<Expr, Expr>(multi.Args.First(), Expr.Multiply(multi.Args.Skip(1)));
            return base.Multiply(multi);
        }
        protected override Tuple<Expr, Expr> GetDefault(Expr expr) {
            return new Tuple<Expr, Expr>(Expr.One, expr);
        }
    }
}
