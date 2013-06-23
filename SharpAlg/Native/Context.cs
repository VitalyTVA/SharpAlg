using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class Context {
        Dictionary<string, Expr> names = new Dictionary<string, Expr>();
        public void Register(string name, Expr value) {
            names[name] = value;
        }
        public Expr GetValue(string name) {
            Expr result;
            names.TryGetValue(name, out result);
            return result;
        }
    }
}
