using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
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
            return n1.value <= n2.value;
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
        public static Number Ln(Number n) {
            return FromDouble(Math.Log(n.value)); //TODO make external
        }

        public static readonly Number Zero;
        public static readonly Number One;
        public static readonly Number Two;
        public static readonly Number MinusOne;
        static Number() {
            Zero = FromDouble(0);
            One = FromDouble(1);
            Two = FromDouble(2);
            MinusOne = FromDouble(-1);
        }
        static Number FromDouble(double value) {
            return new Number(value);
        }
        public static Number FromString(string s) {
            return new Number(PlatformHelper.Parse(s));
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
            return PlatformHelper.ToInvariantString(value);
        }
    }
}