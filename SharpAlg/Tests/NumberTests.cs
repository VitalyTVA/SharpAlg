using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAlg;
using SharpAlg.Native;
using SharpKit.JavaScript;
using SharpAlg.Native.Builder;

namespace SharpAlg.Tests {
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    [TestFixture]
    public class NumberTests {
        [Test]
        public void DoubleOperationsTest() {
            "1.2".Add("2.3").AssertFloatNumber("3.5");
            "9.0".Multiply("13.0").AssertFloatNumber("117");
            "9.0".Subtract("13.0").AssertFloatNumber("-4");
            "3.0".Divide("2.0").AssertFloatNumber("1.5");
            "1593668734.0".Divide("1287293.0").AssertFloatNumber("1238");
            "117.0".Power("5.0").AssertFloatNumber("21924480357");
            "1000000001.0".Add("500000001.0").AssertFloatNumber("1500000002");
            "-1000000001.0".Add("500000001.0").AssertFloatNumber("-500000000");

            "100000000001.0".Equal("100000000001.0").IsTrue();
            "100000000001.0".Equal("100000000000.0").IsFalse();
            "100000000001.0".NotEqual("100000000000.0").IsTrue();
            "100000000001.0".NotEqual("100000000001.0").IsFalse();

            "100000000001.0".Less("100000000001.0").IsFalse();
            "100000000001.0".Less("100000000002.0").IsTrue();
            "100000000001.0".LessOrEqual("100000000002.0").IsTrue();
            "100000000002.0".LessOrEqual("100000000001.0").IsFalse();

            "100000000001.0".Greater("100000000001.0").IsFalse();
            "100000000002.0".Greater("100000000001.0").IsTrue();
            "100000000001.0".GreaterOrEqual("100000000001.0").IsTrue();
            "100000000001.0".GreaterOrEqual("100000000002.0").IsFalse();
        }
        [Test]
        public void IntOperationsTest() {
            "1".Add("2").AssertIntegerNumber("3");
            "9".Subtract("13").AssertIntegerNumber("-4");
            "9".Multiply("13").AssertIntegerNumber("117");
            "1593668734".Divide("1287293").AssertIntegerNumber("1238");
            "117".Power("5").AssertIntegerNumber("21924480357");
            "1000000001".Add("500000001").AssertIntegerNumber("1500000002");
            "-1000000001".Add("500000001").AssertIntegerNumber("-500000000");

            "100000000001".Equal("100000000001").IsTrue();
            "100000000001".Equal("100000000000").IsFalse();
            "100000000001".NotEqual("100000000000").IsTrue();
            "100000000001".NotEqual("100000000001").IsFalse();

            "100000000001".Less("100000000001").IsFalse();
            "100000000001".Less("100000000002").IsTrue();
            "100000000001".LessOrEqual("100000000002").IsTrue();
            "100000000002".LessOrEqual("100000000001").IsFalse();

            "100000000001".Greater("100000000001").IsFalse();
            "100000000002".Greater("100000000001").IsTrue();
            "100000000001".GreaterOrEqual("100000000001").IsTrue();
            "100000000001".GreaterOrEqual("100000000002").IsFalse();

            //"1000000001".Multiply("500000001").AssertIntegerNumber("500000001500000001"); //TODO - long arithmetic
            //(2.AsNumber() ^ 50.AsNumber()).IsEqual(1500000002.AsNumber()); //TODO - long arithmetic
        }
        [Test]
        public void IntegerNumberTest() {
            
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class NumberTestHelper {
        public static Number AssertFloatNumber(this Number n, string expected) {
            return n.IsEqual(x => x.ToString(), expected).IsTrue(x => x is FloatNumber);
        }
        public static Number AssertIntegerNumber(this Number n, string expected) {
            return n.IsEqual(x => x.ToString(), expected).IsTrue(x => x is IntegerNumber);
        }
        public static Number Add(this string s1, string s2) {
            return FromString(s1) + FromString(s2); 
        }
        public static Number Subtract(this string s1, string s2) {
            return FromString(s1) - FromString(s2);
        }
        public static Number Multiply(this string s1, string s2) {
            return FromString(s1) * FromString(s2);
        }
        public static Number Divide(this string s1, string s2) {
            return FromString(s1) / FromString(s2);
        }
        public static Number Power(this string s1, string s2) {
            return FromString(s1) ^ FromString(s2);
        }
        public static bool Equal(this string s1, string s2) {
            return FromString(s1) == FromString(s2);
        }
        public static bool NotEqual(this string s1, string s2) {
            return FromString(s1) != FromString(s2);
        }
        public static bool Less(this string s1, string s2) {
            return FromString(s1) < FromString(s2);
        }
        public static bool LessOrEqual(this string s1, string s2) {
            return FromString(s1) <= FromString(s2);
        }
        public static bool Greater(this string s1, string s2) {
            return FromString(s1) > FromString(s2);
        }
        public static bool GreaterOrEqual(this string s1, string s2) {
            return FromString(s1) >= FromString(s2);
        }
        public static Number FromString(string s) {
            return s.Contains(".") ? Number.FromString(s) : Number.FromIntString(s);
        }
    }
}
