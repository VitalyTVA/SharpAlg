using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class ExprTests {
        [Test]
        public void ParameterExprTest() {
            Expr.Parameter("x")
                .IsEqual(x => x.ParameterName, "x")
                .Fails(x => x.Evaluate(), typeof(ExpressionEvaluationException), e => e.IsEqual(x => x.Message, "x value is undefined"))
                .IsTrue(x => x.ExprEquals(Expr.Parameter("x")))
                .IsFalse(x => x.ExprEquals(Expr.Parameter("y")));
        }
        [Test]
        public void ConstantExprTest() {
            Expr.Constant(9)
                .IsEqual(x => x.Value, 9)
                .IsEqual(x => x.Evaluate(), 9)
                .IsTrue(x => x.ExprEquals(Expr.Constant(9)))
                .IsFalse(x => x.ExprEquals(Expr.Constant(13)));
        }
        [Test]
        public void BinaryExprTest() {
            var left = Expr.Constant(9);
            var right = Expr.Parameter("x");
            var expr = Expr.Divide(left, right);
            expr
                .IsEqual(x => x.Args.ElementAt(0), left)
                .IsEqual(x => x.Operation, BinaryOperation.Multiply)
                .With(x => x.Args.ElementAt(1) as UnaryExpr)
                    .IsEqual(x => x.Expr, right)
                    .IsEqual(x => x.Operation, UnaryOperation.Inverse);

            var expr2 = Expr.Divide(left, right);
            var expr3 = Expr.Divide(right, left);
            var expr4 = Expr.Add(left, right);
            var expr5 = Expr.Power(left, right);
            expr
                .IsTrue(x => x.ExprEquals(expr2))
                .IsFalse(x => x.ExprEquals(expr3))
                .IsFalse(x => x.ExprEquals(expr4))
                .IsFalse(x => x.ExprEquals(expr5));

            Expr.Add(Expr.Constant(9), Expr.Constant(13))
                .IsEqual(x => x.Evaluate(), 22);
            Expr.Subtract(Expr.Constant(9), Expr.Constant(13))
                .IsEqual(x => x.Evaluate(), -4);
            Expr.Divide(Expr.Constant(10), Expr.Constant(5))
                .IsEqual(x => x.Evaluate(), 2);
            Expr.Multiply(Expr.Constant(9), Expr.Constant(13))
                .IsEqual(x => x.Evaluate(), 9 * 13);
            Expr.Power(Expr.Constant(2), Expr.Constant(3))
                .IsEqual(x => x.Evaluate(), 8);
        }
        [Test]
        public void ParameterExprEvaluationTest() {
            var context = new Context();
            context.Register("x", Expr.Constant(9));
            context.Register("y", Expr.Constant(13));
            Expr.Parameter("x")
                .IsEqual(x => x.Evaluate(context), 9);
            Expr.Parameter("y")
                .IsEqual(x => x.Evaluate(context), 13);

            Expr.Add(Expr.Parameter("x"), Expr.Parameter("y"))
                .IsEqual(x => x.Evaluate(context), 22);

            context.Register("y", Expr.Multiply(Expr.Parameter("x"), Expr.Parameter("x")));
            Expr.Add(Expr.Parameter("x"), Expr.Parameter("y"))
                .IsEqual(x => x.Evaluate(context), 90);
        }
        [Test]
        public void ToStringTest() {
            "9".Parse().AssertSimpleStringRepresentation("9");
            "-9".Parse().AssertSimpleStringRepresentation("-9");
            "x".Parse().AssertSimpleStringRepresentation("x");
            "-x".Parse().AssertSimpleStringRepresentation("-x");
            "9 + x".Parse().AssertSimpleStringRepresentation("9 + x");
            "(9 - x)".Parse().AssertSimpleStringRepresentation("9 - x");
            "-(9 - x)".Parse().AssertSimpleStringRepresentation("-(9 - x)");
            "(9 * x)".Parse().AssertSimpleStringRepresentation("9 * x");
            "(9 / x)".Parse().AssertSimpleStringRepresentation("9 / x");
            "x + y * z".Parse().AssertSimpleStringRepresentation("x + y * z");
            "(x + y) * z".Parse().AssertSimpleStringRepresentation("(x + y) * z");
            "x ^ y".Parse().AssertSimpleStringRepresentation("x ^ y");
            "x * z ^ y".Parse().AssertSimpleStringRepresentation("x * z ^ y");
            "x + y + z".Parse().AssertSimpleStringRepresentation("x + y + z");
            "x - y - z".Parse().AssertSimpleStringRepresentation("x - y - z");
            "x / y / z".Parse().AssertSimpleStringRepresentation("x / y / z");
            "1 + 2 * x + 3 * y".Parse().AssertSimpleStringRepresentation("1 + 2 * x + 3 * y");
            "(x + 1) ^ (x * y)".Parse().AssertSimpleStringRepresentation("(x + 1) ^ (x * y)");

            Expr.Minus(Expr.Parameter("x")).AssertSimpleStringRepresentation("-x");
            Expr.Inverse(Expr.Parameter("x")).AssertSimpleStringRepresentation("1 / x");

            Expr.Add(Expr.Constant(9), Expr.Minus(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 - x");
            Expr.Multiply(Expr.Constant(9), Expr.Inverse(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 / x");

            Expr.Add(Expr.Constant(9), Expr.Minus(Expr.Minus(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 - (-x)");
            Expr.Multiply(Expr.Constant(9), Expr.Inverse(Expr.Inverse(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 / (1 / x)");

            Expr.Add(Expr.Constant(9), Expr.Inverse(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 + 1 / x");
            Expr.Multiply(Expr.Constant(9), Expr.Minus(Expr.Parameter("x"))).AssertSimpleStringRepresentation("9 * (-x)");
            Expr.Add(Expr.Constant(9), Expr.Minus(Expr.Inverse(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 - 1 / x");
            Expr.Multiply(Expr.Constant(9), Expr.Inverse(Expr.Minus(Expr.Parameter("x")))).AssertSimpleStringRepresentation("9 / (-x)");

            Expr.Multi(new Expr[] { Expr.Parameter("x"), Expr.Constant(-1) }, BinaryOperation.Multiply).AssertSimpleStringRepresentation("x * (-1)");
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
            "x * 2".Parse().AssertSimpleStringRepresentation("x * 2");
            "2 * x".Parse().AssertSimpleStringRepresentation("2 * x");
            "0 * x".Parse().AssertSimpleStringRepresentation("0");
            "x * 0".Parse().AssertSimpleStringRepresentation("0");
            "1 * x".Parse().AssertSimpleStringRepresentation("x");
            "x * 1".Parse().AssertSimpleStringRepresentation("x");

            "4 / 2 / x".Parse().AssertSimpleStringRepresentation("2 / x");
            "x / 2".Parse().AssertSimpleStringRepresentation("x / 2");
            "1 / x".Parse().AssertSimpleStringRepresentation("1 / x");
            "x / 1".Parse().AssertSimpleStringRepresentation("x");
            "0 / x".Parse().AssertSimpleStringRepresentation("0");

            "x + x".Parse().AssertSimpleStringRepresentation("2 * x");
            "2 * x +  2 * x".Parse().AssertSimpleStringRepresentation("2 * 2 * x"); //TODO convolution
            "x - x".Parse().AssertSimpleStringRepresentation("0");
            "2 * x - 2 * x".Parse().AssertSimpleStringRepresentation("0");
            "x / x".Parse().AssertSimpleStringRepresentation("1");
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
            "-(x + 1) + (x + 1)".Parse().AssertSimpleStringRepresentation("0");
            "-(x + 1) - (x + 1)".Parse().AssertSimpleStringRepresentation("(-2) * (x + 1)");
            "-((x + 1) / (x + 1))".Parse().AssertSimpleStringRepresentation("-1");
            
            "x + (1 - 2)".Parse().AssertSimpleStringRepresentation("x - 1");
            //"x + 1 - 2".Parse().AssertSimpleStringRepresentation("(x - 1)"); //TODO convolution
            //"-(x + 1) / (x + 1)".Parse().AssertSimpleStringRepresentation("-1"); //TODO convolution
            //"x + x + x".Parse().AssertSimpleStringRepresentation("(3 * x)"); //TODO convolution
            //"3 * x + 2 * x".Parse().AssertSimpleStringRepresentation("(5 * x)"); //TODO convolution
            //"x * x * x".Parse().AssertSimpleStringRepresentation("x ^ 2"); //TODO convolution

            //Expr placeholder = Expr.Placeholder;
            //Expr placeholder2 = Expr.Placeholder;
            //Rule.Binary(Expr.Zero, placeholder, BinaryOperation.Add, expr => expr);
            //Rule.Binary(placeholder, Expr.Zero, BinaryOperation.Add, expr => expr);
            //Rule.Binary(placeholder, Expr.Zero, BinaryOperation.Multiply, expr => Expr.Zero);
            //Rule.Binary(placeholder, placeholder, BinaryOperation.Add, expr => Expr.Mult(Expr.Const(2), expr));
            //Rule.Binary(Expr.Zero, Expr.Zero, BinaryOperation.Add, expr => expr);

            //Rule.Unary(e => e.Expr is ConstantExpr, op => op == UnaryOperation.Minus, unary => Expr.Const(-unary.Expr.Value));
            //Rule.Binary(e => e == Expr.Zero, e => true, op => op == BinaryOperation.Add, binary => binary.Right);
            //Rule.Binary(e => e is Const, e => e is Const, op => op == BinaryOperation.Add, binary => Expr.Constant(binary.Right.Left + binary.Right.Value));
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class ExprTestHelper {
        public static Expr AssertSimpleStringRepresentation(this Expr expr, string value) {
            return expr.IsEqual(x => x.Print(), value);
        }
        public static Expr AssertEvaluatedValues(this Expr expr, double[] input, double[] expected) {
            var evaluator = expr.AsEvaluator();
            input.Select(x => evaluator(x)).IsSequenceEqual(expected);
            return expr;
        }
        public static Func<double, double> AsEvaluator(this Expr expr) {
            return x => { 
                Context context = new Context();
                context.Register("x", Expr.Constant(x));
                return expr.Evaluate(context);
            };
        }
    }
}