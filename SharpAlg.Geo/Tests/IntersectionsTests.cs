using NUnit.Framework;
using SharpAlg.Native;

namespace SharpAlg.Geo.Tests {
    [TestFixture]
    public class IntersectionsTests {
        [Test]
        public void LinesIntersetions() {
            var p1 = Point.FromName('A');
            var p2 = Point.FromName('B');
            var p3 = Point.FromName('C');
            var p4 = Point.FromName('D');
            var l1 = Line.FromPoins(p1, p2);
            var l2 = Line.FromPoins(p3, p4);
            var x = l1.Intersect(l2);
        }
    }
}