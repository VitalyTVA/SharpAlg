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
    [TestFixture]
    public class ParserTests {
        [Test]
        public void ParseNumeric() {
            Parse("9 + 13")
                .IsEqual(x => x.errors.Count, 0);
            Parse("9 + 13 + 117")
                .IsEqual(x => x.errors.Count, 0);
            Parse("x")
                .IsEqual(x => x.errors.Count, 1)
                .IsEqual(x => x.errors.Errors, ErrorsBase.GetErrorText(1, 1, "number expected\r\n"));
            Parse("+")
                .IsEqual(x => x.errors.Count, 1)
                .IsEqual(x => x.errors.Errors, ErrorsBase.GetErrorText(1, 1, "number expected\r\n"));
            Parse("9 + ")
                .IsEqual(x => x.errors.Count, 1)
                .IsEqual(x => x.errors.Errors, ErrorsBase.GetErrorText(1, 5, "number expected\r\n"));
        }
        Parser Parse(string expression) {
            Scanner scanner = new Scanner(new MemoryStream(Encoding.ASCII.GetBytes(expression)));
            Parser parser = new Parser(scanner);
            parser.Parse();
            return parser;
        }
    }
}
