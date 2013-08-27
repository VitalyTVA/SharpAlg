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

            "exp(0.0)".Parse()
                .AssertIsInteger()
                .AssertSimpleStringRepresentation("1");
            "exp(0)".Parse()
                .AssertIsInteger()
                .IsFloatEqual(x => x.Evaluate(), "1")
                .AssertSimpleStringRepresentation("1");
            "exp(1)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "2.718281")
                .AssertSimpleStringRepresentation("exp(1)");
            "exp(2)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "7.389056")
                .AssertSimpleStringRepresentation("exp(2)");
            "exp(-1)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "0.367879")
                .AssertSimpleStringRepresentation("exp(-1)");
            "exp(-2)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "0.135335")
                .AssertSimpleStringRepresentation("exp(-2)");

            "exp(x)".Parse()
                .AssertSimpleStringRepresentation("exp(x)")
                .Diff()
                    .AssertSimpleStringRepresentation("exp(x)");
            "exp(x^2)".Parse()
                .AssertSimpleStringRepresentation("exp(x ^ 2)")
                .Diff()
                    .AssertSimpleStringRepresentation("2 * x * exp(x ^ 2)");
            "exp(x!)".Parse()
                .AssertSimpleStringRepresentation("exp(x!)");

            "exp(ln(x))".Parse()
                .AssertSimpleStringRepresentation("x");
            "ln(exp(x))".Parse()
                .AssertSimpleStringRepresentation("x");
            "ln(exp(x^2))".Parse()
                .AssertSimpleStringRepresentation("x ^ 2");
            "ln(x^2)".Parse()
                .AssertSimpleStringRepresentation("2 * ln(x)");
            "ln(x^x)".Parse()
                .AssertSimpleStringRepresentation("x * ln(x)");

            "exp(2 * ln(x))".Parse()
                .AssertSimpleStringRepresentation("x ^ 2");

            "exp(2 * ln(x) ^ 2)".Parse()
                .AssertSimpleStringRepresentation("exp(2 * ln(x) ^ 2)");


            "exp(ln(x) * y * 2)".Parse()
                .AssertSimpleStringRepresentation("x ^ (2 * y)");

            "exp(z * ln(y) * ln(x) * 2)".Parse()
                .AssertSimpleStringRepresentation("y ^ (2 * z * ln(x))");

            "exp(ln(x^2))".Parse()
                .AssertSimpleStringRepresentation("x ^ 2");
            "exp(ln(x^x))".Parse()
                .AssertSimpleStringRepresentation("x ^ x");

        }
    }
}
