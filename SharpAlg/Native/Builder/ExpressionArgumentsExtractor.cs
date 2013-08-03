using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native.Builder {
    [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
    public class ExpressionArgumentsExtractor : DefaultExpressionVisitor<IEnumerable<Expr>> {
        public static IEnumerable<Expr> ExtractArguments(Expr expr, BinaryOperation operation) {
            return expr.Visit(new ExpressionArgumentsExtractor(operation)); //TODO singleton
        }
        readonly BinaryOperation operation;
        ExpressionArgumentsExtractor(BinaryOperation operation) { this.operation = operation; }
        public override IEnumerable<Expr> Add(AddExpr multi) {
            if(operation == BinaryOperation.Add)
                return multi.Args;
            return base.Add(multi);
        }
        public override IEnumerable<Expr> Multiply(MultiplyExpr multi) {
            if(operation == BinaryOperation.Multiply)
                return multi.Args;
            return base.Multiply(multi);
        }
        protected override IEnumerable<Expr> GetDefault(Expr expr) {
            return new Expr[] { expr };
        }
    }
}
