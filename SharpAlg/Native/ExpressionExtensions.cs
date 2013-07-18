using SharpAlg.Native.Parser;
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
        public static Expr Diff(this Expr expr) {
            return expr.Visit(new DiffExpressionVisitor(new ConvolutionExprBuilder()));
        }
        public static bool ExprEquals(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionEqualityComparer(expr2));
        }
        public static bool ExprEquivalent(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionEquivalenceComparer(expr2));
        }
        public static string Print(this Expr expr) {
            return expr.Visit(new ExpressionPrinter());
        }
        public static Expr Parse(this string expression) {
            Parser.Parser parser = ParseCore(expression, new ConvolutionExprBuilder());
            if(parser.errors.Count > 0)
                throw new InvalidOperationException("String can not be parsed"); //TODO message
            return parser.Expr;
        }
        internal static Parser.Parser ParseCore(string expression, ExprBuilder builder) {
            Scanner scanner = new Scanner(expression);
            Parser.Parser parser = new Parser.Parser(scanner, builder);
            parser.Parse();
            return parser;
        }
        public static void Accumulate(this MultiExpr multi, Action<Expr> init, Action<Expr> next) {
            var enumerator = multi.Args.GetEnumerator();
            if(enumerator.MoveNext())
                init(enumerator.Current);
            else
                throw new InvalidOperationException();
            while(enumerator.MoveNext()) {
                next(enumerator.Current);
            }
        }
        public static Expr Tail(this MultiExpr multi) {
            int count = multi.Args.Count();
            if(count > 2)
                return Expr.Multi(multi.Args.RemoveAt(0), multi.Operation);
            if(count == 2)
                return multi.Args.ElementAt(1);
            throw new InvalidOperationException();
        }
    }
}