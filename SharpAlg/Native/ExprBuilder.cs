using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class ExprBuilder {
        public abstract Expr Binary(Expr left, Expr right, BinaryOperation operation);
        public Expr Add(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Add);
        }
        public Expr Subtract(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Subtract);
        }
        public Expr Multiply(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Multiply);
        }
        public Expr Divide(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Divide);
        }
        public Expr Power(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Power);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class TrivialExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return Expr.Binary(left, right, operation);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ConvolutionExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return ConstantConvolution(left, right, operation) 
                ?? EqualityConvolution(left, right, operation) 
                ?? Expr.Binary(left, right, operation);
        }
        static Expr ConstantConvolution(Expr left, Expr right, BinaryOperation operation) {
            double? leftConst = GetConstValue(left);

            if(leftConst == 0) {
                if(operation == BinaryOperation.Add)
                    return right;
                if(operation == BinaryOperation.Multiply || operation == BinaryOperation.Divide || operation == BinaryOperation.Power)
                    return Expr.Zero;
            }
            if(leftConst == 1) {
                if(operation == BinaryOperation.Multiply)
                    return right;
                if(operation == BinaryOperation.Power)
                    return Expr.One;
            }

            double? rightConst = GetConstValue(right);

            if(rightConst == 0) {
                if(operation == BinaryOperation.Add || operation == BinaryOperation.Subtract)
                    return left;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Zero;
                if(operation == BinaryOperation.Power)
                    return Expr.One;
            }
            if(rightConst == 1) {
                if(operation == BinaryOperation.Multiply || operation == BinaryOperation.Divide)
                    return left;
                if(operation == BinaryOperation.Power)
                    return left;
            }

            if(rightConst != null && leftConst != null)
                return Expr.Constant(ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst.Value, rightConst.Value));
            return null;
        }
        static Expr EqualityConvolution(Expr left, Expr right, BinaryOperation operation) {
            if(left.ExprEquals(right)) {
                if(operation == BinaryOperation.Add)
                    return Expr.Multiply(Expr.Constant(2), left);
                if(operation == BinaryOperation.Subtract)
                    return Expr.Zero;
                if(operation == BinaryOperation.Divide)
                    return Expr.One;
            }
            return null;
        }
        static double? GetConstValue(Expr expr) {
            var constant = expr as ConstantExpr;
            return constant != null ? (double)constant.Value : (double?)null;
        }
    }
}
