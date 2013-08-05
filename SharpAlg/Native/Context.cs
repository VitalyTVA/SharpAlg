using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class Context {
        public static Context CreateEmpty() {
            return new Context();
        }
        public static Context CreateDefault() {
            Context context = new Context();
            context.Register(Functions.Factorial);
            return context;

        }
        public static readonly Context Default;
        static Context() {
            Default = CreateDefault();
            Default.ReadOnly = true;
        }

        Dictionary<string, Expr> names = new Dictionary<string, Expr>();
        Dictionary<string, Function> functions = new Dictionary<string, Function>();

        public bool ReadOnly {
            get; private set;
        }
        Context() { }

        public void Register(Function func) {
            CheckReadonly();
            functions[func.Name] = func;
        }
        public Function GetFunction(string name) {
            Function result;
            functions.TryGetValue(name, out result); //TODO duplicated TryGetValue
            return result;
        }

        public void Register(string name, Expr value) {
            CheckReadonly();
            names[name] = value;
        }
        public Expr GetValue(string name) {
            Expr result;
            names.TryGetValue(name, out result);
            return result;
        }
        void CheckReadonly() {
            if(ReadOnly)
                throw new InvalidOperationException(); //TODO correct exception and text
        }
    }
}
