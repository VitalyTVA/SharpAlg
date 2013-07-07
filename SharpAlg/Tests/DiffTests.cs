using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;

namespace SharpAlg.Tests {
    [TestFixture]
    public class DiffTests {
        [Test]
        public void DiffEvaluateTest() {
            "13".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(0, 0, 0);
            "x".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(1, 1, 1);
            "x + x".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(2, 2, 2);
            "x + 1".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(1, 1, 1);
            "x + 1 + x".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(2, 2, 2);
            "x * x - x".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(-1, 1, 3);
            "x * x".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(0, 2, 4);
            "2 * x * x * x + x".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(1, 7, 25);
            "(x + 1) * (x + 2)".Parse().Diff().AsEvaluator().Map(0, 1, 2).IsSequenceEqual(3, 5, 7);
            "1 / x".Parse().Diff().AsEvaluator().Map(1, 2, 3).IsSequenceEqual(-1, -1.0 / 4, -1.0 / 9);
            "1 / (x * x + x)".Parse().Diff().AsEvaluator().Map(1, 2).IsSequenceEqual(-3.0 / 4, -5.0 / 36);
            "(x * x + 1) / (x * x * x + 1)".Parse().Diff().AsEvaluator().Map(1, 2).IsSequenceEqual(-1.0 / 2, -8.0 / 27);
        }
        [Test]
        public void DiffSimplifyTest() {
            "1 + x".Parse().Diff().AssertSimpleStringRepresentation("1");
            "x + 1".Parse().Diff().AssertSimpleStringRepresentation("1");
            "x + x".Parse().Diff().AssertSimpleStringRepresentation("2");
            "x + x + x".Parse().Diff().AssertSimpleStringRepresentation("3");
            "x * 2".Parse().Diff().AssertSimpleStringRepresentation("2");
            "2 * x".Parse().Diff().AssertSimpleStringRepresentation("2");
            "x * x + 1".Parse().Diff().AssertSimpleStringRepresentation("(2 * x)");
            "1 + x * x + 1".Parse().Diff().AssertSimpleStringRepresentation("(2 * x)");
        }
    }
}