using SharpAlg.Native.Builder;
using SharpKit.JavaScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class DiffFunction : Function, ISupportConvolution {
        public DiffFunction()
            : base("diff") {
        }
        public override Number Evaluate(IExpressionVisitor<Number> evaluator, IEnumerable<Expr> args) {
            return Convolute(args).Visit(evaluator);
        }
        public Expr Convolute(IEnumerable<Expr> args) {
            var argsTail = args.Tail();
            if(!argsTail.All(x => x is ParameterExpr))
                throw new ExpressionDefferentiationException("All diff arguments should be parameters");//TODO correct message, go to constant
            var diffList = argsTail.Cast<ParameterExpr>();
            if(!diffList.Any())
                return args.First().Diff();
            Expr result = args.First();
            diffList.ForEach(x => result = result.Diff(x.ParameterName));
            return result;
        }
    }
}
