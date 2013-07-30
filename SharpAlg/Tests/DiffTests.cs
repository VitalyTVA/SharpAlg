﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class DiffTests {
        [Test]
        public void DiffEvaluateTest() {
            "13".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 0, 0, 0 });
            "x".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 1, 1, 1 });
            "x + x".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 2, 2, 2 });
            "x + 1".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 1, 1, 1 });
            "x + 1 + x".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 2, 2, 2 });
            "x * x - x".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { -1, 1, 3 });
            "x * x".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 0, 2, 4 });
            "2 * x * x * x + x".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 1, 7, 25 });
            "(x + 1) * (x + 2)".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 3, 5, 7 });
            "(x * x) / x".Parse().Diff().AssertEvaluatedValues(new double[] { 1, 2, 3 }, new double[] { 1, 1, 1 });
            "36 / x".Parse().Diff().AssertEvaluatedValues(new double[] { 1, 2, 3 }, new double[] { -36, -9, -4 });
            "36 / (x * x + x)".Parse().Diff().AssertEvaluatedValues(new double[] { 1, 2 }, new double[] { -3.0 * 9, -5.0 });
            "27 * (x * x + 1) / (x * x * x + 1)".Parse().Diff().AssertEvaluatedValues(new double[] { 1, 2 }, new double[] { -1.0 * 27 / 2, -8.0 });

            "(x * x) ^ 3".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 0, 6, 6 * 2 * 2 * 2 * 2 * 2 });
            "(x * x) ^ (1 + 2)".Parse().Diff().AssertEvaluatedValues(new double[] { 0, 1, 2 }, new double[] { 0, 6, 6 * 2 * 2 * 2 * 2 * 2 });
        }
        [Test]
        public void DiffSimplifyTest() {
            "1 + x".Parse().Diff().AssertSimpleStringRepresentation("1");
            "x + 1".Parse().Diff().AssertSimpleStringRepresentation("1");
            "x + x".Parse().Diff().AssertSimpleStringRepresentation("2");
            "x + x + x".Parse().Diff().AssertSimpleStringRepresentation("3");
            "x * 2".Parse().Diff().AssertSimpleStringRepresentation("2");
            "2 * x".Parse().Diff().AssertSimpleStringRepresentation("2");
            "x * x + 1".Parse().Diff().AssertSimpleStringRepresentation("2 * x");
            "1 + x * x + 1".Parse().Diff().AssertSimpleStringRepresentation("2 * x");

            "1 / x".Parse().Diff().AssertSimpleStringRepresentation("-x ^ (-2)");

            "x / x".Parse().Diff().AssertSimpleStringRepresentation("0");
            "(x * x) / x".Parse().Diff().AssertSimpleStringRepresentation("1");

            "2 ^ 3".Parse().Diff().AssertSimpleStringRepresentation("0");
            "(x + x) ^ 1".Parse().Diff().AssertSimpleStringRepresentation("2");
            "x ^ 2".Parse().Diff().AssertSimpleStringRepresentation("2 * x");
            "(x * x) ^ 3".Parse().Diff().AssertSimpleStringRepresentation("6 * x ^ 5");
        }
        [Test]
        public void DiffMultiParametersTest() {
            "x".Parse().Diff("y").AssertSimpleStringRepresentation("0");
            "x + y".Parse().Diff("y").AssertSimpleStringRepresentation("1");
            "x * y".Parse().Diff("y").AssertSimpleStringRepresentation("x");
            "x * y".Parse().Diff("x").AssertSimpleStringRepresentation("y");
            "x^2 * y^3".Parse().Diff("x").AssertSimpleStringRepresentation("2 * x * y ^ 3");
            "x^2 * y^3".Parse().Diff("y").AssertSimpleStringRepresentation("3 * x ^ 2 * y ^ 2");
            "x + y".Parse().Fails(x => x.Diff(), typeof(ExpressionDefferentiationException));
        }
    }
}