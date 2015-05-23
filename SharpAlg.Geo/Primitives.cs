using SharpAlg.Native;
using SharpAlg.Native.Builder;
using System;
using System.Collections.Immutable;
using System.Linq;
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
            return new Line(a, b, c).FMap(x => x.Convolute());
        }
        public readonly Expr A, B, C;
        public Line(Expr a, Expr b, Expr c) {
            this.A = a;
            this.B = b;
            this.C = c;
        }
        public override string ToString() {
            var context = ImmutableContext.Empty.RegisterLine(this, "A", "B", "C");
            return "A*x + B*y + C".Parse().Substitute(context).Print();
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
                    );
            return new Circle(p1.X, p1.Y, r);
        }
        public readonly Expr X, Y, R;
        public Point Center { get { return new Point(X, Y); } }
        public Circle(Expr x, Expr y, Expr r) {
            this.X = x;
            this.Y = y;
            this.R = r;
        }
        public override string ToString() {
            return string.Format("(x - ({0}))^2 + (y - ({1}))^2 = {2})", X.Print(), Y.Print(), R.Print());
        }
    }
    public static class LinesIntersector {
        static readonly Point Intersection;
        static LinesIntersector() {
            const string divider = "(A1*B2-A2*B1)";
            var x = ("(B1*C2-B2*C1)/" + divider).Parse(ExprBuilderFactory.CreateEmpty());
            var y = ("(C1*A2-C2*A1)/" + divider).Parse(ExprBuilderFactory.CreateEmpty());
            Intersection = new Point(x, y);
        }

        public static Point Intersect(this Line l1, Line l2) {
            var context = ImmutableContext.Empty
                .RegisterLine(l1, "A1", "B1", "C1")
                .RegisterLine(l2, "A2", "B2", "C2");
            return Intersection.Substitute(context);
        }
    }
    public static class LineCircleIntersector {
        static readonly System.Tuple<Point, Point> Intersections;
        static LineCircleIntersector() {
            var eqA = "B^2+A^2".Parse();
            var eqYB = "2*X*A*B-2*Y*A^2+2*C*B".Parse();
            var eqXB = "2*Y*A*B-2*X*B^2+2*C*A".Parse();
            var eqYC = "2*X*A*C+X^2*A^2+C^2+Y^2*A^2-R*A^2".Parse();
            var eqXC = "2*Y*B*C+Y^2*B^2+C^2+X^2*B^2-R*B^2".Parse();
            var xRoots = new QuadraticEquation(eqA, eqXB, eqXC).Solve();
            var yRoots = new QuadraticEquation(eqA, eqYB, eqYC).Solve();
            Intersections = Tuple.Create(new Point(xRoots.Item1, yRoots.Item1), new Point(xRoots.Item2, yRoots.Item2));
        }
        public static System.Tuple<Point,Point> Intersect(this Line l, Circle c) {
            var context = ImmutableContext.Empty
                .RegisterLine(l, "A", "B", "C")
                .Register("X", c.X)
                .Register("Y", c.Y)
                .Register("R", c.R);
            return Intersections.Substitute(context);
        }
    }
    public static class CirclesIntersector {
        static readonly System.Tuple<Point, Point> Intersections;
        static CirclesIntersector() {
            var eqA = "4*X0^2+4*Y0^2".Parse();
            var eqYB = "-4*Y0^3-4*R1*Y0+4*Y0*R2-4*X0^2*Y0".Parse();
            var eqXB = "-4*X0^3-4*R1*X0+4*X0*R2-4*Y0^2*X0".Parse();
            var eqYC = "X0^4+R1^2-2*Y0^2*R2+2*X0^2*Y0^2-2*X0^2*R2+Y0^4+R2^2+2*R1*Y0^2-2*R1*R2-2*R1*X0^2".Parse();
            var eqXC = "Y0^4+R1^2-2*X0^2*R2+2*Y0^2*X0^2-2*Y0^2*R2+X0^4+R2^2+2*R1*X0^2-2*R1*R2-2*R1*Y0^2".Parse();
            var xRoots = new QuadraticEquation(eqA, eqXB, eqXC).Solve();
            var yRoots = new QuadraticEquation(eqA, eqYB, eqYC).Solve();
            Intersections = Tuple.Create(
                new Point(xRoots.Item1, yRoots.Item2),
                new Point(xRoots.Item2, yRoots.Item1)
            );

        }
        public static System.Tuple<Point, Point> Intersect(this Circle c1, Circle c2) {
            var context = ImmutableContext.Empty
                .Register("R1", c1.R)
                .Register("X0", Expr.Subtract(c2.X, c1.X))
                .Register("Y0", Expr.Subtract(c2.Y, c1.Y))
                .Register("R2", c2.R);
            return Intersections.Substitute(context).FMap(x => x.Offset(c1.Center));
        }
    }

    public static class QuadraticEquationHelper {
        static readonly System.Tuple<Expr, Expr> Roots;
        static QuadraticEquationHelper() {
            var d = "(B^2-4*A*C)^(1/2)";
            var x1 = string.Format("(-B+{0})/(2*A)", d).Parse();
            var x2 = string.Format("(-B-{0})/(2*A)", d).Parse();
            Roots = Tuple.Create(x1, x2);
        }
        public static System.Tuple<Expr, Expr> Solve(this QuadraticEquation eq) {
            var context = ImmutableContext.Empty
                 .Register("A", eq.A)
                 .Register("B", eq.B)
                 .Register("C", eq.C);
            return Roots.FMap(x => x.Substitute(context));
        }
    }

    public static class Functor {
        public static Point FMap(this Point x, Func<Expr, Expr> f) {
            return new Point(f(x.X), f(x.Y));
        }
        public static Line FMap(this Line x, Func<Expr, Expr> f) {
            return new Line(f(x.A), f(x.B), f(x.C));
        }
        public static Circle FMap(this Circle x, Func<Expr, Expr> f) {
            return new Circle(f(x.X), f(x.Y), f(x.R));
        }
        public static System.Tuple<T, T> FMap<T>(this System.Tuple<T, T> x, Func<T, T> f) {
            return Tuple.Create(f(x.Item1), f(x.Item2));
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
        public static ImmutableContext RegisterPoint(this ImmutableContext context, Point p, double x, double y) {
            return context
                .RegisterValue(p.X, x)
                .RegisterValue(p.Y, y);
        }
        public static ImmutableContext RegisterValue(this ImmutableContext context, Expr parameter, double value) {
            return context
                .Register(((ParameterExpr)parameter).ParameterName, Expr.Constant(NumberFactory.FromDouble(value)));
        }
        public static ImmutableContext RegisterValue(this ImmutableContext context, string nane, double value) {
            return context.RegisterValue(nane.Parse(), value);
        }
        public static ImmutableContext RegisterLine(this ImmutableContext context, Line l, string a, string b, string c) {
            return context
                .Register(a, l.A)
                .Register(b, l.B)
                .Register(c, l.C);
        }
        public static RealPoint ToRealPoint(this Point p, ImmutableContext context) {
            return new RealPoint(p.X.ToReal(context), p.Y.ToReal(context));
        }
        public static double ToReal(this Expr expr, ImmutableContext context) {
            return expr.Evaluate(context).ToDouble();
        }
        public static Expr AsConst(this double value) {
            return Expr.Constant(NumberFactory.FromDouble(value));
        }
        public static Expr Convolute(this Expr expr) {
            return expr.Visit(new ExprRewriter(new ConvolutionExprBuilder(ContextFactory.Empty)));
        }
        public static Point Offset(this Point p, Point offset) {
            return new Point(Expr.Add(p.X, offset.X), Expr.Add(p.Y, offset.Y));
        }
        public static Expr Substitute(this Expr expr, IContext context) {
            return ExprSubstitutor.Substitute(expr, context);
        }
        public static Point Substitute(this Point p, IContext context) {
            return p.FMap(x => x.Substitute(context));
        }
        public static System.Tuple<Point, Point> Substitute(this System.Tuple<Point, Point> p, IContext context) {
            return p.FMap(x => x.Substitute(context));
        }
    }
    public class ExprSubstitutor : IExpressionVisitor<Expr> {
        public static Expr Substitute(Expr expr, IContext context) {
            return expr.Visit(new ExprSubstitutor(context));
        }
        readonly IContext context;
        ExprSubstitutor(IContext context) {
            this.context = context;
        }
        Expr IExpressionVisitor<Expr>.Constant(ConstantExpr constant) {
            return constant;
        }

        Expr IExpressionVisitor<Expr>.Parameter(ParameterExpr parameter) {
            return context.GetValue(parameter.ParameterName) ?? parameter;
        }

        Expr IExpressionVisitor<Expr>.Add(AddExpr multi) {
            return Expr.Add(multi.Args.Select(x => x.Visit(this)));
        }

        Expr IExpressionVisitor<Expr>.Multiply(MultiplyExpr multi) {
            return Expr.Multiply(multi.Args.Select(x => x.Visit(this)));
        }

        Expr IExpressionVisitor<Expr>.Power(PowerExpr power) {
            return Expr.Power(power.Left.Visit(this), power.Right.Visit(this));
        }

        Expr IExpressionVisitor<Expr>.Function(FunctionExpr functionExpr) {
            throw new NotImplementedException();
        }
    }
    public class ExprRewriter : IExpressionVisitor<Expr> {
        readonly ExprBuilder builder;
        public ExprRewriter(ExprBuilder builder) {
            this.builder = builder;
        }
        Expr IExpressionVisitor<Expr>.Constant(ConstantExpr constant) {
            return constant;
        }

        Expr IExpressionVisitor<Expr>.Parameter(ParameterExpr parameter) {
            return parameter;
        }

        Expr IExpressionVisitor<Expr>.Add(AddExpr multi) {
            return multi.Args.Select(x => x.Visit(this)).Aggregate((x, y) => builder.Add(x, y));
        }

        Expr IExpressionVisitor<Expr>.Multiply(MultiplyExpr multi) {
            return multi.Args.Select(x => x.Visit(this)).Aggregate((x, y) => builder.Multiply(x, y));
        }

        Expr IExpressionVisitor<Expr>.Power(PowerExpr power) {
            return builder.Power(power.Left.Visit(this), power.Right.Visit(this));
        }

        Expr IExpressionVisitor<Expr>.Function(FunctionExpr functionExpr) {
            throw new NotImplementedException();
            //return builder.Function(functionExpr.FunctionName, functionExpr.Args);
        }
    }
}