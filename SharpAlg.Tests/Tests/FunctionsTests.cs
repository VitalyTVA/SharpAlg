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
        const string STR_PiIsAConstantAndCantBeUsedAsFunction = "Pi is a constant and can't be used as function\r\n";
        [Test]
        public void PiTest() {
            "Pi".Parse().IsFloatEqual(x => x.Evaluate(), "3.14159");
            "Pi()".GetParser().AssertSingleSyntaxError(STR_PiIsAConstantAndCantBeUsedAsFunction);
            "Pi(1)".GetParser().AssertSingleSyntaxError(STR_PiIsAConstantAndCantBeUsedAsFunction);
            "Pi".Parse().Diff().AssertSimpleStringRepresentation("0").AssertIsInteger();
            "Pi".Parse().AssertSimpleStringRepresentation("Pi");
            "Pi + 1.0".Parse().AssertSimpleStringRepresentation("Pi + 1");
        }
        [Test]
        public void TrigonometryTest() {
            "sin(1)".Parse().IsFloatEqual(x => x.Evaluate(), "0.84147").AssertSimpleStringRepresentation("sin(1)");
            "sin(1.0)".Parse().IsFloatEqual(x => x.Print(), "0.84147");
            //"cos(1)".Parse().IsFloatEqual(x => x.Evaluate(), "0.54030").AssertSimpleStringRepresentation("cos(1)");
            //"sin(x)".Parse().Diff().AssertSimpleStringRepresentation("cos(x)");
        }
        [Test]
        public void ExpTest() {
            Expr.Function("exp", new Expr[] { "1".Parse(), "2".Parse() }).Fails(x => x.Diff(), typeof(InvalidArgumentCountException));

            "exp(0.0)".Parse()
                .AssertIsFloat()
                .AssertSimpleStringRepresentation("1");
            "exp(0)".Parse()
                .AssertIsInteger()
                .IsFloatEqual(x => x.Evaluate(), "1")
                .AssertSimpleStringRepresentation("1");
            "exp(1) + 1.0".Parse()
                .IsFloatEqual(x => x.Evaluate(), "3.718281")
                .AssertSimpleStringRepresentation("exp(1) + 1");
            "exp(2)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "7.389056")
                .AssertSimpleStringRepresentation("exp(2)");
            "exp(2.0)".Parse()
                .IsFloatEqual(x => x.Print(), "7.389056");
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
