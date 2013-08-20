using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharpAlg.Native.Numbers {
    [JsType(JsMode.Clr, Filename = SR.JS_Core_Number)]
    internal sealed class FractionNumber : Number {
        readonly LongIntegerNumber denominator;
        readonly LongIntegerNumber numerator;
        internal static Number Create(LongIntegerNumber numerator, LongIntegerNumber denominator) {
            return new FractionNumber(numerator, denominator);
        }
        FractionNumber(LongIntegerNumber numerator, LongIntegerNumber denominator) {
            if(denominator < NumberFactory.Zero)
                throw new ArgumentException("denominator");
            this.denominator = denominator;
            this.numerator = numerator;
        }
        public override string ToString() {
            return numerator.ToString() + "/" + denominator.ToString();
        }
        protected override Number ConvertToCore(int type) {
            throw new NotImplementedException();
        }

        protected override int NumberType {
            get { throw new NotImplementedException(); }
        }

        protected override Number Add(Number n) {
            throw new NotImplementedException();
        }

        protected override Number Subtract(Number n) {
            throw new NotImplementedException();
        }

        protected override Number Multiply(Number n) {
            throw new NotImplementedException();
        }

        protected override Number Divide(Number n) {
            throw new NotImplementedException();
        }

        protected override Number Power(Number n) {
            throw new NotImplementedException();
        }

        protected override int Compare(Number n) {
            throw new NotImplementedException();
        }
    }
}
