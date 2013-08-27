using SharpAlg.Native.Builder;
using SharpKit.JavaScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation)]
    public class InvalidArgumentCountException : Exception {
        public InvalidArgumentCountException() {
            
        }
        public InvalidArgumentCountException(string message)
            : base(message) {
            
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation)]
    public abstract class SingleArgumentFunction : Function, ISupportCheckArgs {
        static bool IsValidArgsCount<T>(IEnumerable<T> args) {
            return args.Count() == 1;
        }
        public override Number Evaluate(IExpressionEvaluator evaluator, IEnumerable<Expr> args) {
            return EvaluateCore(args.Select(x => x.Visit(evaluator)));
        }
        protected static void CheckArgsCount<T>(IEnumerable<T> args) {
            if(!IsValidArgsCount<T>(args))
                throw new InvalidArgumentCountException(); //TODO message
        }
        protected SingleArgumentFunction(string name)
            : base(name) {
        }
        Number EvaluateCore(IEnumerable<Number> args) {
            CheckArgsCount(args);
            return Evaluate(args.First());
        }
        protected abstract Number Evaluate(Number arg);

        public string Check(IEnumerable<Expr> args) {
            return IsValidArgsCount(args) ? string.Empty : string.Format("Error, (in {0}) expecting 1 argument, got {1}", Name, args.Count());
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation)]
    public abstract class SingleArgumentDifferentiableFunction : SingleArgumentFunction, ISupportDiff {
        protected SingleArgumentDifferentiableFunction(string name)
            : base(name) {
        }
        public Expr Diff(IDiffExpressionVisitor diffVisitor, IEnumerable<Expr> args) {
            CheckArgsCount(args);
            Expr arg = args.First();
            return diffVisitor.Builder.Multiply(arg.Visit(diffVisitor), DiffCore(diffVisitor.Builder, arg)); //TODO use builder
        }
        protected abstract Expr DiffCore(ExprBuilder builder, Expr arg);
    }
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation)]
    public class FactorialFunction : SingleArgumentFunction, ISupportConvolution {
        public FactorialFunction()
            : base(FunctionFactory.FactorialName) {
        }
        protected override Number Evaluate(Number arg) {
            Number result = NumberFactory.One;
            for(Number i = NumberFactory.Two; i <= arg; i = i + NumberFactory.One) {
                result = result * i;
            }
            return result;
        }
        //TODO factorial differentiation

        public Expr Convolute(IContext context, IEnumerable<Expr> args) {
            return args
                .First()
                .ConvertAs<ConstantExpr>()
                .If(x => x.Value.IsInteger)
                .Return(x => Expr.Constant(Evaluate(x.Value)), () => null);
        }
    }

    [JsType(JsMode.Clr, Filename = SR.JS_Implementation)]
    public class LnFunction : SingleArgumentDifferentiableFunction, ISupportConvolution {
        public LnFunction()
            : base(FunctionFactory.LnName) {
        }
        protected override Number Evaluate(Number arg) {
            return NumberFactory.Ln(arg);
        }
        protected override Expr DiffCore(ExprBuilder builder, Expr arg) {
            return builder.Inverse(arg);
        }

        public Expr Convolute(IContext context, IEnumerable<Expr> args) {
            var arg = args.First();
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
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation)]
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
                MultiplyConvoultion(context, arg) ??
                InverseFunctionConvolution(context, arg);
        }
        static Expr MultiplyConvoultion(IContext context, Expr arg) {
            return arg
                .ConvertAs<MultiplyExpr>()
                .With(x => {
                    FunctionExpr lnExpr = x.Args
                        .Where(y => y is FunctionExpr)
                        .Cast<FunctionExpr>()
                        .FirstOrDefault(y => context.GetFunction(y.FunctionName) is LnFunction);
                    if(lnExpr != null)
                        return Expr.Power(lnExpr.Args.First(), Expr.Multiply(x.Args.Where(y => y != lnExpr)));
                    return null;
                });
        }
        static Expr InverseFunctionConvolution(IContext context, Expr arg) {
            return arg
                .ConvertAs<FunctionExpr>()
                .If(x => context.GetFunction(x.FunctionName) is LnFunction)
                .Return(x => x.Args.First(), () => null);
        }
        static ConstantExpr ConstantConvolution(Expr arg) {
            return arg.If(x => x.ExprEquals(Expr.Zero)).Return(x => Expr.One, () => null);
        }
    }

}
