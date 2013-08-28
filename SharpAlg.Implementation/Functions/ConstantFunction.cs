using SharpAlg.Native.Builder;
using SharpKit.JavaScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Implementation_Functions)]
    public abstract class ConstantFunction : Function, ISupportCheckArgs, ISupportDiff {
        public ConstantFunction(string name)
            : base(name) {
        }
        public sealed override Number Evaluate(IExpressionEvaluator evaluator, IEnumerable<Expr> args) {
            return Value;
        }

        public string Check(IEnumerable<Expr> args) {
            return string.Format("{0} is a constant and can't be used as function", Name); //TODO string resources
        }

        public Expr Diff(IDiffExpressionVisitor diffVisitor, IEnumerable<Expr> args) {
            return Expr.Zero;
        }

        protected abstract Number Value { get; }
    }
}
