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
            return expr.Visit(new ExpressionEvaluator(context ?? ContextFactory.Default));
        }
        public static Expr Diff(this Expr expr, string parameterName = null) {
            return expr.Visit(new DiffExpressionVisitor(ExprBuilderFactory.CreateDefault(), ContextFactory.Default, parameterName));
        }
        public static string Print(this Expr expr) {
            return expr.Visit(SharpAlg.Native.Printer.ExpressionPrinter.Instance);
        }
        public static Expr Parse(this string expression, ExprBuilder builder = null) {
            return GetExpression(ParseCore(expression, builder ?? ExprBuilderFactory.CreateDefault()));
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
    }
}