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
        public virtual bool Constant(ConstantExpr constant) {
            return DoEqualityCheck(constant, (x1, x2) => x1.Value == x2.Value);
        }
        public virtual bool Multi(MultiExpr multi) {
            return DoEqualityCheck(multi, (x1, x2) => {
                return x1.Operation == x2.Operation && x1.Args.EnumerableEqual(x2.Args, (x, y) => x.ExprEquals(y));  //TODO singleton
            });
        }
        public virtual bool Power(PowerExpr power) {
            return DoEqualityCheck(power, (x1, x2) => Equals(x1.Left, x2.Left) && Equals(x1.Right, x2.Right));
        }
        public virtual bool Parameter(ParameterExpr parameter) {
            return DoEqualityCheck(parameter, (x1, x2) => x1.ParameterName == x2.ParameterName);
        }
        protected bool DoEqualityCheck<T>(T expr2, Func<T, T, bool> equalityCheck) where T : Expr {
            var other = expr as T;
            return other != null && equalityCheck(other, expr2);
        }
        bool Equals(Expr expr1, Expr expr2) {
            return expr1.Visit(Clone(expr2));
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
        public override bool Multi(MultiExpr multi) {
            if(!DoEqualityCheck(multi, (x1, x2) => x1.Operation == x2.Operation)) //TODO!!!!!
                return false;
            var list = ((MultiExpr)expr).Args.ToList();
            foreach(var item in multi.Args) {
                bool found = false;
                foreach(var item2 in list) {
                    if(item.ExprEquivalent(item2)) {
                        list.Remove(item2);
                        found = true;
                        break;
                    }
                }
                if(found == false)
                    return false;
            }
            return list.Count == 0;
        }
        protected override ExpressionEqualityComparer Clone(Expr expr) {
            return new ExpressionEquivalenceComparer(expr);
        }
    }
}
