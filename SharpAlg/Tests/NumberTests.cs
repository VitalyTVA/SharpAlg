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
        public void IntOperationsTest() {
            "1".Add("2").AssertNumber("3");
            "9".Multiply("13").AssertNumber("117");
            "9".Subtract("13").AssertNumber("-4");
            "1593668734".Divide("1287293").AssertNumber("1238");
            "117".Power("5").AssertNumber("21924480357");
            "1000000001".Add("500000001").AssertNumber("1500000002");
            "-1000000001".Add("500000001").AssertNumber("-500000000");

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

            //"1000000001".Multiply("500000001").AssertNumber("500000001500000001");
            //(2.AsNumber() ^ 50.AsNumber()).IsEqual(1500000002.AsNumber());
        }
        [Test]
        public void IntegerNumberTest() { 

        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSTestsName)]
    public static class NumberTestHelper {
        public static Number AssertNumber(this Number n, string expected) {
            return n.IsEqual(x => x.ToString(), expected);
        }
        public static Number Add(this string s1, string s2) {
            return Number.FromString(s1) + Number.FromString(s2); 
        }
        public static Number Subtract(this string s1, string s2) {
            return Number.FromString(s1) - Number.FromString(s2);
        }
        public static Number Multiply(this string s1, string s2) {
            return Number.FromString(s1) * Number.FromString(s2);
        }
        public static Number Divide(this string s1, string s2) {
            return Number.FromString(s1) / Number.FromString(s2);
        }
        public static Number Power(this string s1, string s2) {
            return Number.FromString(s1) ^ Number.FromString(s2);
        }
        public static bool Equal(this string s1, string s2) {
            return Number.FromString(s1) == Number.FromString(s2);
        }
        public static bool NotEqual(this string s1, string s2) {
            return Number.FromString(s1) != Number.FromString(s2);
        }
        public static bool Less(this string s1, string s2) {
            return Number.FromString(s1) < Number.FromString(s2);
        }
        public static bool LessOrEqual(this string s1, string s2) {
            return Number.FromString(s1) <= Number.FromString(s2);
        }
        public static bool Greater(this string s1, string s2) {
            return Number.FromString(s1) > Number.FromString(s2);
        }
        public static bool GreaterOrEqual(this string s1, string s2) {
            return Number.FromString(s1) >= Number.FromString(s2);
        }

    }
}
