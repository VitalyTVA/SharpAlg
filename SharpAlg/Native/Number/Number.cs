using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNumberName)]
    public class Number {
        public static bool operator ==(Number n1, Number n2) {
            return object.Equals(n1, n2);
        }
        public static bool operator !=(Number n1, Number n2) {
            return !object.Equals(n1, n2);
        }
        public static bool operator >=(Number n1, Number n2) {
            return n1.value >= n2.value;
        }
        public static bool operator <=(Number n1, Number n2) {
            throw new NotImplementedException();
        }
        public static bool operator <(Number n1, Number n2) {
            return n1.value < n2.value;
        }
        public static bool operator >(Number n1, Number n2) {
            return n1.value > n2.value;
        }
        public static Number operator *(Number n1, Number n2) {
            return FromDouble(n1.value * n2.value);
        }
        public static Number operator +(Number n1, Number n2) {
            return FromDouble(n1.value + n2.value);
        }
        public static Number operator -(Number n1, Number n2) {
            return FromDouble(n1.value - n2.value);
        }
        public static Number operator ^(Number n1, Number n2) {
            return FromDouble(Math.Pow(n1.value, n2.value));
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
        static Number FromDouble(double value) {
            return new Number(value);
        }
        public static Number FromString(string s) {
            return new Number(Parse(s));
        }

        readonly double value;
        Number(double value) {
            this.value = value;
        }

        public override bool Equals(object obj) {
            var other = obj as Number;
            return other != null && other.value == value;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return ToString(value);
        }
        [JsMethod(Code = "return d.toString();")]
        public static string ToString(double d) { //TODO compatibility layer
            return d.ToString(CultureInfo.InvariantCulture);
        }
        [JsMethod(Code = "return System.Double.Parse$$String(s);")]
        static double Parse(string s) { //TODO compatibility layer
            return double.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}
