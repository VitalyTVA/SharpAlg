using NUnit.Framework;
using SharpAlg.Native;
using RealPoint = System.Windows.Point;

namespace SharpAlg.Geo.Tests {
    [TestFixture]
    public class IntersectionsTests {
        [Test]
        public void LinesIntersetions() {
            var A = Point.FromName('A');
            var B = Point.FromName('B');
            var C = Point.FromName('C');
            var D = Point.FromName('D');
            var m = Line.FromPoins(A, B);
            var n = Line.FromPoins(C, D);
            var context = ContextFactory.CreateEmpty()
                .RegisterPoint(A, 2, 1)
                .RegisterPoint(B, 8, 4)
                .RegisterPoint(C, 3, 5)
                .RegisterPoint(D, 5, -1);
            var X = m.Intersect(n);
            Assert.AreEqual(new RealPoint(4, 2), X.ToRealPoint(context));
        }
    }
}