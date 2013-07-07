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
        public static bool ExprEquals(this Expr expr1, Expr expr2) {
            return expr1.Visit(new ExpressionComparer(expr2));
        }
        public static string Print(this Expr expr) {
            return expr.Visit(new ExpressionPrinter());
        }
        public static Expr Parse(this string expression) {
            Parser.Parser parser = ParseCore(expression);
            if(parser.errors.Count > 0)
                throw new InvalidOperationException("String can not be parsed"); //TODO message
            return parser.Expr;
        }
        internal static Parser.Parser ParseCore(string expression) {
            Scanner scanner = new Scanner(expression);
            Parser.Parser parser = new Parser.Parser(scanner);
            parser.Parse();
            return parser;
        }

    }
}