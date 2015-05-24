using NUnit.Framework;
using SharpAlg.Native;
using System.Windows;
using RealPoint = System.Windows.Point;

namespace SharpAlg.Geo.Tests {
    [TestFixture]
    public class ProblemsTests {
        [Test, Explicit]
        public void MiddleOfLineSegment_Maple() {
            var res = GetMiddleOfLineSegmentZeroAssertion(Point.FromName('X'), Point.FromName('Y'));
            var mappleCommand = string.Format("simplify({0}); simplify({1});", res.X.Print(), res.Y.Print());
            //Clipboard.SetText(mappleCommand);
        }
        [Test, Explicit]
        public void AngleBisection_Maple() {
            //var res = GetAngleBisectionZeroAssertion(Point.FromName('A'), Point.FromName('B'), Point.FromName('C'));
            var res = GetAngleBisectionZeroAssertion(new Point(Expr.Zero, Expr.Zero), Point.FromName('B'), Point.FromName('C'));
            var mappleCommand = string.Format("simplify({0});", res.Print());
            //Clipboard.SetText(mappleCommand);
        }
        Point GetMiddleOfLineSegmentZeroAssertion(Point p1, Point p2) {
            var l1 = Line.FromPoints(p1, p2);

            var c1 = Circle.FromPoints(p1, p2);
            var c2 = Circle.FromPoints(p2, p1);

            var c1_c2 = c1.Intersect(c2);
            var l2 = Line.FromPoints(c1_c2.Item1, c1_c2.Item2);

            var l1_l2 = l1.Intersect(l2);

            var expected = ExprHelper.Middle(p1, p2);
            return l1_l2.Offset(expected.Invert());

        }
        Expr GetAngleBisectionZeroAssertion(Point A, Point B, Point C) {
            var l1 = Line.FromPoints(A, B);
            var l2 = Line.FromPoints(A, C);

            var c = Circle.FromPoints(A, C);
            var c_l2 = l1.Intersect(c).Item1;

            var middle = ExprHelper.Middle(C, c_l2);//TODO make real
            var bisectrissa = Line.FromPoints(A, middle);

            return Expr.Add(LinesOperations.TangentBetween(l1, bisectrissa), LinesOperations.TangentBetween(l2, bisectrissa));
        }
    }
}
