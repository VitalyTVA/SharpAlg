using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class ExprBuilder {
        public abstract Expr Binary(Expr left, Expr right, BinaryOperation operation);
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
                if(operation == BinaryOperation.Multiply || operation == BinaryOperation.Divide)
                    return Expr.Constant(0);
            }
            if(leftConst == 1) {
                if(operation == BinaryOperation.Multiply)
                    return right;
            }

            double? rightConst = GetConstValue(right);

            if(rightConst == 0) {
                if(operation == BinaryOperation.Add || operation == BinaryOperation.Subtract)
                    return left;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Constant(0);
            }
            if(rightConst == 1) {
                if(operation == BinaryOperation.Multiply || operation == BinaryOperation.Divide)
                    return left;
            }

            if(rightConst != null && leftConst != null)
                return Expr.Constant(ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst.Value, rightConst.Value));
            return null;
        }
        static Expr EqualityConvolution(Expr left, Expr right, BinaryOperation operation) {
            if(left.ExprEquals(right)) {
                if(operation == BinaryOperation.Add)
                    return Expr.Binary(Expr.Constant(2), left, BinaryOperation.Multiply);
                if(operation == BinaryOperation.Subtract)
                    return Expr.Constant(0);
                if(operation == BinaryOperation.Divide)
                    return Expr.Constant(1);
            }
            return null;
        }
        static double? GetConstValue(Expr expr) {
            var constant = expr as ConstantExpr;
            return constant != null ? (double)constant.Value : (double?)null;
        }
    }
}
