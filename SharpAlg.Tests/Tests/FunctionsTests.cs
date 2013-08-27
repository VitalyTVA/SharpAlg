using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;
using SharpAlg.Native.Builder;
using SharpAlg.Native.Parser;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class FunctionsTests {
        [Test]
        public void ExpTest() {
            Expr.Function("exp", new Expr[] { "1".Parse(), "2".Parse() }).Fails(x => x.Diff(), typeof(InvalidArgumentCountException));

            "exp(1)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "2.718281")
                .AssertSimpleStringRepresentation("e");
            "exp(2)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "7.389056")
                .AssertSimpleStringRepresentation("e ^ 2");
            "exp(-1)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "0.367879")
                .AssertSimpleStringRepresentation("1 / e");
            "exp(-2)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "0.135335")
                .AssertSimpleStringRepresentation("e ^ (-2)");
            "exp(x)".Parse().AssertSimpleStringRepresentation("e ^ x").Diff().AssertSimpleStringRepresentation("e ^ x");
            "exp(x^2)".Parse().AssertSimpleStringRepresentation("e ^ (x ^ 2)").Diff().AssertSimpleStringRepresentation("2 * x * e ^ (x ^ 2)");

        }
    }
}
