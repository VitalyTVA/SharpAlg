using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = "../res/SharpAlg.Tests.js")]
    [TestFixture]
    public class ExprTests {
        [Test]
        public void ConstExprTest() {
            var expr = Expr.Parameter("x");
            if(!expr.ParameterName.StartsWith("x"))
                throw new InvalidOperationException();
        }
        [Test]
        public void ParameterExprTest() {
            var expr = Expr.Const(9);
            if(expr.Value != 9)
                throw new InvalidOperationException();
        }
    }
}
