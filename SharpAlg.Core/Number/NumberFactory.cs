using SharpAlg.Native.Numbers;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
    public static class NumberFactory {
        public static readonly Number Zero;
        public static readonly Number One;
        public static readonly Number Two;
        public static readonly Number MinusOne;
        static NumberFactory() {
            Zero = FromIntString("0");
            One = FromIntString("1");
            Two = FromIntString("2");
            MinusOne = FromIntString("-1");
        }

        public static Number Ln(Number n) {
            //TODO conversion to int for ln(1);
            return FromDouble(Math.Log(n.ToFloat().ConvertCast<FloatNumber>().value));
        }
        static Number FromDouble(double value) {
            return new FloatNumber(value);
        }
        public static Number FromString(string s) {
            return FromDouble(PlatformHelper.Parse(s));
        }
        public static Number FromIntString(string s) {
            return LongIntegerNumber.FromLongIntStringCore(s);
        }

    }
}
