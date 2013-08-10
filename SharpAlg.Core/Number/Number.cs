using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
    public abstract class Number {
        protected const int IntegerNumberType = 0;
        protected const int FloatNumberType = 1;
        static void ToSameType(ref Number n1, ref Number n2) {
            var type = Math.Max(n1.NumberType, n2.NumberType);
            n1 = n1.ConvertTo(type);
            n2 = n2.ConvertTo(type);
        }
        public static bool operator ==(Number n1, Number n2) {
            if((object)n1 != null && (object)n2 != null)
                ToSameType(ref n1, ref n2);
            return object.Equals(n1, n2);
        }
        public static bool operator !=(Number n1, Number n2) {
            return !(n1 == n2);
        }
        public static bool operator >=(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return !n1.Less(n2);
        }
        public static bool operator <=(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return !n1.Greater(n2);
        }
        public static bool operator <(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Less(n2);
        }
        public static bool operator >(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Greater(n2);
        }
        public static Number operator *(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Multiply(n2);
        }
        public static Number operator /(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Divide(n2);
        }
        public static Number operator +(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Add(n2);
        }
        public static Number operator -(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Subtract(n2);
        }
        public static Number operator ^(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Power(n2);
        }
        public static Number Ln(Number n) {
            //TODO ln for integer number
            //TODO conversion to int for ln(1);
            return FromDouble(Math.Log(n.ConvertCast<FloatNumber>().value)); //TODO make external
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
        protected static Number FromDouble(double value) {
            return new FloatNumber(value);
        }
        protected static Number FromLong(long value) {
            return new IntegerNumber(value);
        }
        public static Number FromString(string s) {
            return FromDouble(PlatformHelper.Parse(s));
        }
        public static Number FromIntString(string s) {
            return FromLong(long.Parse(s));
        }

        protected Number() {
        }
        protected Number ConvertTo(int type) {
            if(type < NumberType)
                throw new NotImplementedException();
            if(type == NumberType)
                return this;
            return ConvertToCore(type);
        }

        protected abstract Number ConvertToCore(int type);
        protected abstract int NumberType { get; }
        protected abstract Number Add(Number n);
        protected abstract Number Subtract(Number n);
        protected abstract Number Multiply(Number n);
        protected abstract Number Divide(Number n);
        protected abstract Number Power(Number n);
        protected abstract bool Less(Number n);
        protected abstract bool Greater(Number n);
    }
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
    public sealed class FloatNumber : Number {
        internal readonly double value;
        public FloatNumber(double value) {
            this.value = value;
        }
        protected override int NumberType { get { return FloatNumberType; } }
        protected override Number ConvertToCore(int type) {
            throw new NotImplementedException();
        }
        public override bool Equals(object obj) {
            var other = obj as FloatNumber;
            return other != null && other.value == value;
        }
        public override int GetHashCode() {
            return value.GetHashCode();
        }
        public override string ToString() {
            return PlatformHelper.ToInvariantString(value);
        }
        protected override Number Add(Number n) {
            return BinaryOperation(n, (x, y) => x + y);
        }
        protected override Number Subtract(Number n) {
            return BinaryOperation(n, (x, y) => x - y);
        }
        protected override Number Multiply(Number n) {
            return BinaryOperation(n, (x, y) => x * y);
        }
        protected override Number Divide(Number n) {
            return BinaryOperation(n, (x, y) => x / y);
        }
        protected override Number Power(Number n) {
            return BinaryOperation(n, (x, y) => Math.Pow(x, y));
        }
        protected override bool Less(Number n) {
            return BinaryOperation(n, (x, y) => x < y);
        }
        protected override bool Greater(Number n) {
            return BinaryOperation(n, (x, y) => x > y);
        }
        T BinaryOperation<T>(Number n, Func<double, double, T> operation) {
            return operation(value, n.ConvertCast<FloatNumber>().value);
        }
        Number BinaryOperation(Number n, Func<double, double, double> operation) {
            return FromDouble(BinaryOperation<double>(n, operation));
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
    public sealed class IntegerNumber : Number {
        internal readonly long value;
        public IntegerNumber(long value) {
            this.value = value;
        }
        protected override int NumberType { get { return IntegerNumberType; } }
        protected override Number ConvertToCore(int type) {
            return FromDouble(value);
        }
        public override bool Equals(object obj) {
            var other = obj as IntegerNumber;
            return other != null && other.value == value;
        }
        public override int GetHashCode() {
            return value.GetHashCode();
        }
        public override string ToString() {
            return value.ToString();
        }
        protected override Number Add(Number n) {
            return BinaryOperation(n, (x, y) => x + y);
        }
        protected override Number Subtract(Number n) {
            return BinaryOperation(n, (x, y) => x - y);
        }
        protected override Number Multiply(Number n) {
            return BinaryOperation(n, (x, y) => x * y);
        }
        protected override Number Divide(Number n) {
            return BinaryOperation(n, (x, y) => x / y);
        }
        protected override Number Power(Number n) {
            return BinaryOperation(n, (x, y) => (long)Math.Pow(x, y)); //TODO real power without long conversion
        }
        protected override bool Less(Number n) {
            return BinaryOperation(n, (x, y) => x < y);
        }
        protected override bool Greater(Number n) {
            return BinaryOperation(n, (x, y) => x > y);
        }
        T BinaryOperation<T>(Number n, Func<long, long, T> operation) {
            return operation(value, n.ConvertCast<IntegerNumber>().value);
        }
        Number BinaryOperation(Number n, Func<long, long, long> operation) {
            return FromLong(BinaryOperation<long>(n, operation));
        }
    }
}