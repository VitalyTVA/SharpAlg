using SharpAlg.Native;
using SharpAlg.Native.Builder;
using System;
using RealPoint = System.Windows.Point;

namespace SharpAlg.Geo {
    public class Point {
        public static Point FromName(char name) {
            if(!char.IsUpper(name))
                throw new InvalidOperationException();
            return new Point(Expr.Parameter(name + "x"), Expr.Parameter(name + "y"));
        }
        public static Point FromValues(double x, double y) {
            return new Point(x.AsConst(), y.AsConst());
        }
        public readonly Expr X, Y;
        public Point(Expr x, Expr y) {
            this.X = x;
            this.Y = y;
        }
        public override string ToString() {
            return string.Format("({0}, {1})", X.Print(), Y.Print());
        }
    }

    public class Line {
        public static Line FromPoints(Point p1, Point p2) {
            var a = Expr.Subtract(p1.Y, p2.Y);
            var b = Expr.Subtract(p2.X, p1.X);
            var c = Expr.Subtract(Expr.Multiply(p1.X, p2.Y), Expr.Multiply(p2.X, p1.Y));
            return new Line(a, b, c);
        }
        public readonly Expr A, B, C;
        public Line(Expr a, Expr b, Expr c) {
            this.A = a;
            this.B = b;
            this.C = c;
        }
        public override string ToString() {
            return string.Format("({0})*x + ({1})*y + ({2}) = 0", A.Print(), B.Print(), C.Print());
        }
    }

    public class QuadraticEquation {
        public readonly Expr A, B, C;
        public QuadraticEquation(Expr a, Expr b, Expr c) {
            this.A = a;
            this.B = b;
            this.C = c;
        }
        public override string ToString() {
            return string.Format("({0})*x^2 + ({1})*x + ({2}) = 0", A.Print(), B.Print(), C.Print());
        }
    }

    public class Circle {
        public static Circle FromPoints(Point p1, Point p2) {
            var r = Expr.Add(
                        Expr.Subtract(p1.X, p2.X).Square(),
                        Expr.Subtract(p1.Y, p2.Y).Square()
                    ).Sqrt();
            return new Circle(p1.X, p1.Y, r);
        }
        public readonly Expr X, Y, R;
        public Circle(Expr x, Expr y, Expr r) {
            this.X = x;
            this.Y = y;
            this.R = r;
        }
        public override string ToString() {
            return string.Format("(x - ({0}))^2 + (y - ({1}))^2  = ({2})^2)", X.Print(), Y.Print(), R.Print());
        }
    }
    public static class LinesIntersector {
        public static Point Intersect(this Line l1, Line l2) {
            var context = ContextFactory.CreateEmpty()
                .Register("A1", l1.A)
                .Register("B1", l1.B)
                .Register("C1", l1.C)
                .Register("A2", l2.A)
                .Register("B2", l2.B)
                .Register("C2", l2.C);
            var builder = ExprBuilderFactory.Create(context);
            var divider = "A1*B2-A2*B1".Parse(builder);
            var x = "B1*C2-B2*C1".Parse(builder);
            var y = "C1*A2-C2*A1".Parse(builder);
            return new Point(Expr.Divide(x, divider), Expr.Divide(y, divider));
        }
        public static System.Tuple<Point,Point> Intersect(this Line l, Circle c) {
            var context = ContextFactory.CreateEmpty()
                .Register("A", l.A)
                .Register("B", l.B)
                .Register("C", l.C)
                .Register("X", c.X)
                .Register("Y", c.Y)
                .Register("R", c.R);
            //(B^2+A^2)*_Z^2+(2*X*A*B-2*Y*A^2+2*C*B)*_Z+2*X*A*C+X^2*A^2+C^2+Y^2*A^2-R^2*A^2
            var builder = ExprBuilderFactory.Create(context);
            var eqA = "B^2+A^2".Parse(builder);
            var eqYB = "2*X*A*B-2*Y*A^2+2*C*B".Parse(builder);
            var eqXB = "2*Y*A*B-2*X*B^2+2*C*A".Parse(builder);
            var eqYC = "2*X*A*C+X^2*A^2+C^2+Y^2*A^2-R^2*A^2".Parse(builder);
            var eqXC = "2*Y*B*C+Y^2*B^2+C^2+X^2*B^2-R^2*B^2".Parse(builder);
            var xRoots = new QuadraticEquation(eqA, eqXB, eqXC).Solve();
            var yRoots = new QuadraticEquation(eqA, eqYB, eqYC).Solve();
            return Tuple.Create(new Point(xRoots.Item1, yRoots.Item1), new Point(xRoots.Item2, yRoots.Item2));
        }
    }

    public static class QuadraticEquationHelper {
        public static System.Tuple<Expr, Expr> Solve(this QuadraticEquation eq) {
            var context = ContextFactory.CreateEmpty()
                 .Register("A", eq.A)
                 .Register("B", eq.B)
                 .Register("C", eq.C);
            var builder = ExprBuilderFactory.Create(context);
            var d = "(B^2-4*A*C)^(1/2)".Parse(builder);
            context.Register("D", d);
            var x1 = "(-B+D)/(2*A)".Parse(builder);
            var x2 = "(-B-D)/(2*A)".Parse(builder);
            return Tuple.Create(x1, x2);
        }
    }
    public static class ExprHelper {
        public static readonly Expr Half = "1/2".Parse();
        public static readonly Expr Two = "2".Parse();
        public static Expr Sqrt(this Expr e) {
            return Expr.Power(e, Half);
        }
        public static Expr Square(this Expr e) {
            return Expr.Power(e, Two);
        }
        //public static bool IsPrimitive(this Point p) {
        //    return p.X is ParameterExpr && p.Y is ParameterExpr;
        //}
        public static Context RegisterPoint(this Context context, Point p, double x, double y) {
            return context
                .RegisterValue(p.X, x)
                .RegisterValue(p.Y, y);
        }
        public static Context RegisterValue(this Context context, Expr parameter, double value) {
            return context
                .Register(((ParameterExpr)parameter).ParameterName, Expr.Constant(NumberFactory.FromDouble(value)));
        }
        public static Context RegisterValue(this Context context, string nane, double value) {
            return context.RegisterValue(nane.Parse(), value);
        }
        public static RealPoint ToRealPoint(this Point p, Context context) {
            return new RealPoint(p.X.ToReal(context), p.Y.ToReal(context));
        }
        public static double ToReal(this Expr expr, Context context) {
            return expr.Evaluate(context).ToDouble();
        }
        public static Expr AsConst(this double value) {
            return Expr.Constant(NumberFactory.FromDouble(value));
        }
    }
}