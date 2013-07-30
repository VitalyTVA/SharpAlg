using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionEqualityComparer : IExpressionVisitor<bool> {
        protected readonly Expr expr;
        public ExpressionEqualityComparer(Expr expr) {
            this.expr = expr;
        }
        public bool Constant(ConstantExpr constant) {
            return DoEqualityCheck(constant, (x1, x2) => x1.Value.Equals(x2.Value));
        }
        public bool Multi(MultiExpr multi) {
            return DoEqualityCheck(multi, (x1, x2) => {
                return x1.Operation == x2.Operation && GetArgsEqualComparer()(x1.Args, x2.Args);  //TODO singleton
            });
        }
        public bool Power(PowerExpr power) {
            return DoEqualityCheck(power, (x1, x2) => EqualsCore(x1.Left, x2.Left) && EqualsCore(x1.Right, x2.Right));
        }
        public bool Parameter(ParameterExpr parameter) {
            return DoEqualityCheck(parameter, (x1, x2) => x1.ParameterName == x2.ParameterName);
        }
        protected bool DoEqualityCheck<T>(T expr2, Func<T, T, bool> equalityCheck) where T : Expr {
            var other = expr as T;
            return other != null && equalityCheck(other, expr2);
        }
        protected bool EqualsCore(Expr expr1, Expr expr2) {
            return expr1.Visit(Clone(expr2));
        }
        protected virtual Func<IEnumerable<Expr>, IEnumerable<Expr>, bool> GetArgsEqualComparer() {
            return (x, y) => x.EnumerableEqual(y, EqualsCore);
        }
        protected virtual ExpressionEqualityComparer Clone(Expr expr) {
            return new ExpressionEqualityComparer(expr);
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionEquivalenceComparer : ExpressionEqualityComparer {
        public ExpressionEquivalenceComparer(Expr expr)
            : base(expr) {
        }
        protected override Func<IEnumerable<Expr>, IEnumerable<Expr>, bool> GetArgsEqualComparer() {
            return (x, y) => x.SetEqual(y, EqualsCore);
        }
        protected override ExpressionEqualityComparer Clone(Expr expr) {
            return new ExpressionEquivalenceComparer(expr);
        }
    }
}
