using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNumberName)]
    public class Number {
        public static bool operator >=(Number n1, Number n2) {
            return n1.Value >= n2.Value;
        }
        public static Number operator *(Number n1, Number n2) {
            return FromDouble(n1.Value * n2.Value);
        }
        public static Number operator +(Number n1, Number n2) {
            return FromDouble(n1.Value + n2.Value);
        }
        public static Number operator ^(Number n1, Number n2) {
            return FromDouble(Math.Pow(n1.Value, n2.Value));
        }
        public static bool operator <=(Number n1, Number n2) {
            throw new NotImplementedException();
        }
        //public static Number operator -(Number n) {
        //    return FromDouble(-n.Value);
        //}
        //public static implicit operator Number(double value) {
        //    return FromDouble(value);
        //}

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
        public override string ToString() {
            return ToString(Value);
        }
        [JsMethod(Code = "return d.toString();")]
        static string ToString(double d) { //TODO compatibility layer
            return d.ToString(CultureInfo.InvariantCulture);
        }

    }
}
