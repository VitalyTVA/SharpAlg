using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNumberName)]
    public class Number {
        public static readonly Number Zero;
        public static readonly Number One;
        public static readonly Number MinusOne;
        static Number() {
            Zero = FromDouble(0);
            One = FromDouble(1);
            MinusOne = FromDouble(-1);
        }
        public static Number FromDouble(double value) {
            return new Number(value);
        }
        Number(double value) {
            Value = value;
        }
        public double Value { get; private set; }
        public override bool Equals(object obj) {
            var other = obj as Number;
            return other != null && other.Value == Value;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
