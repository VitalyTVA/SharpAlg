using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class DefaultExpressionVisitor<T> : IExpressionVisitor<T> {
        protected DefaultExpressionVisitor() { }
        public virtual T Constant(ConstantExpr constant) {
            return GetDefault(constant);
        }
        public virtual T Parameter(ParameterExpr parameter) {
            return GetDefault(parameter);
        }
        public virtual T Add(AddExpr multi) {
            return GetDefault(multi);
        }
        public virtual T Multiply(MultiplyExpr multi) {
            return GetDefault(multi);
        }
        public virtual T Power(PowerExpr power) {
            return GetDefault(power);
        }
        protected abstract T GetDefault(Expr expr);
    }
}