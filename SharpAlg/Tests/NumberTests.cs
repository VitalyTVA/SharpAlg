using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;
using SharpAlg.Native.Builder;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class NumberTests {
        [Test]
        public void IntOperationsTest() {
            (1.AsNumber() + 2.AsNumber()).IsEqual(3.AsNumber());
            (9.AsNumber() * 13.AsNumber()).IsEqual(117.AsNumber());
            (1593668734.AsNumber() / 1287293.AsNumber()).IsEqual(1238.AsNumber());
            (117.AsNumber() ^ 5.AsNumber()).IsEqual(21924480357.AsNumber());
            (1000000001.AsNumber() + 500000001.AsNumber()).IsEqual(1500000002.AsNumber());
            ((-1000000001).AsNumber() + 500000001.AsNumber()).IsEqual((-500000000).AsNumber());

            (100000000001.AsNumber() == 100000000001.AsNumber()).IsTrue();
            (100000000001.AsNumber() == 100000000000.AsNumber()).IsFalse();
            (100000000001.AsNumber() != 100000000000.AsNumber()).IsTrue();
            (100000000001.AsNumber() != 100000000001.AsNumber()).IsFalse();

            (100000000001.AsNumber() < 100000000001.AsNumber()).IsFalse();
            (100000000001.AsNumber() < 100000000002.AsNumber()).IsTrue();
            (100000000001.AsNumber() <= 100000000001.AsNumber()).IsTrue();
            (100000000002.AsNumber() <= 100000000001.AsNumber()).IsFalse();

            (100000000001.AsNumber() > 100000000001.AsNumber()).IsFalse();
            (100000000002.AsNumber() > 100000000001.AsNumber()).IsTrue();
            (100000000001.AsNumber() >= 100000000001.AsNumber()).IsTrue();
            (100000000001.AsNumber() >= 100000000002.AsNumber()).IsFalse();

            //(1000000001.AsNumber() * 500000001.AsNumber()).IsEqual(500000001500000001.AsNumber());
            //(2.AsNumber() ^ 50.AsNumber()).IsEqual(1500000002.AsNumber());
        }
    }
}
