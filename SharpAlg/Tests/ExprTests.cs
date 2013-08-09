using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;
using SharpAlg.Native.Builder;
using SharpAlg.Native.Parser;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class ExprTests {
        [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
        class CustomFunction : Function {
            public CustomFunction()
                : base("CustomFunc") {
            }
            public override Number Evaluate(IExpressionVisitor<Number> evaluator, IEnumerable<Expr> args) {
                Number result = args.Select(x => x.Visit(evaluator)).Aggregate(Number.Zero, (res, x) => res + x * x);
                return result;
            }
        }
        [Test]
        public void ContextTest() {
            var context = Context.CreateEmpty();
            CustomFunction func = new CustomFunction();

            Context.CreateEmpty()
                .Register(func)
                .Register("x", "3".Parse())
                .IsEqual(x => x.GetFunction("CustomFunc"), func)
                .IsTrue(x => x.GetValue("x").ExprEquals("3".Parse()));

            Context.CreateDefault()
                .Register(func)
                .Register("x", "3".Parse())
                .IsEqual(x => x.GetFunction(Functions.Factorial.Name), Functions.Factorial)
                .IsEqual(x => x.GetFunction("CustomFunc"), func)
                .IsTrue(x => x.GetValue("x").ExprEquals("3".Parse()));
            Context.Default
                .Fails(x => x.Register(func), typeof(InvalidOperationException))
                .Fails(x => x.Register("x", "3".Parse()), typeof(InvalidOperationException));
            Context.Empty
                .Fails(x => x.Register(func), typeof(InvalidOperationException))
                .Fails(x => x.Register("x", "3".Parse()), typeof(InvalidOperationException));
        }
        [Test]
        public void ParameterExprTest() {
            Expr.Parameter("x")
                .IsEqual(x => x.ParameterName, "x")
                .Fails(x => x.Evaluate(), typeof(ExpressionEvaluationException), e => e.IsEqual(x => x.Message, "x value is undefined"))
                .IsTrue(x => x.ExprEquals(Expr.Parameter("x")))
                .IsFalse(x => x.ExprEquals(Expr.Parameter("y")));
        }
        [Test]
        public void FunctionExprTest() {
            Expr.Function("ln", ExprTestHelper.AsConstant(3))
                .IsEqual(x => x.FunctionName, "ln")
                .AssertSimpleStringRepresentation("ln(3)")
                .IsTrue(x => x.ExprEquals(Expr.Function("ln", ExprTestHelper.AsConstant(3))))
                .IsFalse(x => x.ExprEquals(Expr.Function("ln", ExprTestHelper.AsConstant(4))))
                .IsFalse(x => x.ExprEquals(Expr.Function("sin", ExprTestHelper.AsConstant(3))));

            Expr.Function("ln", Expr.Multiply(Expr.Parameter("x"), Expr.Parameter("y")))
                .AssertSimpleStringRepresentation("ln(x * y)")
                .IsTrue(x => x.ExprEquals(Expr.Function("ln", Expr.Multiply(Expr.Parameter("x"), Expr.Parameter("y")))))
                .IsFalse(x => x.ExprEquals(Expr.Function("ln", Expr.Multiply(Expr.Parameter("y"), Expr.Parameter("x")))))
                .IsTrue(x => x.ExprEquivalent(Expr.Function("ln", Expr.Multiply(Expr.Parameter("x"), Expr.Parameter("y")))))
                .IsTrue(x => x.ExprEquivalent(Expr.Function("ln", Expr.Multiply(Expr.Parameter("y"), Expr.Parameter("x")))));

            Expr.Function("someFunc", new Expr[] { Expr.Parameter("x"), Expr.Parameter("y") })
                //.AssertSimpleStringRepresentation("ln(x * y)")
                .IsTrue(x => x.ExprEquals(Expr.Function("someFunc", new Expr[] { Expr.Parameter("x"), Expr.Parameter("y") })))
                .IsFalse(x => x.ExprEquals(Expr.Function("someFunc", new Expr[] { Expr.Parameter("x"), Expr.Parameter("z") })))
                .IsTrue(x => x.ExprEquivalent(Expr.Function("someFunc", new Expr[] { Expr.Parameter("x"), Expr.Parameter("y") })))
                .IsFalse(x => x.ExprEquivalent(Expr.Function("someFunc", new Expr[] { Expr.Parameter("y"), Expr.Parameter("x") })));
        }
        [Test]
        public void ConstantExprTest() {
            ExprTestHelper.AsConstant(9)
                .IsEqual(x => x.Value, ExprTestHelper.AsNumber(9))
                .IsEqual(x => x.Evaluate(), ExprTestHelper.AsNumber(9))
                .IsTrue(x => x.ExprEquals(ExprTestHelper.AsConstant(9)))
                .IsFalse(x => x.ExprEquals(ExprTestHelper.AsConstant(13)));
        }
        [Test]
        public void BinaryExprTest() {
            var left = ExprTestHelper.AsConstant(9);
            var right = Expr.Parameter("x");
            var expr = (MultiplyExpr)Expr.Divide(left, right);
            expr
                .IsEqual(x => x.Args.ElementAt(0), left)
                .With(x => x.Args.ElementAt(1) as PowerExpr)
                    .IsEqual(x => x.Left, right)
                    .IsEqual(x => x.Right, Expr.MinusOne);

            var expr2 = Expr.Divide(left, right);
            var expr3 = Expr.Divide(right, left);
            var expr4 = Expr.Add(left, right);
            var expr5 = Expr.Power(left, right);
            expr
                .IsTrue(x => x.ExprEquals(expr2))
                .IsFalse(x => x.ExprEquals(expr3))
                .IsFalse(x => x.ExprEquals(expr4))
                .IsFalse(x => x.ExprEquals(expr5));

            Expr.Add(ExprTestHelper.AsConstant(9), ExprTestHelper.AsConstant(13))
                .IsEqual(x => x.Evaluate(), ExprTestHelper.AsNumber(22));
            Expr.Subtract(ExprTestHelper.AsConstant(9), ExprTestHelper.AsConstant(13))
                .IsEqual(x => x.Evaluate(), ExprTestHelper.AsNumber(-4));
            Expr.Divide(ExprTestHelper.AsConstant(10), ExprTestHelper.AsConstant(5))
                .IsEqual(x => x.Evaluate(), ExprTestHelper.AsNumber(2));
            Expr.Multiply(ExprTestHelper.AsConstant(9), ExprTestHelper.AsConstant(13))
                .IsEqual(x => x.Evaluate(), ExprTestHelper.AsNumber(9 * 13));
            Expr.Power(ExprTestHelper.AsConstant(2), ExprTestHelper.AsConstant(3))
                .IsEqual(x => x.Evaluate(), ExprTestHelper.AsNumber(8));
        }
        [Test]
        public void ParameterExprEvaluationTest() {
            var context = Context.CreateDefault()
                .Register("x", ExprTestHelper.AsConstant(9))
                .Register("y", ExprTestHelper.AsConstant(13));
            Expr.Parameter("x")
                .IsEqual(x => x.Evaluate(context), ExprTestHelper.AsNumber(9));
            Expr.Parameter("y")
                .IsEqual(x => x.Evaluate(context), ExprTestHelper.AsNumber(13));

            Expr.Add(Expr.Parameter("x"), Expr.Parameter("y"))
                .IsEqual(x => x.Evaluate(context), ExprTestHelper.AsNumber(22));
            "(y - x)!".Parse()
                .IsEqual(x => x.Evaluate(context), ExprTestHelper.AsNumber(24));

            context.Register("y", Expr.Multiply(Expr.Parameter("x"), Expr.Parameter("x")));
            Expr.Add(Expr.Parameter("x"), Expr.Parameter("y"))
                .IsEqual(x => x.Evaluate(context), ExprTestHelper.AsNumber(90));
        }
        [Test]
        public void FunctionEvaluationTest() {
            var context = Context.CreateEmpty()
                .Register(new CustomFunction())
                .Register("x", "3".Parse());
            "CustomFunc(1, x + 2, 2)".Parse()
                .IsEqual(x => x.Evaluate(context), 30.0.AsNumber());
            "ln(1)".Parse()
                .IsEqual(x => x.Evaluate(), 0.0.AsNumber());
            "ln(3)".Parse()
                .IsFloatEqual(x => x.Evaluate(), "1.098612");
            Expr.Function("ln", new Expr[] { "1".Parse(), "2".Parse() }).Fails(x => x.Diff(), typeof(InvalidArgumentCountException));
        }
        [Test]
        public void SemanticErrorsTest() {
            "ln(3, x)".GetParser().AssertSingleSyntaxError("Error, (in ln) expecting 1 argument, got 2\r\n");
            "factorial(3, x, 1)".GetParser().AssertSingleSyntaxError("Error, (in factorial) expecting 1 argument, got 3\r\n");
        }
        [Test]
        public void ToStringTest() {
            "9".Parse().AssertSimpleStringRepresentation("9");
            "-9".Parse().AssertSimpleStringRepresentation("-9");
            "x".Parse().AssertSimpleStringRepresentation("x");
            "-x".Parse().AssertSimpleStringRepresentation("-x");
            "9 + x".Parse().AssertSimpleStringRepresentation("9 + x");
            "(9 - x)".Parse().AssertSimpleStringRepresentation("9 - x");
            "-(9 - x)".ParseNoConvolution().AssertSimpleStringRepresentation("-(9 - x)");
            "(9 * x)".Parse().AssertSimpleStringRepresentation("9 * x");
            "(9 / x)".Parse().AssertSimpleStringRepresentation("9 / x");
            "x + y * z".Parse().AssertSimpleStringRepresentation("x + y * z");
            "(x + y) * z".Parse().AssertSimpleStringRepresentation("(x + y) * z");
            "z * (x + y)".Parse().AssertSimpleStringRepresentation("z * (x + y)");
            "x ^ y".Parse().AssertSimpleStringRepresentation("x ^ y");
            "x * z ^ y".Parse().AssertSimpleStringRepresentation("x * z ^ y");
            "x + y + z".Parse().AssertSimpleStringRepresentation("x + y + z");
            "x - y - z".Parse().AssertSimpleStringRepresentation("x - y - z");
            "x / y / z".Parse().AssertSimpleStringRepresentation("x / y / z");
            "1 + 2 * x + 3 * y".Parse().AssertSimpleStringRepresentation("1 + 2 * x + 3 * y");
            "(x + 1) ^ (x * y)".Parse().AssertSimpleStringRepresentation("(x + 1) ^ (x * y)");
            "(x - 0.05) ^ (x * .2 * y)".Parse().AssertSimpleStringRepresentation("(x - 0.05) ^ (0.2 * x * y)");

            Expr.Minus(Expr.Parameter("x")).AssertSimpleStringRepresentation("-x");
            Expr.Inverse(Expr.Parameter("x")).AssertSimpleStringRepresentation("1 / x");

            Expr.Add(ExprTestHelper.AsConstant(9), Expr.Minus(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 - x");
            Expr.Multiply(ExprTestHelper.AsConstant(9), Expr.Inverse(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 / x");

            Expr.Add(ExprTestHelper.AsConstant(9), Expr.Minus(Expr.Minus(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 - (-x)");
            Expr.Multiply(ExprTestHelper.AsConstant(9), Expr.Inverse(Expr.Inverse(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 / (1 / x)");

            Expr.Add(ExprTestHelper.AsConstant(9), Expr.Inverse(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 + 1 / x");
            Expr.Multiply(ExprTestHelper.AsConstant(9), Expr.Minus(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 * (-x)");
            Expr.Add(ExprTestHelper.AsConstant(9), Expr.Minus(Expr.Inverse(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 - 1 / x");
            Expr.Multiply(ExprTestHelper.AsConstant(9), Expr.Inverse(Expr.Minus(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 / (-x)");

            Expr.Multiply(new Expr[] { Expr.Parameter("x"), ExprTestHelper.AsConstant(-1) }).AssertSimpleStringRepresentation("x * (-1)");

            "x ^ y ^ z".Parse().AssertSimpleStringRepresentation("(x ^ y) ^ z");
            "(-2) * x".Parse().AssertSimpleStringRepresentation("-2 * x");
            Expr.Multiply(ExprTestHelper.AsConstant(-2), Expr.Add(Expr.Parameter("x"), ExprTestHelper.AsConstant(1))).AssertSimpleStringRepresentation("-2 * (x + 1)");
            "-x + y".Parse().AssertSimpleStringRepresentation("-x + y");
            "1 / (3 + x)".Parse().AssertSimpleStringRepresentation("1 / (3 + x)");
            "(2 + x) / (3 + x)".Parse().AssertSimpleStringRepresentation("(2 + x) / (3 + x)");
            "2 * x / (3 + x)".Parse().AssertSimpleStringRepresentation("2 * x / (3 + x)");
            "2 * x / (y * z)".Parse().AssertSimpleStringRepresentation("2 * x / y / z");
            "x ^ z / y ^ t".Parse().AssertSimpleStringRepresentation("x ^ z / y ^ t");
            "1 / 3 ^ x".Parse().AssertSimpleStringRepresentation("1 / 3 ^ x");
            "1 / (4 * x)".Parse().AssertSimpleStringRepresentation("0.25 / x");
            "t * (-x)".Parse().AssertSimpleStringRepresentation("-t * x");
            "t * (-2) * x".Parse().AssertSimpleStringRepresentation("-2 * t * x");
            "z + t * (-x)".Parse().AssertSimpleStringRepresentation("z - t * x");
            "z + t * (-2) * x".Parse().AssertSimpleStringRepresentation("z - 2 * t * x");
            "(- x * t) ^ z".Parse().AssertSimpleStringRepresentation("(-x * t) ^ z");

            Expr.Multiply(new Expr[] { ExprTestHelper.AsConstant(2), Expr.Parameter("x"), Expr.Power(Expr.Multiply(Expr.Parameter("y"), Expr.Parameter("z")), Expr.MinusOne) }).AssertSimpleStringRepresentation("2 * x / (y * z)");
            Expr.Power(Expr.Multiply(ExprTestHelper.AsConstant(3), Expr.Parameter("x")), Expr.MinusOne).AssertSimpleStringRepresentation("1 / (3 * x)");

            "x * ln(2)".Parse().AssertSimpleStringRepresentation("x * ln(2)");
            "ln(x + y) * ln(x * ln(x)) ^ 2".Parse().AssertSimpleStringRepresentation("ln(x + y) * ln(x * ln(x)) ^ 2");
            "x! + factorial(y)".Parse().AssertSimpleStringRepresentation("x! + y!");
            "x * y!".Parse().AssertSimpleStringRepresentation("x * y!");
            "x ^ (y + z)!".Parse().AssertSimpleStringRepresentation("x ^ (y + z)!");
            "(y ^ z)!".Parse().AssertSimpleStringRepresentation("(y ^ z)!");
            "y! ^ z!".Parse().AssertSimpleStringRepresentation("y! ^ z!");
            "someFunc(x, x + y, x ^ y)".Parse().AssertSimpleStringRepresentation("someFunc(x, x + y, x ^ y)");
        }
        [Test]
        public void ConvolutionTest() {
            "9 + 13".Parse().AssertSimpleStringRepresentation("22");
            "9 - 13".Parse().AssertSimpleStringRepresentation("-4");
            "9 * 13".Parse().AssertSimpleStringRepresentation("117");
            "117 / 9".Parse().AssertSimpleStringRepresentation("13");
            "117 / 9 - 4".Parse().AssertSimpleStringRepresentation("9");
            "(5 + 5) / 2".Parse().AssertSimpleStringRepresentation("5");

            "1 + 1 + x".Parse().AssertSimpleStringRepresentation("2 + x");
            "x + 1".Parse().AssertSimpleStringRepresentation("x + 1");
            "1 + x".Parse().AssertSimpleStringRepresentation("1 + x");
            "0 + x".Parse().AssertSimpleStringRepresentation("x");
            "x + 0".Parse().AssertSimpleStringRepresentation("x");

            "2 - 1 + x".Parse().AssertSimpleStringRepresentation("1 + x");
            "x - 1".Parse().AssertSimpleStringRepresentation("x - 1");
            "1 - x".Parse().AssertSimpleStringRepresentation("1 - x");
            "0 - x".Parse().AssertSimpleStringRepresentation("-x");
            "x - 0".Parse().AssertSimpleStringRepresentation("x");

            "2 * 2 * x".Parse().AssertSimpleStringRepresentation("4 * x");
            "x * 2".Parse().AssertSimpleStringRepresentation("2 * x");
            "2 * x".Parse().AssertSimpleStringRepresentation("2 * x");
            "0 * x".Parse().AssertSimpleStringRepresentation("0");
            "x * 0".Parse().AssertSimpleStringRepresentation("0");
            "1 * x".Parse().AssertSimpleStringRepresentation("x");
            "x * 1".Parse().AssertSimpleStringRepresentation("x");

            "4 / 2 / x".Parse().AssertSimpleStringRepresentation("2 / x");
            "x / 2".Parse().AssertSimpleStringRepresentation("0.5 * x");
            "1 / x".Parse().AssertSimpleStringRepresentation("1 / x");
            "x / 1".Parse().AssertSimpleStringRepresentation("x");
            "0 / x".Parse().AssertSimpleStringRepresentation("0");

            "x + x".Parse().AssertSimpleStringRepresentation("2 * x");
            "2 * x + 2 * x".Parse().AssertSimpleStringRepresentation("4 * x");
            "x - x".Parse().AssertSimpleStringRepresentation("0");
            "2 * x - 2 * x".Parse().AssertSimpleStringRepresentation("0");
            "x / x".Parse().AssertSimpleStringRepresentation("1");
            "1 + 2 + 3 + 4 + 5".Parse().AssertSimpleStringRepresentation("15");
            "(2 * x) / (2 * x)".Parse().AssertSimpleStringRepresentation("1");
            "x * x".Parse().AssertSimpleStringRepresentation("x ^ 2");
            "(x + 1) * (x + 1)".Parse().AssertSimpleStringRepresentation("(x + 1) ^ 2");

            "(x + 1) ^ (2 + 1)".Parse().AssertSimpleStringRepresentation("(x + 1) ^ 3");
            "(x + 1) ^ 0".Parse().AssertSimpleStringRepresentation("1");
            "(x + 1) ^ (2 - 1)".Parse().AssertSimpleStringRepresentation("x + 1");
            "(x + 1) ^ (y - y)".Parse().AssertSimpleStringRepresentation("1");
            "2 ^ 3".Parse().AssertSimpleStringRepresentation("8");
            "(x - x) ^ y".Parse().AssertSimpleStringRepresentation("0");
            "(x / x) ^ y".Parse().AssertSimpleStringRepresentation("1");

            "-9 + 13".Parse().AssertSimpleStringRepresentation("4");
            "-(x + 1)".Parse().AssertSimpleStringRepresentation("-x - 1");
            "-2 * (x + 1)".Parse().AssertSimpleStringRepresentation("-2 * x - 2");
            "(x + 1) * 2".Parse().AssertSimpleStringRepresentation("2 * x + 2");
            "z * (x + 1)".Parse().AssertSimpleStringRepresentation("z * (x + 1)");
            "-(x + 1) + (x + 1)".Parse().AssertSimpleStringRepresentation("0");
            "-(x + 1) - (x + 1)".Parse().AssertSimpleStringRepresentation("-2 * x - 2");
            "-((x + 1) / (x + 1))".Parse().AssertSimpleStringRepresentation("-1");
            
            "x + (1 - 2)".Parse().AssertSimpleStringRepresentation("x - 1");
            "x * y * x".Parse().AssertSimpleStringRepresentation("x ^ 2 * y");
            "x + y + x".Parse().AssertSimpleStringRepresentation("2 * x + y");

            "x * y * x * y * x".Parse().AssertSimpleStringRepresentation("x ^ 3 * y ^ 2");
            "x * x * x".Parse().AssertSimpleStringRepresentation("x ^ 3");
            "x + x + x".Parse().AssertSimpleStringRepresentation("3 * x");
            "2 * y + 3 * y".Parse().AssertSimpleStringRepresentation("5 * y");
            "2 * y + y".Parse().AssertSimpleStringRepresentation("3 * y");
            "y + x + y + 2 * x + y + 3 * x".Parse().AssertSimpleStringRepresentation("3 * y + 6 * x");
            "x + 1 + y - 2".Parse().AssertSimpleStringRepresentation("x - 1 + y");

            "y * x + 2 * y * x".Parse().AssertSimpleStringRepresentation("3 * y * x");
            "2 * y - 3 * y".Parse().AssertSimpleStringRepresentation("-y");
            "2 * ((x + 1) / (x + 1))".Parse().AssertSimpleStringRepresentation("2");
            "-(x + 1) / (x + 1)".Parse().AssertSimpleStringRepresentation("(-x - 1) / (x + 1)"); //TODO convolution
            "(x ^ 2) ^ 3 + x ^ 2 ^ y + x ^ y ^ 2 + x ^ y ^ z".Parse().AssertSimpleStringRepresentation("x ^ 6 + (x ^ 2) ^ y + (x ^ y) ^ 2 + (x ^ y) ^ z");

            "x * 14  + 2 * x * 2 + x - 5 * x + x * (-1)".Parse().AssertSimpleStringRepresentation("13 * x");
            "x * 14 * y  + 2 * x * y + x * y * 3 - 5 * x *y - x * (-2) * y + 2 * x * (-3) * y".Parse().AssertSimpleStringRepresentation("10 * x * y");

            "y * z * x + x * y * z + 2 * z * x * y - z * y * (-2) * x".Parse().AssertSimpleStringRepresentation("6 * y * z * x");

            "(x + 1) ^ 2 * (x + 1) ^ 3".Parse().AssertSimpleStringRepresentation("(x + 1) ^ 5");
            "(x + 1 - y) ^ 2 * (-y + 1 + x) ^ 3".Parse().AssertSimpleStringRepresentation("(x + 1 - y) ^ 5");
            "(x + y) ^ 2 * (x * y) ^ 3".Parse().AssertSimpleStringRepresentation("(x + y) ^ 2 * x ^ 3 * y ^ 3");
            "(x * y + 1) ^ 2 * (1 + y * x) ^ 3".Parse().AssertSimpleStringRepresentation("(x * y + 1) ^ 5");
            "y ^ 2 * x + 3 * x * y ^ 2".Parse().AssertSimpleStringRepresentation("4 * y ^ 2 * x");
            "x / y  / z + 3 * x / z / y".Parse().AssertSimpleStringRepresentation("4 * x / y / z");
            "x ^ (z * y) + x ^ (y * z)".Parse().AssertSimpleStringRepresentation("2 * x ^ (z * y)");
            "(t * x) ^ (z * y) + (x * t) ^ (y * z)".Parse().AssertSimpleStringRepresentation("2 * (t * x) ^ (z * y)");
            "t * 2 * y".Parse().AssertSimpleStringRepresentation("2 * t * y");
            "-x + y".Parse().AssertSimpleStringRepresentation("-x + y");
            "t * (-x)".Parse().AssertSimpleStringRepresentation("-t * x");
            "z + t * (-x)".Parse().AssertSimpleStringRepresentation("z - t * x");
            "(t * (-x)) ^ (z * y) + (x * (-t)) ^ (y * z)".Parse().AssertSimpleStringRepresentation("2 * (-t * x) ^ (z * y)");

            "(x * y) ^ 3 * (x * y) ^ 2".Parse().AssertSimpleStringRepresentation("x ^ 5 * y ^ 5");
            "(x * y) ^ 3 * (x ^ 2 * y) ^ 2".Parse().AssertSimpleStringRepresentation("x ^ 7 * y ^ 5");
            "(x * y) * (x * y) ^ 2".Parse().AssertSimpleStringRepresentation("x ^ 3 * y ^ 3");
            "(x * y) ^ 2 * (x * y) ^ 2".Parse().AssertSimpleStringRepresentation("x ^ 4 * y ^ 4");
            "(x * y) ^ z".Parse().AssertSimpleStringRepresentation("(x * y) ^ z");
            "(x * y) ^ z * (y * x) ^ t".Parse().AssertSimpleStringRepresentation("(x * y) ^ (z + t)");

            "ln(y * x) + ln(x * y)".Parse().AssertSimpleStringRepresentation("2 * ln(y * x)");
            "(y * x)! + (x * y)!".Parse().AssertSimpleStringRepresentation("2 * (y * x)!");
            "someFunc(x, y * x) + someFunc(x, x * y)".Parse().AssertSimpleStringRepresentation("2 * someFunc(x, y * x)");
            "someFunc(x, y * x)! + 2 * someFunc(x, x * y)!".Parse().AssertSimpleStringRepresentation("3 * someFunc(x, y * x)!");
            "ln(x * x) + ln(x + x)".Parse().AssertSimpleStringRepresentation("ln(x ^ 2) + ln(2 * x)");

            "ln(1)".Parse().AssertSimpleStringRepresentation("0");
        }
        [Test]
        public void SubsitutionTest() {
            var context = Context.CreateDefault()
                .Register("x", "y + 1".Parse());
            var builder = ConvolutionExprBuilder.Create(context);
            "x ^ 3".Parse(builder).AssertSimpleStringRepresentation("(y + 1) ^ 3");

            //TODO recursive substitution x => x + 1
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class ExprTestHelper {
        public static Parser GetParser(this string expression) {
            return expression.ParseCore(ConvolutionExprBuilder.CreateDefault());
        }
        public static Expr AssertSimpleStringRepresentation(this Expr expr, string value) {
            return expr.IsEqual(x => x.Print(), value);
        }
        public static Expr AssertEvaluatedValues(this Expr expr, double[] input, double[] expected) {
            var evaluator = expr.AsEvaluator();
            input.Select(x => evaluator(x)).IsSequenceEqual(expected.Select(x => AsNumber(x)));
            return expr;
        }
        public static Func<double, Number> AsEvaluator(this Expr expr) {
            return x => {
                return expr.Evaluate(Context.CreateEmpty().Register("x", ExprTestHelper.AsConstant(x)));
            };
        }
        public static ConstantExpr AsConstant(this double constant) {
            return Expr.Constant(AsNumber(constant));
        }
        public static Number AsNumber(this double constant) {
            return SharpAlg.Native.Number.FromString(PlatformHelper.ToInvariantString(constant));
        }
        public static TInput IsFloatEqual<TInput>(this TInput obj, Func<TInput, object> valueEvaluator, string expected) {
            int floatSignCount = expected.Length - expected.IndexOf(".");
            return obj.IsEqual(x => {
                var res = valueEvaluator(x).ToString();
                res = res.Substring(0, res.IndexOf(".") + floatSignCount);
                return res;
            }, expected);
        }
    }
}