using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public abstract class Function {
        protected Function(string name) {
            Name = name;
        }
        public abstract Number Evaluate(IEnumerable<Number> args);
        public string Name { get; private set; }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public abstract class SingleArgumentFunction : Function {
        protected SingleArgumentFunction(string name)
            : base(name) {
        }
        public sealed override Number Evaluate(IEnumerable<Number> args) {
            if(args.Count() != 1)
                throw new InvalidOperationException();
            return Evaluate(args.First());
        }
        protected abstract Number Evaluate(Number arg);
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class FactorialFunction : SingleArgumentFunction {
        public FactorialFunction()
            : base("factorial") {
        }
        protected override Number Evaluate(Number arg) {
            Number result = Number.One;
            for(Number i = Number.Two; i <= arg; i = i + Number.One) {
                result = result * i;
            }
            return result;
        }
    }

    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class LnFunction : SingleArgumentFunction {
        public LnFunction()
            : base("ln") {
        }
        protected override Number Evaluate(Number arg) {
            return Number.Ln(arg);
        }
    }

    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public static class Functions {
        static Function factorial;
        public static Function Factorial { get { return factorial ?? (factorial = new FactorialFunction()); } }

        static Function ln;
        public static Function Ln { get { return ln ?? (ln = new LnFunction()); } }

    }
}
