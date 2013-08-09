/*Generated by SharpKit 5 v5.01.1000*/
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Native$Number =
{
    fullname: "SharpAlg.Native.Number",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        op_Equality: function (n1, n2)
        {
            return System.Object.Equals$$Object$$Object(n1, n2);
        },
        op_Inequality: function (n1, n2)
        {
            return !System.Object.Equals$$Object$$Object(n1, n2);
        },
        op_GreaterThanOrEqual: function (n1, n2)
        {
            return n1.value >= n2.value;
        },
        op_LessThanOrEqual: function (n1, n2)
        {
            return n1.value <= n2.value;
        },
        op_LessThan: function (n1, n2)
        {
            return n1.value < n2.value;
        },
        op_GreaterThan: function (n1, n2)
        {
            return n1.value > n2.value;
        },
        op_Multiply: function (n1, n2)
        {
            return SharpAlg.Native.Number.FromDouble(n1.value * n2.value);
        },
        op_Addition: function (n1, n2)
        {
            return SharpAlg.Native.Number.FromDouble(n1.value + n2.value);
        },
        op_Subtraction: function (n1, n2)
        {
            return SharpAlg.Native.Number.FromDouble(n1.value - n2.value);
        },
        op_ExclusiveOr: function (n1, n2)
        {
            return SharpAlg.Native.Number.FromDouble(System.Math.Pow(n1.value, n2.value));
        },
        Ln: function (n)
        {
            return SharpAlg.Native.Number.FromDouble(System.Math.Log$$Double(n.value));
        },
        cctor: function ()
        {
            SharpAlg.Native.Number.Zero = null;
            SharpAlg.Native.Number.One = null;
            SharpAlg.Native.Number.Two = null;
            SharpAlg.Native.Number.MinusOne = null;
            SharpAlg.Native.Number.Zero = SharpAlg.Native.Number.FromDouble(0);
            SharpAlg.Native.Number.One = SharpAlg.Native.Number.FromDouble(1);
            SharpAlg.Native.Number.Two = SharpAlg.Native.Number.FromDouble(2);
            SharpAlg.Native.Number.MinusOne = SharpAlg.Native.Number.FromDouble(-1);
        },
        FromDouble: function (value)
        {
            return new SharpAlg.Native.Number.ctor(value);
        },
        FromString: function (s)
        {
            return new SharpAlg.Native.Number.ctor(SharpAlg.Native.PlatformHelper.Parse(s));
        }
    },
    assemblyName: "SharpArg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function (value)
        {
            this.value = 0;
            System.Object.ctor.call(this);
            this.value = value;
        },
        Equals$$Object: function (obj)
        {
            var other = As(obj, SharpAlg.Native.Number.ctor);
            return SharpAlg.Native.Number.op_Inequality(other, null) && other.value == this.value;
        },
        GetHashCode: function ()
        {
            return System.Object.commonPrototype.GetHashCode.call(this);
        },
        toString: function ()
        {
            return SharpAlg.Native.PlatformHelper.ToInvariantString(this.value);
        }
    }
};
JsTypes.push(SharpAlg$Native$Number);
