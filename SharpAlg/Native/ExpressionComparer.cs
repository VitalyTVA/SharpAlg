using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionComparer : IExpressionVisitor<bool> {
        readonly Expr expr;
        public ExpressionComparer(Expr expr) {
            this.expr = expr;
        }
        public bool Constant(ConstantExpr constant) {
            return DoEqualityCheck(constant, (x1, x2) => x1.Value == x2.Value);
        }
        public bool Binary(BinaryExpr binary) {
            return DoEqualityCheck(binary, (x1, x2) => x1.Left.ExprEquals(x2.Left) && x1.Right.ExprEquals(x2.Right) && x1.Operation == x2.Operation);
        }
        public bool Unary(UnaryExpr unary) {
            return DoEqualityCheck(unary, (x1, x2) => x1.Expr.ExprEquals(x2.Expr) && x1.Operation == x2.Operation);
        }
        public bool Parameter(ParameterExpr parameter) {
            return DoEqualityCheck(parameter, (x1, x2) => x1.ParameterName == x2.ParameterName);
        }
        bool DoEqualityCheck<T>(T expr2, Func<T, T, bool> equalityCheck) where T : Expr {
            var other = expr as T;
            return other != null && equalityCheck(other, expr2);
        }
    }
}
