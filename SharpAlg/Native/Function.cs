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
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class FactorialFunction : SingleArgumentFunction {
        public FactorialFunction()
            : base(FunctionFactory.FactorialName) {
        }
        protected override Number Evaluate(Number arg) {
            Number result = Number.One;
            for(Number i = Number.Two; i <= arg; i = i + Number.One) {
                result = result * i;
            }
            return result;
        }
        //TODO factorial differentiation
    }

    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class LnFunction : SingleArgumentDifferentiableFunction, ISupportConvolution {
        public LnFunction()
            : base(FunctionFactory.LnName) {
        }
        protected override Number Evaluate(Number arg) {
            return Number.Ln(arg);
        }
        protected override Expr DiffCore(ExprBuilder builder, Expr arg) {
            return builder.Inverse(arg);
        }

        public Expr Convolute(IEnumerable<Expr> args) {
            if(args.First().ExprEquals(Expr.One))
                return Expr.Zero;
            return null;
        }
    }
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

    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class Functions {
        static Function factorial;
        public static Function Factorial { get { return factorial ?? (factorial = new FactorialFunction()); } }

        static Function ln;
        public static Function Ln { get { return ln ?? (ln = new LnFunction()); } }

        static Function diff;
        public static Function Diff { get { return diff ?? (diff = new DiffFunction()); } }
    }
}
