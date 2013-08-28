using SharpAlg.Native.Builder;
using SharpKit.JavaScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation_Functions)]
    public class ExpFunction : SingleArgumentDifferentiableFunction, ISupportConvolution {
        public ExpFunction()
            : base(FunctionFactory.ExpName) {
        }
        protected override Number Evaluate(Number arg) {
            return NumberFactory.Exp(arg);
        }
        protected override Expr DiffCore(ExprBuilder builder, Expr arg) {
            return FunctionFactory.Exp(arg);
        }
        //public Expr GetPrintableExpression(IContext context, IEnumerable<Expr> args) {
        //    Expr arg = args.First();
        //    ParameterExpr expExpression = Expr.Parameter("e");
        //    return arg.ExprEquals(Expr.One) ? (Expr)expExpression : Expr.Power(expExpression, arg);
        //}

        public Expr Convolute(IContext context, IEnumerable<Expr> args) {
            var arg = args.First();
            return ConstantConvolution(arg) ??
                MultiplyConvoultion(context, arg);
        }
        static Expr MultiplyConvoultion(IContext context, Expr arg) {
            return arg
                .With(x => MultiplyExpressionExtractor.ExtractMultiply(x))
                .ConvertAs<MultiplyExpr>()
                .With(x => {
                    FunctionExpr lnExpr = x.Args
                        .Where(y => y is FunctionExpr)
                        .Cast<FunctionExpr>()
                        .FirstOrDefault(y => context.GetFunction(y.FunctionName) is LnFunction);
                    if(lnExpr != null) {
                        return new ConvolutionExprBuilder(context).Power(lnExpr.Args.First(), Expr.Multiply(x.Args.Where(y => y != lnExpr)));
                    }
                    return null;
                });
        }
        static ConstantExpr ConstantConvolution(Expr arg) {
            return arg.If(x => x.ExprEquals(Expr.Zero)).Return(x => Expr.One, () => null);
        }
    }

}
