using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;
using System.IO;
using System.Text;
using SharpAlg.Native.Parser;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class ParserTests {
        [Test]
        public void ParseNumericTest() {
            Parse("1")
                .AssertValue(1, Expr.Constant(1));
            Parse("9 + 13")
                .AssertValue(22, Expr.Binary(Expr.Constant(9), Expr.Constant(13), BinaryOperation.Add));
            Parse("9 + 13 + 117")
                .AssertValue(139, Expr.Binary(Expr.Binary(Expr.Constant(9), Expr.Constant(13), BinaryOperation.Add), Expr.Constant(117), BinaryOperation.Add));
            //Parse("x")
            //    .AssertSingleSyntaxError(GetNumberExpectedMessage(1));
            Parse("+")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(1));
            Parse("9+")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(3));
            Parse("9 + ")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(5));

            Parse("13 - 9")
                .AssertValue(4, Expr.Binary(Expr.Constant(13), Expr.Constant(9), BinaryOperation.Subtract));
            Parse("130 - 9 - 2")
                .AssertValue(119, Expr.Binary(Expr.Binary(Expr.Constant(130), Expr.Constant(9), BinaryOperation.Subtract), Expr.Constant(2), BinaryOperation.Subtract));
            Parse("130 - 9 + 12 - 4")
                .AssertValue(129, Expr.Binary(Expr.Binary(Expr.Binary(Expr.Constant(130), Expr.Constant(9), BinaryOperation.Subtract), Expr.Constant(12), BinaryOperation.Add), Expr.Constant(4), BinaryOperation.Subtract));
            Parse("13 -")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(5));

            Parse("2 * 3")
                .AssertValue(6);

            Parse("6 / 2")
                .AssertValue(3);

            //Parse("1 + 2 * 3")
            //    .AssertValue(7);

            //Parse("6 / 2")
            //    .AssertValue(3);
        }
        Parser Parse(string expression) {
            Scanner scanner = new Scanner(expression);
            Parser parser = new Parser(scanner);
            parser.Parse();
            return parser;
        }
        static string GetNumberExpectedMessage(int column) {
            return ErrorsBase.GetErrorText(1, column, "number expected\r\n");
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class ParserTestHelper {
        public static Parser AssertValue(this Parser parser, int value, Expr expectedExpr = null) {
            return parser
                .IsEqual(x => x.errors.Errors, string.Empty)
                .IsEqual(x => x.errors.Count, 0)
                .IsEqual(x => x.Expr.Evaluate(), value)
                .IsTrue(x => expectedExpr == null || x.Expr.ExprEquals(expectedExpr));
        }
        public static Parser AssertSingleSyntaxError(this Parser parser, string text) {
            return parser.IsEqual(x => x.errors.Count, 1).IsEqual(x => x.errors.Errors, text);
        }
    }
}
