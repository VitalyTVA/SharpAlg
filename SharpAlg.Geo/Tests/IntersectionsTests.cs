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
            var m = Line.FromPoints(A, B);
            var n = Line.FromPoints(C, D);
            var X = m.Intersect(n);
            var context = ContextFactory.CreateEmpty()
                .RegisterPoint(A, 2, 1)
                .RegisterPoint(B, 8, 4)
                .RegisterPoint(C, 3, 5)
                .RegisterPoint(D, 5, -1); 
            Assert.AreEqual(new RealPoint(4, 2), X.ToRealPoint(context));
        }
        [Test]
        public void LineAndCircleIntersection() {
            var A = Point.FromName('A');
            var B = Point.FromName('B');
            var C = Point.FromName('C');
            var D = Point.FromName('D');
            var l = Line.FromPoints(A, B);
            var c = Circle.FromPoints(C, D);
            var X = l.Intersect(c);
            var context = ContextFactory.CreateEmpty()
                .RegisterPoint(A, -9, 5)
                .RegisterPoint(B, 12, 8)
                .RegisterPoint(C, 2, 3)
                .RegisterPoint(D, 6, 6);
            Assert.AreEqual(new RealPoint(5, 7), X.Item1.ToRealPoint(context));
            Assert.AreEqual(new RealPoint(-2, 6), X.Item2.ToRealPoint(context));

            //int X1 = 0;
            //int Y1 = 0;
            //int X2 = 0;
            //int Y2 = 0;
            //int R1 = 0;
            //int R2 = 0;
            //int _Z = 0;
            //var x = 1 / 2 * (-2 * Y2 * 
            //    RootOf(
            //        (4 * X2 ^ 2 + 4 * Y2 ^ 2) * _Z ^ 2 
            //        + (-4 * Y2 ^ 3 - 4 * R1 ^ 2 * Y2 + 4 * Y2 * R2 ^ 2 - 4 * X2 ^ 2 * Y2) * _Z 
            //        + X2 ^ 4 + R1 ^ 4 - 2 * Y2 ^ 2 * R2 ^ 2 + 2 * X2 ^ 2 * Y2 ^ 2 - 2 * X2 ^ 2 * R2 ^ 2 + Y2 ^ 4 + R2 ^ 4 + 2 * R1 ^ 2 * Y2 ^ 2 - 2 * R1 ^ 2 * R2 ^ 2 - 2 * R1 ^ 2 * X2 ^ 2
            //    ) + R1 ^ 2 + X2 ^ 2 + Y2 ^ 2 - R2 ^ 2) / X2;

            //var y = RootOf(
            //        (4 * X2 ^ 2 + 4 * Y2 ^ 2) * _Z ^ 2 
            //        + (-4 * Y2 ^ 3 - 4 * R1 ^ 2 * Y2 + 4 * Y2 * R2 ^ 2 - 4 * X2 ^ 2 * Y2) * _Z 
            //        + X2 ^ 4 + R1 ^ 4 - 2 * Y2 ^ 2 * R2 ^ 2 + 2 * X2 ^ 2 * Y2 ^ 2 - 2 * X2 ^ 2 * R2 ^ 2 + Y2 ^ 4 + R2 ^ 4 + 2 * R1 ^ 2 * Y2 ^ 2 - 2 * R1 ^ 2 * R2 ^ 2 - 2 * R1 ^ 2 * X2 ^ 2
            //    );
        }
        [Test]
        public void CirclesIntersection1() {
            AssertCirclesIntersection(
                new RealPoint(0, 0),
                new RealPoint(0, 5),
                new RealPoint(7, 7),
                new RealPoint(7, 2),
                new RealPoint(4, 3),
                new RealPoint(3, 4)
            );
        }
        [Test]
        public void CirclesIntersection2() {
            AssertCirclesIntersection(
                new RealPoint(0, 0),
                new RealPoint(0, 5),
                new RealPoint(8, 0),
                new RealPoint(3, 0),
                new RealPoint(4, -3),
                new RealPoint(4, 3)
            );
        }
        [Test]
        public void CirclesIntersection3() {
            AssertCirclesIntersection(
                new RealPoint(0 + 1, 0 + 2),
                new RealPoint(0 + 1, 5 + 2),
                new RealPoint(7 + 1, 7 + 2),
                new RealPoint(7 + 1, 2 + 2),
                new RealPoint(4 + 1, 3 + 2),
                new RealPoint(3 + 1, 4 + 2)
            );
        }
        [Test]
        public void CirclesIntersection4() {
            AssertCirclesIntersection(
                new RealPoint(-1, 2),
                new RealPoint(-1, -2),
                new RealPoint(3, -1),
                new RealPoint(3, 4),
                new RealPoint(2.4796363335788034, -1.8928484447717376042),
                new RealPoint(-1.9196363335788034, 3.9728484447717376042)
            );
        }
        void AssertCirclesIntersection(RealPoint a, RealPoint b, RealPoint c, RealPoint d, RealPoint x1, RealPoint x2) {
            var A = Point.FromName('A');
            var B = Point.FromName('B');
            var C = Point.FromName('C');
            var D = Point.FromName('D');
            var c1 = Circle.FromPoints(A, B);
            var c2 = Circle.FromPoints(C, D);
            var X = c1.Intersect(c2);
            var context = ContextFactory.CreateEmpty()
                .RegisterPoint(A, a.X, a.Y)
                .RegisterPoint(B, b.X, b.Y)
                .RegisterPoint(C, c.X, c.Y)
                .RegisterPoint(D, d.X, d.Y);
            AssertHelper.ArePointsEqual(x1, X.Item1.ToRealPoint(context));
            AssertHelper.ArePointsEqual(x2, X.Item2.ToRealPoint(context));
        }
        //int RootOf(int x) {
        //    return x;
        //}
        //[Test]
        //public void LineAndCircleIntersection2() {
        //    var A = Point.FromValues(-9, 5);
        //    var B = Point.FromValues(12, 8);
        //    var C = Point.FromValues(2, 3);
        //    var D = Point.FromValues(6, 6);
        //    var l = Line.FromPoints(A, B).With(x => new Line(Expr.Constant(x.A.Evaluate()), Expr.Constant(x.B.Evaluate()), Expr.Constant(x.C.Evaluate())));
        //    var c = Circle.FromPoints(C, D).With(x => new Circle(Expr.Constant(x.X.Evaluate()), Expr.Constant(x.Y.Evaluate()), Expr.Constant(x.R.Evaluate())));
        //    var X = l.Intersect(c);
        //    var context = ContextFactory.CreateEmpty();
        //    Assert.AreEqual(new RealPoint(5, 7), X.Item1.ToRealPoint(context));
        //}
        [Test]
        public void QuadraticEquation() {
            var eq = new QuadraticEquation("A".Parse(), "B".Parse(), "C".Parse());
            var roots = eq.Solve();
            var context = ContextFactory.CreateEmpty()
                .RegisterValue("A".Parse(), 1)
                .RegisterValue("B".Parse(), -2)
                .RegisterValue("C".Parse(), -3);
            Assert.AreEqual(3, roots.Item1.ToReal(context));
            Assert.AreEqual(-1, roots.Item2.ToReal(context));

            context = ContextFactory.CreateEmpty()
                .RegisterValue("A".Parse(), 2)
                .RegisterValue("B".Parse(), -4)
                .RegisterValue("C".Parse(), -6);
            Assert.AreEqual(3, roots.Item1.ToReal(context));
            Assert.AreEqual(-1, roots.Item2.ToReal(context));

            eq = new QuadraticEquation("X + 1".Parse(), "Y - 2".Parse(), "Z / 2".Parse());
            roots = eq.Solve();
            context = ContextFactory.CreateEmpty()
                .RegisterValue("X".Parse(), 0)
                .RegisterValue("Y".Parse(), 0)
                .RegisterValue("Z".Parse(), -6);
            Assert.AreEqual(3, roots.Item1.ToReal(context));
            Assert.AreEqual(-1, roots.Item2.ToReal(context));
        }
    }
    public static class AssertHelper {
        public const double Delta = 0.0000000001;
        public static void ArePointsEqual(RealPoint p1, RealPoint p2, double delta = Delta) {
            Assert.AreEqual(p1.X, p2.X, delta);
            Assert.AreEqual(p1.Y, p2.Y, delta);        
        }
    }
}