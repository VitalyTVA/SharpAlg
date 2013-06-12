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
        public void PolinomDiff1() {
            Expression<Func<double, double>> expr = x => x * x;
            var compiled = expr.Diff().Compile();
            //Assert.AreEqual(2, compiled(1));
            //Assert.AreEqual(4, compiled(2));
        }
        //[Test]
        //public void PolinomDiff2() {
        //    Expression<Func<double, double>> expr = x => x * x + x + Math.PI;
        //    var compiled = expr.Diff().Compile();
        //    Assert.AreEqual(2, compiled(1));
        //    Assert.AreEqual(4, compiled(2));
        //}

    }
}
