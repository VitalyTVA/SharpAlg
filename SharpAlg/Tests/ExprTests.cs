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
            var expr = Expr.Binary(left, right, BinaryOperation.Divide);
            expr
                .IsEqual(x => x.Left, left).IsEqual(x => x.Right, right)
                .IsEqual(x => x.Operation, BinaryOperation.Divide);

            var expr2 = Expr.Binary(left, right, BinaryOperation.Divide);
            var expr3 = Expr.Binary(right, left, BinaryOperation.Divide);
            var expr4 = Expr.Binary(left, right, BinaryOperation.Add);
            expr
                .IsTrue(x => x.ExprEquals(expr2))
                .IsFalse(x => x.ExprEquals(expr3))
                .IsFalse(x => x.ExprEquals(expr4));

            Expr.Binary(Expr.Constant(9), Expr.Constant(13), BinaryOperation.Add)
                .IsEqual(x => x.Evaluate(), 22);
            Expr.Binary(Expr.Constant(9), Expr.Constant(13), BinaryOperation.Subtract)
                .IsEqual(x => x.Evaluate(), -4);
            Expr.Binary(Expr.Constant(10), Expr.Constant(5), BinaryOperation.Divide)
                .IsEqual(x => x.Evaluate(), 2);
            Expr.Binary(Expr.Constant(9), Expr.Constant(13), BinaryOperation.Multiply)
                .IsEqual(x => x.Evaluate(), 9 * 13);
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

            Expr.Binary(Expr.Parameter("x"), Expr.Parameter("y"), BinaryOperation.Add)
                .IsEqual(x => x.Evaluate(context), 22);

            context.Register("y", Expr.Binary(Expr.Parameter("x"), Expr.Parameter("x"), BinaryOperation.Multiply));
            Expr.Binary(Expr.Parameter("x"), Expr.Parameter("y"), BinaryOperation.Add)
                .IsEqual(x => x.Evaluate(context), 90);
        }
        [Test]
        public void ToStringTest() {
            "9".Parse().AssertSimpleStringRepresentation("9");
            "x".Parse().AssertSimpleStringRepresentation("x");
            "9 + x".Parse().AssertSimpleStringRepresentation("(9 + x)");
            "(9 - x)".Parse().AssertSimpleStringRepresentation("(9 - x)");
            "(9 * x)".Parse().AssertSimpleStringRepresentation("(9 * x)");
            "(9 / x)".Parse().AssertSimpleStringRepresentation("(9 / x)");
            "x + y * z".Parse().AssertSimpleStringRepresentation("(x + (y * z))");
            "(x + y) * z".Parse().AssertSimpleStringRepresentation("((x + y) * z)");
        }
        [Test]
        public void ConvolutionTest() {
            "9 + 13".Parse().AssertSimpleStringRepresentation("22");
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class ExprTestHelper {
        public static Expr AssertSimpleStringRepresentation(this Expr expr, string value) {
            return expr.IsEqual(x => x.Print(), value);
        }
    }
}