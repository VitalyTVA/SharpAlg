/*Generated by SharpKit 5 v5.01.1000*/
if (typeof($CreateException)=='undefined') 
{
    var $CreateException = function(ex, error) 
    {
        if(error==null)
            error = new Error();
        if(ex==null)
            ex = new System.Exception.ctor();       
        error.message = ex.message;
        for (var p in ex)
           error[p] = ex[p];
        return error;
    }
}
if (typeof ($CreateAnonymousDelegate) == 'undefined') {
    var $CreateAnonymousDelegate = function (target, func) {
        if (target == null || func == null)
            return func;
        var delegate = function () {
            return func.apply(target, arguments);
        };
        delegate.func = func;
        delegate.target = target;
        delegate.isDelegate = true;
        return delegate;
    }
}
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Native$Numbers$FloatNumber =
{
    fullname: "SharpAlg.Native.Numbers.FloatNumber",
    baseTypeName: "SharpAlg.Native.Number",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function (value)
        {
            this.value = 0;
            SharpAlg.Native.Number.ctor.call(this);
            this.value = value;
        },
        NumberType$$: "System.Int32",
        get_NumberType: function ()
        {
            return 2;
        },
        ConvertToCore: function (type)
        {
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
        },
        toString: function ()
        {
            return SharpAlg.Native.PlatformHelper.ToInvariantString(this.value);
        },
        Add: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$Double$Double$Double(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return x + y;
            }));
        },
        Subtract: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$Double$Double$Double(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return x - y;
            }));
        },
        Multiply: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$Double$Double$Double(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return x * y;
            }));
        },
        Divide: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$Double$Double$Double(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return x / y;
            }));
        },
        Power: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$Double$Double$Double(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return System.Math.Pow(x, y);
            }));
        },
        Compare$$Number: function (n)
        {
            return this.BinaryOperation$1$$Number$$Func$3(System.Int32.ctor, n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return System.Math.Sign$$Double(x - y);
            }));
        },
        BinaryOperation$1$$Number$$Func$3: function (T, n, operation)
        {
            return operation(this.value, SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.FloatNumber.ctor, n).value);
        },
        BinaryOperation$$Number$$Func$3$Double$Double$Double: function (n, operation)
        {
            return new SharpAlg.Native.Numbers.FloatNumber.ctor(this.BinaryOperation$1$$Number$$Func$3(System.Double.ctor, n, operation));
        }
    }
};
JsTypes.push(SharpAlg$Native$Numbers$FloatNumber);
var SharpAlg$Native$Numbers$FractionNumber =
{
    fullname: "SharpAlg.Native.Numbers.FractionNumber",
    baseTypeName: "SharpAlg.Native.Number",
    staticDefinition:
    {
        cctor: function ()
        {
        },
        GCD: function (a, b)
        {
            var c;
            while (SharpAlg.Native.Number.op_GreaterThan(b, SharpAlg.Native.NumberFactory.Zero))
            {
                c = a.Modulo(b);
                a = b;
                b = c;
            }
            return a;
        },
        Create: function (numerator, denominator)
        {
            var gcd = SharpAlg.Native.Numbers.FractionNumber.GCD(numerator, denominator);
            var numerator_ = numerator.IntDivide(gcd);
            var denominator_ = denominator.IntDivide(gcd);
            return SharpAlg.Native.Number.op_Equality(denominator_, SharpAlg.Native.NumberFactory.One) ? numerator_ : new SharpAlg.Native.Numbers.FractionNumber.ctor(numerator_, denominator_);
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function (numerator, denominator)
        {
            this.denominator = null;
            this.numerator = null;
            SharpAlg.Native.Number.ctor.call(this);
            if (SharpAlg.Native.Number.op_LessThan(denominator, SharpAlg.Native.NumberFactory.Zero))
                throw $CreateException(new System.ArgumentException.ctor$$String("denominator"), new Error());
            this.denominator = denominator;
            this.numerator = numerator;
        },
        toString: function ()
        {
            return this.numerator.toString() + "/" + this.denominator.toString();
        },
        ConvertToCore: function (type)
        {
            return SharpAlg.Native.Number.op_Division(this.numerator.ToFloat(), this.denominator.ToFloat());
        },
        NumberType$$: "System.Int32",
        get_NumberType: function ()
        {
            return 1;
        },
        Add: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$FractionNumber$FractionNumber$Number(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                var numerator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Addition(SharpAlg.Native.Number.op_Multiply(x.numerator, y.denominator), SharpAlg.Native.Number.op_Multiply(x.denominator, y.numerator))));
                var denominator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Multiply(x.denominator, y.denominator)));
                return SharpAlg.Native.Numbers.FractionNumber.Create(numerator, denominator);
            }));
        },
        Subtract: function (n)
        {
            var other = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.FractionNumber.ctor, n);
            return SharpAlg.Native.Number.op_Addition(this, new SharpAlg.Native.Numbers.FractionNumber.ctor(SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Subtraction(SharpAlg.Native.NumberFactory.Zero, other.numerator))), SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, other.denominator)));
        },
        Multiply: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$FractionNumber$FractionNumber$Number(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                var numerator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Multiply(x.numerator, y.numerator)));
                var denominator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Multiply(x.denominator, y.denominator)));
                return SharpAlg.Native.Numbers.FractionNumber.Create(numerator, denominator);
            }));
        },
        Divide: function (n)
        {
            return this.BinaryOperation$$Number$$Func$3$FractionNumber$FractionNumber$Number(n, $CreateAnonymousDelegate(this, function (x, y)
            {
                var numerator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Multiply(x.numerator, y.denominator)));
                var denominator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Multiply(x.denominator, y.numerator)));
                if (SharpAlg.Native.Number.op_LessThan(denominator, SharpAlg.Native.NumberFactory.Zero))
                {
                    denominator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Subtraction(SharpAlg.Native.NumberFactory.Zero, denominator)));
                    numerator = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, (SharpAlg.Native.Number.op_Subtraction(SharpAlg.Native.NumberFactory.Zero, numerator)));
                }
                return SharpAlg.Native.Numbers.FractionNumber.Create(numerator, denominator);
            }));
        },
        Power: function (n)
        {
            if (SharpAlg.Native.Number.op_Equality(this.numerator, SharpAlg.Native.Numbers.LongIntegerNumber.One) && SharpAlg.Native.Number.op_Equality(this.denominator, SharpAlg.Native.Numbers.LongIntegerNumber.One))
                return SharpAlg.Native.NumberFactory.One;
            var other = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.FractionNumber.ctor, n);
            if (SharpAlg.Native.Number.op_Equality(other.denominator, SharpAlg.Native.Numbers.LongIntegerNumber.One))
                return SharpAlg.Native.Numbers.LongIntegerNumber.FastPower(this, other.numerator);
            return SharpAlg.Native.Number.op_ExclusiveOr(this.ToFloat(), n.ToFloat());
        },
        Compare$$Number: function (n)
        {
            return this.BinaryOperation$1$$Number$$Func$3(System.Int32.ctor, n, $CreateAnonymousDelegate(this, function (x, y)
            {
                return SharpAlg.Native.Number.Compare$$Number$$Number(SharpAlg.Native.Number.op_Multiply(x.numerator, y.denominator), SharpAlg.Native.Number.op_Multiply(x.denominator, y.numerator));
            }));
        },
        BinaryOperation$1$$Number$$Func$3: function (T, n, operation)
        {
            return operation(this, SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.FractionNumber.ctor, n));
        },
        BinaryOperation$$Number$$Func$3$FractionNumber$FractionNumber$Number: function (n, operation)
        {
            return this.BinaryOperation$1$$Number$$Func$3(SharpAlg.Native.Number.ctor, n, operation);
        }
    }
};
JsTypes.push(SharpAlg$Native$Numbers$FractionNumber);
var SharpAlg$Native$Numbers$LongIntegerNumber =
{
    fullname: "SharpAlg.Native.Numbers.LongIntegerNumber",
    baseTypeName: "SharpAlg.Native.Number",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Numbers.LongIntegerNumber.Zero = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor([], false);
            SharpAlg.Native.Numbers.LongIntegerNumber.One = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor([1], false);
            SharpAlg.Native.Numbers.LongIntegerNumber.Two = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor([2], false);
            SharpAlg.Native.Numbers.LongIntegerNumber.MinusOne = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor([1], true);
            SharpAlg.Native.Numbers.LongIntegerNumber.Base = 10;
            SharpAlg.Native.Numbers.LongIntegerNumber.BaseCount = 4;
            SharpAlg.Native.Numbers.LongIntegerNumber.BaseFull = 10000;
        },
        FromLongIntStringCore: function (s)
        {
            var digits = new System.Collections.Generic.List$1.ctor(System.Int32.ctor);
            var currentPart = 0;
            var currentIndex = 0;
            var currentPower = 1;
            var ZeroCode = SharpAlg.Native.PlatformHelper.CharToInt("0");
            var lastIndex = s.charAt(0) == "-" ? 1 : 0;
            for (var i = s.length - 1; i >= lastIndex; i--)
            {
                currentPart += (SharpAlg.Native.PlatformHelper.CharToInt(s.charAt(i)) - ZeroCode) * currentPower;
                currentIndex++;
                currentPower *= 10;
                if (currentIndex == 4)
                {
                    currentIndex = 0;
                    digits.Add(currentPart);
                    currentPart = 0;
                    currentPower = 1;
                }
            }
            if (currentIndex > 0 && currentPart > 0)
            {
                digits.Add(currentPart);
            }
            return new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(digits, lastIndex == 1);
        },
        Compare$$LongIntegerNumber$$LongIntegerNumber: function (n1, n2)
        {
            if (!System.Linq.Enumerable.Any$1$$IEnumerable$1(System.Int32.ctor, n1.parts) && !System.Linq.Enumerable.Any$1$$IEnumerable$1(System.Int32.ctor, n2.parts))
                return 0;
            if (!System.Linq.Enumerable.Any$1$$IEnumerable$1(System.Int32.ctor, n2.parts))
                return n1.isNegative ? -1 : 1;
            if (!System.Linq.Enumerable.Any$1$$IEnumerable$1(System.Int32.ctor, n1.parts))
                return n2.isNegative ? 1 : -1;
            if (n1.isNegative && !n2.isNegative)
                return -1;
            if (!n1.isNegative && n2.isNegative)
                return 1;
            var partsComparisonResult = SharpAlg.Native.Numbers.LongIntegerNumber.CompareCore(n1.parts, n2.parts);
            return n1.isNegative ? -partsComparisonResult : partsComparisonResult;
        },
        CompareCore: function (parts1, parts2)
        {
            var lenDifference = parts1.get_Count() - parts2.get_Count();
            if (lenDifference != 0)
                return lenDifference;
            var count = parts1.get_Count();
            for (var i = count - 1; i >= 0; i--)
            {
                var difference = parts1.get_Item$$Int32(i) - parts2.get_Item$$Int32(i);
                if (difference != 0)
                    return difference;
            }
            return 0;
        },
        AddImpl: function (left, right, isLeftNegative, isRightNegative)
        {
            var count = System.Math.Max$$Int32$$Int32(left.get_Count(), right.get_Count());
            var result = new System.Collections.Generic.List$1.ctor(System.Int32.ctor);
            var carry = 0;
            for (var i = 0; i < count; i++)
            {
                var resultPart = SharpAlg.Native.Numbers.LongIntegerNumber.GetPart(left, i, isLeftNegative) + SharpAlg.Native.Numbers.LongIntegerNumber.GetPart(right, i, isRightNegative) + carry;
                if (resultPart >= 10000)
                {
                    resultPart = resultPart - 10000;
                    carry = 1;
                }
                else if (resultPart < 0)
                {
                    resultPart = resultPart + 10000;
                    carry = -1;
                }
                else
                {
                    carry = 0;
                }
                result.Add(resultPart);
            }
            if (carry > 0)
                result.Add(carry);
            for (var i = result.get_Count() - 1; i >= 0; i--)
            {
                if (result.get_Item$$Int32(i) == 0)
                    result.RemoveAt(i);
                else
                    break;
            }
            return result;
        },
        MultiplyCore: function (left, right)
        {
            if (right.get_Count() == 0)
                return SharpAlg.Native.Numbers.LongIntegerNumber.Zero.parts;
            var result = [];
            var rightCount = right.get_Count();
            for (var i = 0; i < rightCount; i++)
            {
                result = SharpAlg.Native.Numbers.LongIntegerNumber.AddImpl(result, SharpAlg.Native.Numbers.LongIntegerNumber.MultiplyOneDigit(left, right.get_Item$$Int32(i), i), false, false);
            }
            return result;
        },
        MultiplyOneDigit: function (left, digit, shift)
        {
            var result = new System.Collections.Generic.List$1.ctor(System.Int32.ctor);
            var carry = 0;
            var leftCount = left.get_Count();
            SharpAlg.Native.Numbers.LongIntegerNumber.AddTrailingZeros(result, shift);
            for (var leftIndex = 0; leftIndex < leftCount; leftIndex++)
            {
                var resultPart = left.get_Item$$Int32(leftIndex) * digit + carry;
                if (resultPart >= 10000)
                {
                    var remain = resultPart % 10000;
                    carry = (resultPart - remain) / 10000;
                    resultPart = remain;
                }
                else
                {
                    carry = 0;
                }
                result.Add(resultPart);
            }
            if (carry > 0)
                result.Add(carry);
            return result;
        },
        AddTrailingZeros: function (result, shift)
        {
            for (var i = 0; i < shift; i++)
            {
                result.Add(0);
            }
        },
        ShiftLeft: function (result)
        {
            result.Insert(0, 0);
        },
        ShiftRight: function (result)
        {
            result.RemoveAt(0);
        },
        DivieCore: function (dividentParts, originalDivisor, isNegative, allowFraction)
        {
            var remain = SharpAlg.Native.Numbers.LongIntegerNumber.Zero;
            var result = (function ()
            {
                remain = {Value: remain};
                var $res = SharpAlg.Native.Numbers.LongIntegerNumber.DivieImpl(dividentParts, originalDivisor, remain);
                remain = remain.Value;
                return $res;
            })();
            if (allowFraction && SharpAlg.Native.Number.op_Inequality(remain, SharpAlg.Native.Numbers.LongIntegerNumber.Zero))
                return SharpAlg.Native.Numbers.FractionNumber.Create(new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(dividentParts, isNegative), new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(originalDivisor, false));
            return new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(result, isNegative);
        },
        DivieImpl: function (dividentParts, originalDivisor, remain)
        {
            remain.Value = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(dividentParts, false);
            var divisor = new System.Collections.Generic.List$1.ctor$$IEnumerable$1(System.Int32.ctor, originalDivisor);
            var shiftCount = 0;
            while (SharpAlg.Native.Numbers.LongIntegerNumber.CompareCore(remain.Value.parts, divisor) >= 0)
            {
                SharpAlg.Native.Numbers.LongIntegerNumber.ShiftLeft(divisor);
                shiftCount++;
            }
            SharpAlg.Native.Numbers.LongIntegerNumber.ShiftRight(divisor);
            shiftCount--;
            var result = new System.Collections.Generic.List$1.ctor(System.Int32.ctor);
            while (divisor.get_Count() >= originalDivisor.get_Count())
            {
                var digit = SharpAlg.Native.Numbers.LongIntegerNumber.FindDigit(remain.Value, divisor);
                result.Insert(0, digit);
                var temp = (new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(divisor, false)).Multiply(new SharpAlg.Native.Numbers.LongIntegerNumber.ctor([digit], false));
                remain.Value = Cast(remain.Value.Subtract(temp), SharpAlg.Native.Numbers.LongIntegerNumber.ctor);
                if (SharpAlg.Native.Numbers.LongIntegerNumber.CompareCore(remain.Value.parts, divisor) < 0)
                    SharpAlg.Native.Numbers.LongIntegerNumber.ShiftRight(divisor);
            }
            return result;
        },
        Div: function (x, y)
        {
            var remain = x % y;
            return (x - remain) / y;
        },
        Bisect: function (lowDigit, topDigit)
        {
            return lowDigit + SharpAlg.Native.Numbers.LongIntegerNumber.Div(topDigit - lowDigit, 2);
        },
        FindDigit: function (divident, divisorParts)
        {
            var divisor = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(divisorParts, false);
            if (SharpAlg.Native.Number.op_Equality(divident, SharpAlg.Native.Numbers.LongIntegerNumber.Zero) || SharpAlg.Native.Number.op_GreaterThan(divisor, divident))
            {
                return 0;
            }
            var dividentPart = ((divident.parts.get_Count() == divisor.parts.get_Count()) ? System.Linq.Enumerable.Last$1$$IEnumerable$1(System.Int32.ctor, divident.parts) : System.Linq.Enumerable.Last$1$$IEnumerable$1(System.Int32.ctor, divident.parts) * 10000 + divident.parts.get_Item$$Int32(divident.parts.get_Count() - 2));
            var divisorPart = System.Linq.Enumerable.Last$1$$IEnumerable$1(System.Int32.ctor, divisor.parts);
            var lowDigit = SharpAlg.Native.Numbers.LongIntegerNumber.Div(dividentPart, divisorPart + 1);
            var checkCount = 0;
            var topDigit = SharpAlg.Native.Numbers.LongIntegerNumber.Div(dividentPart + 1, divisorPart);
            var digit = SharpAlg.Native.Numbers.LongIntegerNumber.Bisect(lowDigit, topDigit);
            while (true)
            {
                var mult = divisor.Multiply(new SharpAlg.Native.Numbers.LongIntegerNumber.ctor([digit], false));
                var remain = Cast(divident.Subtract(mult), SharpAlg.Native.Numbers.LongIntegerNumber.ctor);
                if (remain.isNegative)
                {
                    checkCount++;
                    topDigit = digit;
                    digit = SharpAlg.Native.Numbers.LongIntegerNumber.Bisect(lowDigit, topDigit);
                    continue;
                }
                var comparisonResult = remain.Compare$$Number(divisor);
                if (comparisonResult >= 0)
                {
                    checkCount++;
                    lowDigit = digit + 1;
                    digit = SharpAlg.Native.Numbers.LongIntegerNumber.Bisect(lowDigit, topDigit);
                    continue;
                }
                break;
            }
            if (checkCount > 15)
                throw $CreateException(new System.InvalidOperationException.ctor(), new Error());
            return digit;
        },
        FastPower: function (a, b)
        {
            var re = SharpAlg.Native.Numbers.LongIntegerNumber.One;
            while (SharpAlg.Native.Number.op_Inequality(b, SharpAlg.Native.Numbers.LongIntegerNumber.Zero))
            {
                if (System.Linq.Enumerable.First$1$$IEnumerable$1(System.Int32.ctor, b.parts) % 2 == 1)
                    re = SharpAlg.Native.Number.op_Multiply(re, a);
                a = (SharpAlg.Native.Number.op_Multiply(a, a));
                b = b.IntDivide(SharpAlg.Native.Numbers.LongIntegerNumber.Two);
            }
            return b.isNegative ? (SharpAlg.Native.Number.op_Division(SharpAlg.Native.Numbers.LongIntegerNumber.One, re)) : re;
        },
        GetPart: function (parts, index, isNegative)
        {
            return index < parts.get_Count() ? (isNegative ? -parts.get_Item$$Int32(index) : parts.get_Item$$Int32(index)) : 0;
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function (parts, isNegative)
        {
            this.isNegative = false;
            this.parts = null;
            SharpAlg.Native.Number.ctor.call(this);
            this.isNegative = isNegative;
            this.parts = parts;
        },
        NumberType$$: "System.Int32",
        get_NumberType: function ()
        {
            return 0;
        },
        ConvertToCore: function (type)
        {
            if (type == 2)
            {
                var result = 0;
                var count = this.parts.get_Count();
                for (var i = count - 1; i >= 0; i--)
                {
                    result = result * 10000 + this.parts.get_Item$$Int32(i);
                }
                return new SharpAlg.Native.Numbers.FloatNumber.ctor(this.isNegative ? -result : result);
            }
            if (type == 1)
            {
                return new SharpAlg.Native.Numbers.FractionNumber.ctor(this, SharpAlg.Native.Numbers.LongIntegerNumber.One);
            }
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
        },
        Compare$$Number: function (n)
        {
            var other = Cast(n, SharpAlg.Native.Numbers.LongIntegerNumber.ctor);
            return SharpAlg.Native.Numbers.LongIntegerNumber.Compare$$LongIntegerNumber$$LongIntegerNumber(this, other);
        },
        toString: function ()
        {
            var startIndex = this.parts.get_Count() - 1;
            var sb = new System.Text.StringBuilder.ctor();
            if (this.parts.get_Count() == 0)
                return "0";
            if (this.isNegative)
                sb.Append$$String("-");
            for (var i = startIndex; i >= 0; i--)
            {
                var stringValue = this.parts.get_Item$$Int32(i).toString();
                if (i != startIndex)
                {
                    for (var j = 4 - stringValue.length; j > 0; j--)
                    {
                        sb.Append$$Char("0");
                    }
                }
                sb.Append$$String(stringValue);
            }
            return sb.toString();
        },
        Add: function (n)
        {
            var longNumber = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, n);
            if (!this.isNegative && !longNumber.isNegative)
                return this.AddCore(longNumber);
            if (longNumber.isNegative)
            {
                var invertedRight = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(longNumber.parts, false);
                if (SharpAlg.Native.Number.op_LessThan(this, invertedRight))
                    return new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, invertedRight.Subtract(this)).parts, true);
            }
            else
            {
                var invertedLeft = new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(this.parts, false);
                if (SharpAlg.Native.Number.op_LessThan(longNumber, invertedLeft))
                    return new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, invertedLeft.Subtract(longNumber)).parts, true);
            }
            return this.AddCore(longNumber);
        },
        Subtract: function (n)
        {
            var longNumber = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, n);
            return this.Add(new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(longNumber.parts, !longNumber.isNegative));
        },
        AddCore: function (longNumber)
        {
            return new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(SharpAlg.Native.Numbers.LongIntegerNumber.AddImpl(this.parts, longNumber.parts, this.isNegative, longNumber.isNegative), false);
        },
        Multiply: function (n)
        {
            var longNumber = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, n);
            return new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(SharpAlg.Native.Numbers.LongIntegerNumber.MultiplyCore(this.parts, longNumber.parts), this.isNegative ^ longNumber.isNegative);
        },
        Divide: function (n)
        {
            return this.Divide$$Number$$Boolean(n, true);
        },
        IntDivide: function (n)
        {
            return Cast(this.Divide$$Number$$Boolean(n, false), SharpAlg.Native.Numbers.LongIntegerNumber.ctor);
        },
        Modulo: function (n)
        {
            var longNumber = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, n);
            var remain = SharpAlg.Native.Numbers.LongIntegerNumber.Zero;
            (function ()
            {
                remain = {Value: remain};
                var $res = SharpAlg.Native.Numbers.LongIntegerNumber.DivieImpl(this.parts, longNumber.parts, remain);
                remain = remain.Value;
                return $res;
            }).call(this);
            return remain;
        },
        Divide$$Number$$Boolean: function (n, allowFraction)
        {
            var longNumber = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, n);
            var isResultNegative = this.isNegative ^ longNumber.isNegative;
            if (allowFraction && SharpAlg.Native.Numbers.LongIntegerNumber.CompareCore(this.parts, longNumber.parts) < 0)
                return SharpAlg.Native.Numbers.FractionNumber.Create(new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(this.parts, isResultNegative), new SharpAlg.Native.Numbers.LongIntegerNumber.ctor(longNumber.parts, false));
            return SharpAlg.Native.Numbers.LongIntegerNumber.DivieCore(this.parts, longNumber.parts, isResultNegative, allowFraction);
        },
        Power: function (n)
        {
            var b = SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.LongIntegerNumber.ctor, n);
            return SharpAlg.Native.Numbers.LongIntegerNumber.FastPower(this, b);
        }
    }
};
JsTypes.push(SharpAlg$Native$Numbers$LongIntegerNumber);
var SharpAlg$Native$Number =
{
    fullname: "SharpAlg.Native.Number",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Number.IntegerNumberType = 0;
            SharpAlg.Native.Number.FractionNumberType = 1;
            SharpAlg.Native.Number.FloatNumberType = 2;
        },
        ToSameType: function (n1, n2)
        {
            var type = System.Math.Max$$Int32$$Int32(n1.Value.get_NumberType(), n2.Value.get_NumberType());
            n1.Value = n1.Value.ConvertTo(type);
            n2.Value = n2.Value.ConvertTo(type);
        },
        op_Equality: function (n1, n2)
        {
            if (n1 != null && n2 != null)
                (function ()
                {
                    n1 = {Value: n1};
                    n2 = {Value: n2};
                    var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                    n1 = n1.Value;
                    n2 = n2.Value;
                    return $res;
                })();
            return System.Object.Equals$$Object$$Object(n1, n2);
        },
        op_Inequality: function (n1, n2)
        {
            return !(SharpAlg.Native.Number.op_Equality(n1, n2));
        },
        op_GreaterThanOrEqual: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return !n1.Less(n2);
        },
        op_LessThanOrEqual: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return !n1.Greater(n2);
        },
        op_LessThan: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Less(n2);
        },
        op_GreaterThan: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Greater(n2);
        },
        Compare$$Number$$Number: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Compare$$Number(n2);
        },
        op_Multiply: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Multiply(n2);
        },
        op_Division: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Divide(n2);
        },
        op_Addition: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Add(n2);
        },
        op_Subtraction: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Subtract(n2);
        },
        op_ExclusiveOr: function (n1, n2)
        {
            (function ()
            {
                n1 = {Value: n1};
                n2 = {Value: n2};
                var $res = SharpAlg.Native.Number.ToSameType(n1, n2);
                n1 = n1.Value;
                n2 = n2.Value;
                return $res;
            })();
            return n1.Power(n2);
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        },
        ToFloat: function ()
        {
            return this.ConvertTo(2);
        },
        ConvertTo: function (type)
        {
            if (type < this.get_NumberType())
                throw $CreateException(new System.NotImplementedException.ctor(), new Error());
            if (type == this.get_NumberType())
                return this;
            return this.ConvertToCore(type);
        },
        NumberType$$: "System.Int32",
        Less: function (n)
        {
            return this.Compare$$Number(n) < 0;
        },
        Greater: function (n)
        {
            return this.Compare$$Number(n) > 0;
        },
        Equals$$Object: function (obj)
        {
            var this_ = this;
            var other = As(obj, SharpAlg.Native.Number.ctor);
            (function ()
            {
                this_ = {Value: this_};
                other = {Value: other};
                var $res = SharpAlg.Native.Number.ToSameType(this_, other);
                this_ = this_.Value;
                other = other.Value;
                return $res;
            }).call(this);
            return this_.Compare$$Number(other) == 0;
        },
        GetHashCode: function ()
        {
            throw $CreateException(new System.NotSupportedException.ctor(), new Error());
        },
        IsInteger$$: "System.Boolean",
        get_IsInteger: function ()
        {
            return this.get_NumberType() == 0;
        },
        IsFraction$$: "System.Boolean",
        get_IsFraction: function ()
        {
            return this.get_NumberType() == 1;
        },
        IsFloat$$: "System.Boolean",
        get_IsFloat: function ()
        {
            return this.get_NumberType() == 2;
        }
    }
};
JsTypes.push(SharpAlg$Native$Number);
var SharpAlg$Native$NumberFactory =
{
    fullname: "SharpAlg.Native.NumberFactory",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.NumberFactory.Zero = null;
            SharpAlg.Native.NumberFactory.One = null;
            SharpAlg.Native.NumberFactory.Two = null;
            SharpAlg.Native.NumberFactory.MinusOne = null;
            SharpAlg.Native.NumberFactory.Pi = null;
            SharpAlg.Native.NumberFactory.Zero = SharpAlg.Native.Numbers.LongIntegerNumber.Zero;
            SharpAlg.Native.NumberFactory.One = SharpAlg.Native.Numbers.LongIntegerNumber.One;
            SharpAlg.Native.NumberFactory.Two = SharpAlg.Native.Numbers.LongIntegerNumber.Two;
            SharpAlg.Native.NumberFactory.MinusOne = SharpAlg.Native.Numbers.LongIntegerNumber.MinusOne;
            SharpAlg.Native.NumberFactory.Pi = SharpAlg.Native.NumberFactory.FromDouble(3.14159265358979);
        },
        GetFloat: function (n, evaluator)
        {
            return SharpAlg.Native.NumberFactory.FromDouble(evaluator(SharpAlg.Native.FunctionalExtensions.ConvertCast$1(SharpAlg.Native.Numbers.FloatNumber.ctor, n.ToFloat()).value));
        },
        FromDouble: function (value)
        {
            return new SharpAlg.Native.Numbers.FloatNumber.ctor(value);
        },
        ToDouble: function (number)
        {
            return (Cast(SharpAlg.Native.NumberFactory.GetFloat(number, function (x)
            {
                return x;
            }), SharpAlg.Native.Numbers.FloatNumber.ctor)).value;
        },
        FromString: function (s)
        {
            return SharpAlg.Native.NumberFactory.FromDouble(SharpAlg.Native.PlatformHelper.Parse(s));
        },
        FromIntString: function (s)
        {
            return SharpAlg.Native.Numbers.LongIntegerNumber.FromLongIntStringCore(s);
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$NumberFactory);
