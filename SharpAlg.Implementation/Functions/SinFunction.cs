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
    public class SinFunction : SingleArgumentDifferentiableFunction {
        public SinFunction()
            : base(FunctionFactory.SinName) {
        }

        protected override Expr DiffCore(ExprBuilder builder, Expr arg) {
            throw new NotImplementedException();
        }

        protected override Number Evaluate(Number arg) {
            return NumberFactory.GetFloat(arg, x => Math.Sin(x));
        }
    }
}
