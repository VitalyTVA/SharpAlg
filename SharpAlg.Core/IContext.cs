using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Core)]
    public interface IContext {
        Function GetFunction(string name);
        Expr GetValue(string name);
    }
}
