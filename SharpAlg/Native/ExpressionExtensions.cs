using SharpAlg.Native.Builder;
using SharpAlg.Native.Parser;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class ExpressionExtensions {
        public static Number Evaluate(this Expr expr, Context context = null) {
            return expr.Visit(new ExpressionEvaluator(context ?? Context.Default));
        }
        public static Expr Diff(this Expr expr, string parameterName = null) {
            return expr.Visit(new DiffExpressionVisitor(ConvolutionExprBuilder.CreateDefault(), Context.Default, parameterName));
        }
        public static bool ExprEquals(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionEqualityComparer(expr2));
        }
        public static bool ExprEquivalent(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionEquivalenceComparer(expr2));
        }
        public static string Print(this Expr expr) {
            return expr.Visit(SharpAlg.Native.Printer.ExpressionPrinter.Instance);
        }
        public static Expr Parse(this string expression, ExprBuilder builder = null) {
            return GetExpression(ParseCore(expression, builder ?? ConvolutionExprBuilder.CreateDefault()));
        }
        internal static Expr GetExpression(Parser.Parser parser) {
            if(parser.errors.Count > 0)
                throw new InvalidOperationException("String can not be parsed"); //TODO message
            return parser.Expr;
        }
        internal static Parser.Parser ParseCore(this string expression, ExprBuilder builder) {
            Scanner scanner = new Scanner(expression);
            Parser.Parser parser = new Parser.Parser(scanner, builder);
            parser.Parse();
            return parser;
        }
        public static Expr Tail(this MultiplyExpr multi) {
            return Expr.Multiply(multi.Args.Tail());
        }
        public static Expr Tail(this AddExpr multi) {
            return Expr.Add(multi.Args.Tail());
        }
    }
}