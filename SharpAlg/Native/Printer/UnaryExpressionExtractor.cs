using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpAlg.Native.Printer {
    [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
    public class UnaryExpressionExtractor : DefaultExpressionVisitor<UnaryExpressionInfo> {
        public static UnaryExpressionInfo ExtractUnaryInfo(Expr expr, BinaryOperation operation) {
            return expr.Visit(new UnaryExpressionExtractor(operation));
        }
        public static bool IsMinusExpression(MultiplyExpr multi) {
            return multi.Args.Count() == 2 && Expr.MinusOne.ExprEquals(multi.Args.ElementAt(0));
        }
        public static bool IsInverseExpression(PowerExpr power) {
            return Expr.MinusOne.ExprEquals(power.Right);
        }

        readonly BinaryOperation operation;
        UnaryExpressionExtractor(BinaryOperation operation) {
            this.operation = operation;
        }
        public override UnaryExpressionInfo Constant(ConstantExpr constant) {
            return constant.Value >= Number.Zero || operation != BinaryOperation.Add ?
                base.Constant(constant) :
                new UnaryExpressionInfo(Expr.Constant(Number.Zero - constant.Value), BinaryOperationEx.Subtract);
        }
        public override UnaryExpressionInfo Add(AddExpr multi) {
            return base.Add(multi);
        }
        public override UnaryExpressionInfo Multiply(MultiplyExpr multi) {
            if(operation == BinaryOperation.Add && IsMinusExpression(multi)) {
                return new UnaryExpressionInfo(multi.Args.ElementAt(1), BinaryOperationEx.Subtract);
            }
            return base.Multiply(multi);
        }
        public override UnaryExpressionInfo Power(PowerExpr power) {
            if(operation == BinaryOperation.Multiply && IsInverseExpression(power)) {
                return new UnaryExpressionInfo(power.Left, BinaryOperationEx.Divide);
            }
            return base.Power(power);
        }
        protected override UnaryExpressionInfo GetDefault(Expr expr) {
            return new UnaryExpressionInfo(expr, ExpressionEvaluator.GetBinaryOperationEx(operation));
        }
    }
}
