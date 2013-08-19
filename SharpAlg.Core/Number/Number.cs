using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

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
        public static int Compare(Number n1, Number n2) {
            ToSameType(ref n1, ref n2);
            return n1.Compare(n2);
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
        public static Number FromLongIntString(string s) {
            return LongIntegerNumber.FromLongIntStringCore(s);
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
        protected abstract int Compare(Number n);

        bool Less(Number n) {
            return Compare(n) < 0;
        }
        bool Greater(Number n) {
            return Compare(n) > 0;
        }
        public sealed override bool Equals(object obj) {
            var other = obj as Number;
            return Compare(other) == 0;
        }
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
        protected override int Compare(Number n) {
            return BinaryOperation<int>(n, (x, y) => Math.Sign(x - y));
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
        protected override int Compare(Number n) {
            return BinaryOperation<int>(n, (x, y) => (int)(x - y));
        }
        T BinaryOperation<T>(Number n, Func<long, long, T> operation) {
            return operation(value, n.ConvertCast<IntegerNumber>().value);
        }
        Number BinaryOperation(Number n, Func<long, long, long> operation) {
            return FromLong(BinaryOperation<long>(n, operation));
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
    public sealed class LongIntegerNumber : Number {
        static readonly LongIntegerNumber ZeroLongNumber = new LongIntegerNumber(new int[0], false);
        internal static Number FromLongIntStringCore(string s) {
            IList<int> digits = new List<int>(); //TODO - set capacity
            int currentPart = 0;
            int currentIndex = 0;
            int currentPower = 1;
            int ZeroCode = PlatformHelper.CharToInt('0');
            int lastIndex = s[0] == '-' ? 1 : 0;
            for(int i = s.Length - 1; i >= lastIndex; i--) {
                currentPart += (PlatformHelper.CharToInt(s[i]) - ZeroCode) * currentPower;
                currentIndex++;
                currentPower *= LongIntegerNumber.Base;
                if(currentIndex == LongIntegerNumber.BaseCount) {
                    currentIndex = 0;
                    digits.Add(currentPart);
                    currentPart = 0;
                    currentPower = 1;
                }
            }
            if(currentIndex > 0 && currentPart > 0) {
                digits.Add(currentPart);
            }
            return new LongIntegerNumber(digits, lastIndex == 1);
        }

        internal const int Base = 10;
        internal const int BaseCount = 4;
        internal const int BaseFull = 10000;
        readonly bool isNegative;
        readonly IList<int> parts;
        LongIntegerNumber(IList<int> parts, bool isNegative) {
            this.isNegative = isNegative;
            this.parts = parts;
        }
        protected override int NumberType { get { return IntegerNumberType; } }
        protected override Number ConvertToCore(int type) {
            if(type == Number.FloatNumberType) {
                double result = 0;
                int count = parts.Count;
                for(int i = count - 1; i >= 0; i--) {
                    result = result * BaseFull + parts[i];
                }
                return FromDouble(result);
            }
            throw new NotImplementedException();
        }
        protected override int Compare(Number n) {
            var other = (LongIntegerNumber)n;
            return Compare(this, other);
        }
        static int Compare(LongIntegerNumber n1, LongIntegerNumber n2) {
            if(!n1.parts.Any() && !n2.parts.Any())
                return 0;
            if(!n2.parts.Any())
                return n1.isNegative ? -1 : 1;
            if(!n1.parts.Any())
                return n2.isNegative ? 1 : -1;

            if(n1.isNegative && !n2.isNegative)
                return -1;
            if(!n1.isNegative && n2.isNegative)
                return 1;
            int partsComparisonResult = CompareCore(n1.parts, n2.parts);
            return n1.isNegative ? -partsComparisonResult : partsComparisonResult;
        }
        static int CompareCore(IList<int> parts1, IList<int> parts2) {
            int lenDifference = parts1.Count - parts2.Count;
            if(lenDifference != 0)
                return lenDifference;
            int count = parts1.Count;
            for(int i = count - 1; i >= 0; i--) {
                int difference = parts1[i] - parts2[i];
                if(difference != 0)
                    return difference;
            }
            return 0;
        }
        public override int GetHashCode() {
            throw new NotImplementedException();
        }
        public override string ToString() {
            int startIndex = parts.Count - 1;
            StringBuilder sb = new StringBuilder();
            if(parts.Count == 0)
                return "0";
            if(isNegative)
                sb.Append("-");
            for(int i = startIndex; i >= 0; i--) {
                string stringValue = parts[i].ToString();
                if(i != startIndex) {
                    for(int j = BaseCount - stringValue.Length; j > 0; j--) {
                        sb.Append('0');
                    }
                }
                sb.Append(stringValue);
            }
            return sb.ToString();
        }
        protected override Number Add(Number n) {
            var longNumber = n.ConvertCast<LongIntegerNumber>();
            if(!isNegative && !longNumber.isNegative)
                return AddCore(longNumber);
            if(longNumber.isNegative) {
                var invertedRight = new LongIntegerNumber(longNumber.parts, false);
                if(this < invertedRight)
                    return new LongIntegerNumber(invertedRight.Subtract(this).ConvertCast<LongIntegerNumber>().parts, true);
            } else {
                var invertedLeft = new LongIntegerNumber(this.parts, false);
                if(longNumber < invertedLeft)
                    return new LongIntegerNumber(invertedLeft.Subtract(longNumber).ConvertCast<LongIntegerNumber>().parts, true);
            }
            return AddCore(longNumber);
        }
        protected override Number Subtract(Number n) {
            var longNumber = n.ConvertCast<LongIntegerNumber>();
            return Add(new LongIntegerNumber(longNumber.parts, !longNumber.isNegative));
        }
        Number AddCore(LongIntegerNumber longNumber) {
            return new LongIntegerNumber(AddImpl(this.parts, longNumber.parts, isNegative, longNumber.isNegative), false);
        }
        static IList<int> AddImpl(IList<int> left, IList<int> right, bool isLeftNegative, bool isRightNegative) {
            int count = Math.Max(left.Count, right.Count);
            List<int> result = new List<int>();
            int carry = 0;
            for(int i = 0; i < count; i++) {
                int resultPart = GetPart(left, i, isLeftNegative) + GetPart(right, i, isRightNegative) + carry;//TODO optimization - stop when one is empty
                if(resultPart >= BaseFull) {
                    resultPart = resultPart - BaseFull;
                    carry = 1;
                } else if(resultPart < 0) {
                    resultPart = resultPart + BaseFull;
                    carry = -1;
                } else {
                    carry = 0;
                }
                result.Add(resultPart);
            }
            if(carry > 0)
                result.Add(carry);
            for(int i = result.Count - 1; i >= 0; i--) {
                if(result[i] == 0)
                    result.RemoveAt(i);
                else
                    break;
            }
            return result;
        }
        protected override Number Multiply(Number n) {
            var longNumber = n.ConvertCast<LongIntegerNumber>();
            return new LongIntegerNumber(MultiplyCore(this.parts, longNumber.parts), isNegative ^ longNumber.isNegative);
        }

        static IList<int> MultiplyCore(IList<int> left, IList<int> right) {
            if(right.Count == 0)
                return ZeroLongNumber.parts;
            IList<int> result = new int[0];
            int rightCount = right.Count;
            for(int i = 0; i < rightCount; i++) {
                result = AddImpl(result, MultiplyOneDigit(left, right[i], i), false, false);
            }
            return result;
        }
        static IList<int> MultiplyOneDigit(IList<int> left, int digit, int shift) {
            List<int> result = new List<int>();
            int carry = 0;
            int leftCount = left.Count;
            AddTrailingZeros(result, shift);
            for(int leftIndex = 0; leftIndex < leftCount; leftIndex++) {
                int resultPart = left[leftIndex] * digit + carry;
                if(resultPart >= BaseFull) {
                    int remain = resultPart % BaseFull;
                    carry = (resultPart - remain) / BaseFull; //TODO move to platfrom helper
                    resultPart = remain;
                } else {
                    carry = 0;
                }
                result.Add(resultPart);
            }
            if(carry > 0)
                result.Add(carry);
            return result;
        }
        static void AddTrailingZeros(IList<int> result, int shift) {
            for(int i = 0; i < shift; i++) {
                result.Add(0);
            }
        }
        static void ShiftLeft(IList<int> result) {
            result.Insert(0, 0);
        }
        static void ShiftRight(IList<int> result) {
            result.RemoveAt(0);
        }
        protected override Number Divide(Number n) {
            var longNumber = n.ConvertCast<LongIntegerNumber>();
            if(this < n)
                return ZeroLongNumber;
            return new LongIntegerNumber(DivieCore(this, longNumber.parts), false);
        }
        static IList<int> DivieCore(LongIntegerNumber divident, IList<int> originalDivisor) {
            IList<int> divisor = new List<int>(originalDivisor); //TODO test it
            int shiftCount = 0;
            while(CompareCore(divident.parts, divisor) >= 0) {
                ShiftLeft(divisor);
                shiftCount++;
            }
            ShiftRight(divisor);
            shiftCount--;
            List<int> result = new List<int>();
            while(divisor.Count >= originalDivisor.Count) {
                int digit = FindDigit(divident, divisor);
                result.Insert(0, digit);
                Number temp = (new LongIntegerNumber(divisor, false)).Multiply(new LongIntegerNumber(new int[] { digit }, false));
                divident = (LongIntegerNumber)divident.Subtract(temp);
                if(CompareCore(divident.parts, divisor) < 0)
                    ShiftRight(divisor);
            }
            return result;
        }
        static int Div(int x, int y) {
            int remain = x % y;
            return (x - remain) / y; //TODO
        }
        static int Bisect(int lowDigit, int topDigit) {
            return lowDigit + Div(topDigit - lowDigit, 2);
        }
        static int FindDigit(LongIntegerNumber divident, IList<int> divisorParts) {
            LongIntegerNumber divisor = new LongIntegerNumber(divisorParts, false);
            if(divident == ZeroLongNumber || divisor > divident)
                return 0;
            int dividentPart = ((divident.parts.Count == divisor.parts.Count) ? divident.parts.Last() : divident.parts.Last() * BaseFull + divident.parts[divident.parts.Count - 2]);
            int divisorPart = divisor.parts.Last();
            int lowDigit = Div(dividentPart, divisorPart + 1);
#if DEBUG 
            int checkCount = 0;
#endif
            int topDigit = Div(dividentPart + 1, divisorPart);
            int digit = Bisect(lowDigit, topDigit);
            while(true) {
                Number mult = divisor.Multiply(new LongIntegerNumber(new int[] { digit }, false));
                LongIntegerNumber diff = (LongIntegerNumber)divident.Subtract(mult);
                if(diff.isNegative) {
#if DEBUG
                    checkCount++;
#endif
                    topDigit = digit;
                    digit = Bisect(lowDigit, topDigit);
                    continue;
                }
                int comparisonResult = diff.Compare(divisor);
                if(comparisonResult >= 0) {
#if DEBUG
                    checkCount++;
#endif
                    lowDigit = digit + 1;
                    digit = Bisect(lowDigit, topDigit);
                    continue;
                }
                break;
            }
#if DEBUG
            if(checkCount > 15)
                throw new InvalidOperationException();
#endif

            return digit;
        }
        protected override Number Power(Number n) {
            throw new NotImplementedException(); //TODO real power without long conversion
        }
        int GetPart(int index) {
            return GetPart(parts, index, isNegative);
        }
        static int GetPart(IList<int> parts, int index, bool isNegative) {
            return index < parts.Count ? (isNegative ? -parts[index] : parts[index]) : 0;
        }
    }
}