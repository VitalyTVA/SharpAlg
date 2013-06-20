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
                .AssertValue(1);
            Parse("9 + 13")
                .AssertValue(22);
            Parse("9 + 13 + 117")
                .AssertValue(139);
            Parse("x")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(1, 1));
            Parse("+")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(1, 1));
            Parse("9+")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(1, 3));
            Parse("9 + ")
                .AssertSingleSyntaxError(GetNumberExpectedMessage(1, 5));
        }
        Parser Parse(string expression) {
            Scanner scanner = new Scanner(expression);
            Parser parser = new Parser(scanner);
            parser.Parse();
            return parser;
        }
        static int Evaluate(Parser x) {
            return x.result;
        }
        static string GetNumberExpectedMessage(int row, int column) {
            return ErrorsBase.GetErrorText(row, column, "number expected\r\n");
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class ParserTestHelper {
        public static Parser AssertValue(this Parser parser, int value) {
            return parser.IsEqual(x => x.errors.Count, 0).IsEqual(x => x.result, value);
        }
        public static Parser AssertSingleSyntaxError(this Parser parser, string text) {
            return parser.IsEqual(x => x.errors.Count, 1).IsEqual(x => x.errors.Errors, text);
        }
    }
}
