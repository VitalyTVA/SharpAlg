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
        public static Line FromPoins(Point p1, Point p2) {
            var a = Expr.Subtract(p1.Y, p2.Y);
            var b = Expr.Subtract(p2.X, p1.X);
            var c = Expr.Subtract(Expr.Multiply(p1.X, p2.Y), Expr.Multiply(p2.X, p1.Y));
            return new Line(a, b, c);
        }
        public readonly Expr A, B, C;
        Line(Expr a, Expr b, Expr c) {
            this.A = a;
            this.B = b;
            this.C = c;
        }
        public override string ToString() {
            return string.Format("({0})*x + ({1})*y + ({2}) = 0", A.Print(), B.Print(), C.Print());
        }
    }

    public class Circle {
        public static Circle FromPoint(Point p1, Point p2) {
            var r = Expr.Add(
                        Expr.Subtract(p1.X, p2.X).Square(),
                        Expr.Subtract(p1.Y, p2.Y).Square()
                    ).Sqrt();
            return new Circle(p1.X, p1.Y, r);
        }
        public readonly Expr X, Y, R;
        Circle(Expr x, Expr y, Expr r) {
            this.X = x;
            this.Y = y;
            this.R = r;
        }
        public override string ToString() {
            return string.Format("(x - ({0}))^2 + (y - ({1}))^2  = ({2})^2)", X.Print(), X.Print(), R.Print());
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
            //if(!p.IsPrimitive())
            //    throw new InvalidOperationException();
            return context
                .Register(((ParameterExpr)p.X).ParameterName, Expr.Constant(NumberFactory.FromDouble(x)))
                .Register(((ParameterExpr)p.Y).ParameterName, Expr.Constant(NumberFactory.FromDouble(y)));
        }
        public static RealPoint ToRealPoint(this Point p, Context context) {
            var x = p.X.Evaluate(context).ToDouble();
            var y = p.Y.Evaluate(context).ToDouble();
            return new RealPoint(x, y);
        }
    }
}