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
                .AssertValue(1, Expr.One);
            Parse("9 + 13")
                .AssertValue(22, Expr.Add(Expr.Constant(9), Expr.Constant(13)));
            Parse("9 + 13 + 117")
                .AssertValue(139, Expr.Add(Expr.Add(Expr.Constant(9), Expr.Constant(13)), Expr.Constant(117)));
            //Parse("x")
            //    .AssertSingleSyntaxError(GetNumberExpectedMessage(1));
            Parse("+")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(1));
            Parse("9+")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(3));
            Parse("9 + ")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(5));

            Parse("13 - 9")
                .AssertValue(4, Expr.Subtract(Expr.Constant(13), Expr.Constant(9)));
            Parse("130 - 9 - 2")
                .AssertValue(119, Expr.Subtract(Expr.Subtract(Expr.Constant(130), Expr.Constant(9)), Expr.Constant(2)));
            Parse("130 - 9 + 12 - 4")
                .AssertValue(129, Expr.Subtract(Expr.Add(Expr.Subtract(Expr.Constant(130), Expr.Constant(9)), Expr.Constant(12)), Expr.Constant(4)));
            Parse("13 -")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(5));

            Parse("2 * 3")
                .AssertValue(6, Expr.Multiply(Expr.Constant(2), Expr.Constant(3)));

            Parse("6 / 2")
                .AssertValue(3, Expr.Divide(Expr.Constant(6), Expr.Constant(2)));
            Parse("2 ^ 3")
                .AssertValue(8, Expr.Power(Expr.Constant(2), Expr.Constant(3)));
            Parse(".234")
                .AssertValue(.234, Expr.Constant(.234));
            Parse("0.234")
                .AssertValue(.234, Expr.Constant(.234));
            Parse("-0.234")
                .AssertValue(-.234);
            Parse("-.234")
                .AssertValue(-.234);
        }
        [Test]
        public void OperationsPriorityTest() {
            Parse("1 + 2 * 3")
                .AssertValue(7, Expr.Add(Expr.One, Expr.Multiply(Expr.Constant(2), Expr.Constant(3))));

            Parse("1 + 6 / 2")
                .AssertValue(4, Expr.Add(Expr.One, Expr.Divide(Expr.Constant(6), Expr.Constant(2))));

            Parse("2 * 3 * 4 / 6 / 2 - 4 / 2")
               .AssertValue(0);

            Parse("2 * 2 ^ 3")
                .AssertValue(16, Expr.Multiply(Expr.Constant(2), Expr.Power(Expr.Constant(2), Expr.Constant(3))));
            Parse("2 + 2 ^ 3")
                .AssertValue(10, Expr.Add(Expr.Constant(2), Expr.Power(Expr.Constant(2), Expr.Constant(3))));
        }
        [Test]
        public void ParenthesesTest() {
            Parse("(1 + 2) * 3")
                .AssertValue(9, Expr.Multiply(Expr.Add(Expr.One, Expr.Constant(2)), Expr.Constant(3)));
            Parse("(2 + 4) / (4 / (1 + 1))")
                .AssertValue(3);
        }
        [Test]
        public void ExpressionsWithParameterTest() {
            var context = new Context();
            context.Register("x", Expr.Constant(9));
            context.Register("someName", Expr.Constant(13));

            Parse("x")
                .AssertValue(9, Expr.Parameter("x"), context);

            Parse("x * someName")
                .AssertValue(117, Expr.Multiply(Expr.Parameter("x"), Expr.Parameter("someName")), context);

            Parse("(x - 4) * (someName + x)")
                .AssertValue(110, null, context);

            Parse("-x")
                .AssertValue(-9, Expr.Minus(Expr.Parameter("x")), context);
            Parse("-9")
                .AssertValue(-9, Expr.Minus(Expr.Constant(9)), context);
            Parse("-(x + 1)")
                .AssertValue(-10, Expr.Minus(Expr.Add(Expr.Parameter("x"), Expr.Constant(1))), context);
            Parse("-(x * 2)")
                .AssertValue(-18, Expr.Minus(Expr.Multiply(Expr.Parameter("x"), Expr.Constant(2))), context);
            Parse("--(x + 1)")
                .AssertValue(10, null, context);
            Parse("-(-(x + 1))")
                .AssertValue(10, null, context);
        }
        Parser Parse(string expression) {
            return ExpressionExtensions.ParseCore(expression, new TrivialExprBuilder());
        }
        static string GetNumberExpectedMessage(int column) {
            return ErrorsBase.GetErrorText(1, column, "invalid Terminal\r\n");
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class ParserTestHelper {
        public static Parser AssertValue(this Parser parser, double value, Expr expectedExpr = null, Context context = null) {
            return parser
                .IsEqual(x => x.errors.Errors, string.Empty)
                .IsEqual(x => x.errors.Count, 0)
                .IsEqual(x => x.Expr.Evaluate(context), value)
                .IsTrue(x => expectedExpr == null || x.Expr.ExprEquals(expectedExpr));
        }
        public static Parser AssertSingleSyntaxError(this Parser parser, string text) {
            return parser.IsEqual(x => x.errors.Count, 1).IsEqual(x => x.errors.Errors, text);
        }
    }
}
