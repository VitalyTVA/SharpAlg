using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;

namespace SharpAlg.Tests {
    [TestFixture]
    public class DiffTests {
        [Test]
        public void Diff1() {
            Expression<Func<double, double>> expr = x => 13;
            expr.Diff().Compile()
                .AreEqual(x => x(0), 0)
                .AreEqual(x => x(1), 0)
                .AreEqual(x => x(2), 0);
        }

        //[Test]
        //public void Diff1() {
        //    Expression<Func<double, double>> expr = x => x * x;
        //    expr.Diff().Compile()
        //        .AreEqual(x => x(1), 2)
        //        .AreEqual(x => x(2), 4);
        //}
        //[Test]
        //public void PolinomDiff2() {
        //    Expression<Func<double, double>> expr = x => x * x + x + Math.PI;
        //    var compiled = expr.Diff().Compile();
        //    Assert.AreEqual(2, compiled(1));
        //    Assert.AreEqual(4, compiled(2));
        //}

    }
}
