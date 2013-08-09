using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class Context : IContext {
        public static Context CreateEmpty() {
            return new Context();
        }
        public static Context CreateDefault() {
            return new Context()
                .Register(Functions.Factorial)
                .Register(Functions.Factorial)
                .Register(Functions.Ln)
                .Register(Functions.Diff);
        }
        public static readonly Context Empty;
        public static readonly Context Default;
        static Context() {
            Default = CreateDefault();
            Default.ReadOnly = true;

            Empty = CreateEmpty();
            Empty.ReadOnly = true;
        }

        Dictionary<string, Expr> names = new Dictionary<string, Expr>();
        Dictionary<string, Function> functions = new Dictionary<string, Function>();

        public bool ReadOnly {
            get; private set;
        }
        Context() { }

        public Context Register(Function func) {
            CheckReadonly();
            functions[func.Name] = func;
            return this;
        }
        public Function GetFunction(string name) {
            return functions.TryGetValue(name); ;
        }

        public Context Register(string name, Expr value) {
            CheckReadonly();
            names[name] = value;
            return this;
        }
        public Expr GetValue(string name) {
            return names.TryGetValue(name);
        }
        void CheckReadonly() {
            if(ReadOnly)
                throw new InvalidOperationException(); //TODO correct exception and text
        }
    }
}
