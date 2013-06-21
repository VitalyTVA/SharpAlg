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
        public void ConstantExprTest() {
            var expr = Expr.Parameter("x");
            expr.IsEqual(x => x.ParameterName, "x");
            expr
                .IsTrue(x => x.ExprEquals(Expr.Parameter("x")))
                .IsFalse(x => x.ExprEquals(Expr.Parameter("y")));
        }
        [Test]
        public void ParameterExprTest() {
            var expr = Expr.Constant(9);
            expr.IsEqual(x => x.Value, 9);
            expr
                .IsTrue(x => x.ExprEquals(Expr.Constant(9)))
                .IsFalse(x => x.ExprEquals(Expr.Constant(13)));
        }
        [Test]
        public void BinaryExprTest() {
            var left = Expr.Constant(9);
            var right = Expr.Parameter("x");
            var expr = Expr.Binary(left, right, BinaryOperation.Divide);
            expr.IsEqual(x => x.Left, left).IsEqual(x => x.Right, right).IsEqual(x => x.Operation, BinaryOperation.Divide);
        }
    }
}