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
    public class LnFunction : SingleArgumentDifferentiableFunction, ISupportConvolution {
        public LnFunction()
            : base(FunctionFactory.LnName) {
        }
        protected override Number Evaluate(Number arg) {
            return NumberFactory.GetFloat(arg, x => Math.Log(x));
        }
        protected override Expr DiffCore(ExprBuilder builder, Expr arg) {
            return builder.Inverse(arg);
        }

        public Expr Convolute(IContext context, IEnumerable<Expr> args) {
            var arg = args.Single();
            return ConstantConvolution(arg) ??
                PowerConvolution(context, arg) ??
                InverseFunctionConvolution(context, arg);
        }

        static Expr PowerConvolution(IContext context, Expr arg) {
            return arg
                .ConvertAs<PowerExpr>()
                .Return(x => Expr.Multiply(x.Right, FunctionFactory.Ln(x.Left)), () => null);
        }
        static Expr InverseFunctionConvolution(IContext context, Expr arg) {
            return arg
                .ConvertAs<FunctionExpr>()
                .If(x => context.GetFunction(x.FunctionName) is ExpFunction)
                .Return(x => x.Args.First(), () => null);
        }
        static ConstantExpr ConstantConvolution(Expr arg) {
            return arg.If(x => x.ExprEquals(Expr.One)).Return(x => Expr.Zero, () => null);
        }
    }
}
