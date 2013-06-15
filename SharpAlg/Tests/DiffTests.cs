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
        public void DiffConst() {
            Expression<Func<double, double>> expr = x => 13;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(0, 0, 0);
        }
        [Test]
        public void DiffX() {
            Expression<Func<double, double>> expr = x => x;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(1, 1, 1);
        }
        [Test]
        public void DiffSum() {
            Expression<Func<double, double>> expr = x => x + x;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(2, 2, 2);

            expr = x => x + 1;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(1, 1, 1);

            expr = x => x + 1 + x;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(2, 2, 2);
        }
        [Test]
        public void DiffMult() {
            Expression<Func<double, double>> expr = x => x * x;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(0, 2, 4);

            expr = x => 2 * x * x * x + x;
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(1, 7, 25);

            expr = x => (x + 1) * (x + 2);
            expr.Diff().Compile().Map(0, 1, 2).IsSequenceEqual(3, 5, 7);
        }
        [Test]
        public void DiffDivide() {
            Expression<Func<double, double>> expr = x => 1 / x;
            expr.Diff().Compile().Map(1, 2, 3).IsSequenceEqual(-1, -1.0 / 4, -1.0 / 9);

            expr = x => 1 / (x * x + x);
            expr.Diff().Compile().Map(1, 2).IsSequenceEqual(-3.0 / 4, -5.0 / 36);

            expr = x => (x * x + 1) / (x * x * x + 1);
            expr.Diff().Compile().Map(1, 2).IsSequenceEqual(-1.0 / 2, -8.0 / 27);
        }
    }
}
