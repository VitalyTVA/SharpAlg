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
            //"1593668734".Divide("1287293").AssertIntegerNumber("1238");
            //"117".Power("5").AssertIntegerNumber("21924480357");
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

            "999999999999999999999999".Multiply("999").AssertIntegerNumber("998999999999999999999999001"); //TODO - long arithmetic
            "10000000000000100000000001".Multiply("500").AssertIntegerNumber("5000000000000050000000000500"); //TODO - long arithmetic
            //"1000000001".Multiply("500000001").AssertIntegerNumber("500000001500000001"); //TODO - long arithmetic
            //(2.AsNumber() ^ 50.AsNumber()).IsEqual(1500000002.AsNumber()); //TODO - long arithmetic
        }
        [Test]
        public void LongIntOperationsTest() {
            "123456789123456789123456789".FromString().AssertIntegerNumber("123456789123456789123456789");
            "-123456789123456789123456789".FromString().AssertIntegerNumber("-123456789123456789123456789");
            "100000000000000000000000009".FromString().AssertIntegerNumber("100000000000000000000000009");
            "0".FromString().AssertIntegerNumber("0");
            "-0".FromString().AssertIntegerNumber("0");

            "9999".Add("1").AssertIntegerNumber("10000");
            "9999".Add("9999").AssertIntegerNumber("19998");
            "999999999".Add("999999999").AssertIntegerNumber("1999999998");
            "1000000010000000000".Add("999999999999999999").AssertIntegerNumber("2000000009999999999");
            "123123123123".Add("231231231231").AssertIntegerNumber("354354354354");
            "123123123123".Add("231231231231123456").AssertIntegerNumber("231231354354246579");
            "231231231231123456".Add("123123123123").AssertIntegerNumber("231231354354246579");
            "123456789123456789123456789".Add("123456789123456789123456789").AssertIntegerNumber("246913578246913578246913578");

            "123456789123456789123456789".Subtract("123456789123456789123456789").AssertIntegerNumber("0");
            "123456789123456789123456789".Subtract("123456789123456789123456788").AssertIntegerNumber("1");
            "123456789123456789123456789123456789123456789".Subtract("123456789123456789023456789123456789123456788").AssertIntegerNumber("100000000000000000000000001");
            "234567892345678923456789".Subtract("123456781234567812345678").AssertIntegerNumber("111111111111111111111111");
            "100000000000000000".Subtract("99999999999999999").AssertIntegerNumber("1");
            "19999999999999999900000000000000000".Subtract("9999999999999999999999999999999999").AssertIntegerNumber("9999999999999999900000000000000001");

            "123456789123456789123456789".Add("-123456789123456789123456789").AssertIntegerNumber("0");
            "123456789123456789123456789".Add("-123456789123456789123456788").AssertIntegerNumber("1");
            "123456789123456789123456789123456789123456789".Add("-123456789123456789023456789123456789123456788").AssertIntegerNumber("100000000000000000000000001");
            "234567892345678923456789".Add("-123456781234567812345678").AssertIntegerNumber("111111111111111111111111");
            "100000000000000000".Add("-99999999999999999").AssertIntegerNumber("1");
            "19999999999999999900000000000000000".Add("-9999999999999999999999999999999999").AssertIntegerNumber("9999999999999999900000000000000001");

            "9999".Subtract("-1").AssertIntegerNumber("10000");
            "9999".Subtract("-9999").AssertIntegerNumber("19998");
            "999999999".Subtract("-999999999").AssertIntegerNumber("1999999998");
            "1000000010000000000".Subtract("-999999999999999999").AssertIntegerNumber("2000000009999999999");
            "123123123123".Subtract("-231231231231").AssertIntegerNumber("354354354354");
            "123123123123".Subtract("-231231231231123456").AssertIntegerNumber("231231354354246579");
            "231231231231123456".Subtract("-123123123123").AssertIntegerNumber("231231354354246579");
            "123456789123456789123456789".Subtract("-123456789123456789123456789").AssertIntegerNumber("246913578246913578246913578");

            "-1".Less("1").IsTrue();
            "-1".Greater("1").IsFalse();
            "1".Less("-1").IsFalse();
            "-1".Less("1").IsTrue();
            "-1".Less("0").IsTrue();
            "-1".Greater("0").IsFalse();
            "1".Less("0").IsFalse();
            "1".Greater("0").IsTrue();
            "0".Less("-1").IsFalse();
            "0".Greater("-1").IsTrue();
            "0".Less("1").IsTrue();
            "0".Greater("1").IsFalse();
            "0".Less("0").IsFalse();
            "0".Greater("0").IsFalse();
            "0".GreaterOrEqual("0").IsTrue();
            "0".LessOrEqual("0").IsTrue();
            "0".Equal("0").IsTrue();
            "100".Less("1000000000000000000000001").IsTrue();
            "-100".Greater("-1000000000000000000000001").IsTrue();
            "1000000000000000000001".Equal("1000000000000000000001").IsTrue();
            "1000000000000000000001".Equal("1000000000000000000000").IsFalse();
            "1000000000000000000001".NotEqual("1000000000000000000000").IsTrue();
            "1000000000000000000001".NotEqual("1000000000000000000001").IsFalse();
            "1000000000000000000001".Less("1000000000000000000001").IsFalse();
            "1000000000000000000001".Less("1000000000000000000002").IsTrue();
            "1000000000000000000001".LessOrEqual("1000000000000000000002").IsTrue();
            "1000000000000000000002".LessOrEqual("1000000000000000000001").IsFalse();
            "1000000000000000000001".Greater("1000000000000000000001").IsFalse();
            "1000000000000000000002".Greater("1000000000000000000001").IsTrue();
            "1000000000000000000001".GreaterOrEqual("1000000000000000000001").IsTrue();
            "1000000000000000000001".GreaterOrEqual("1000000000000000000002").IsFalse();
            "100000000000000000".Less("99999999999999999").IsFalse();

            "0".Subtract("1").AssertIntegerNumber("-1");
            "-1".Add("0").AssertIntegerNumber("-1");
            "-1".Add("-10000000000000").AssertIntegerNumber("-10000000000001");
            "-10000000000000".Add("-1").AssertIntegerNumber("-10000000000001");
            "0".Subtract("999999999999999999999").AssertIntegerNumber("-999999999999999999999");
            "1".Subtract("999999999999999999999").AssertIntegerNumber("-999999999999999999998");
            "-1".Subtract("999999999999999999999").AssertIntegerNumber("-1000000000000000000000");
            "1000000000000000000000000000".Subtract("999999999999999999999999999999999999999999").AssertIntegerNumber("-999999999999998999999999999999999999999999");
            "-1000000000000000000000000000".Subtract("999999999999999999999999999999999999999999").AssertIntegerNumber("-1000000000000000999999999999999999999999999");
            "-999999999".Subtract("999999999").AssertIntegerNumber("-1999999998");
            "-999999999".Subtract("-999999999").AssertIntegerNumber("0");

            //(2.AsNumber() ^ 50.AsNumber()).IsEqual(1500000002.AsNumber()); //TODO - long arithmetic
        }
        //[Test]
        //public void FloatIntOperationsTest() {
        //    "1.0".Add("2").AssertFloatNumber("3");
        //    "1".Add("2.0").AssertFloatNumber("3");
        //    "1".Add("2.3").AssertFloatNumber("3.3");
        //    "9".Multiply("13.0").AssertFloatNumber("117");
        //    "9.0".Multiply("13").AssertFloatNumber("117");
        //    "9".Subtract("13.0").AssertFloatNumber("-4");
        //    "9.0".Subtract("13").AssertFloatNumber("-4");
        //    "3".Divide("2.0").AssertFloatNumber("1.5");
        //    "3.0".Divide("2").AssertFloatNumber("1.5");
        //    "4.0".Divide("2").AssertFloatNumber("2");
        //    "117".Power("5.0").AssertFloatNumber("21924480357");
        //    "117.0".Power("5").AssertFloatNumber("21924480357");

        //    "100000000001".Equal("100000000001.0").IsTrue();
        //    "100000000001.0".Equal("100000000000").IsFalse();
        //    "100000000001".NotEqual("100000000000.0").IsTrue();
        //    "100000000001.0".NotEqual("100000000001").IsFalse();

        //    "100000000001".Less("100000000001.0").IsFalse();
        //    "100000000001.0".Less("100000000002").IsTrue();
        //    "100000000001".LessOrEqual("100000000002.0").IsTrue();
        //    "100000000002.0".LessOrEqual("100000000001").IsFalse();

        //    "100000000001".Greater("100000000001.0").IsFalse();
        //    "100000000002.0".Greater("100000000001").IsTrue();
        //    "100000000001".GreaterOrEqual("100000000001.0").IsTrue();
        //    "100000000001.0".GreaterOrEqual("100000000002").IsFalse();
        //}
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class NumberTestHelper {
        public static Number AssertFloatNumber(this Number n, string expected) {
            return n.IsEqual(x => x.ToString(), expected).IsTrue(x => x is FloatNumber);
        }
        public static Number AssertIntegerNumber(this Number n, string expected) {
            return n.IsEqual(x => x.ToString(), expected).IsTrue(x => x is LongIntegerNumber);
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
        public static Number FromString(this string s) {
            return s.Contains(".") ? Number.FromString(s) : Number.FromLongIntString(s);
        }
    }
}
