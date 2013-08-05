using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public static class PlatformHelper {
        [JsMethod(Code = "return d.toString();")]
        public static string ToString(double d) {
            return d.ToString(CultureInfo.InvariantCulture);
        }
        [JsMethod(Code = "return System.Double.Parse$$String(s);")]
        public static double Parse(string s) {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }
        [JsMethod(Code = "return String.fromCharCode(n);")]
        public static char IntToChar(int n) {
            return (char)n;
        }
        [JsMethod(Code = "return c.charCodeAt();")]
        public static int CharToInt(char c) {
            return c;
        }
    }
}
