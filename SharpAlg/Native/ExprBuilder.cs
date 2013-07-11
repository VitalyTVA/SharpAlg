using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class ExprBuilder {
        public abstract Expr Binary(Expr left, Expr right, BinaryOperation operation);
        public abstract Expr Unary(Expr expr, UnaryOperation operation);
        public Expr Add(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Add);
        }
        public Expr Subtract(Expr left, Expr right) {
            return Add(left, Minus(right));
        }
        public Expr Multiply(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Multiply);
        }
        public Expr Divide(Expr left, Expr right) {
            return Multiply(left, Inverse(right));
        }
        public Expr Power(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Power);
        }
        public Expr Minus(Expr expr) {
            return Unary(expr, UnaryOperation.Minus);
        }
        public Expr Inverse(Expr expr) {
            return Unary(expr, UnaryOperation.Inverse);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class TrivialExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return Expr.Binary(left, right, operation);
        }
        public override Expr Unary(Expr expr, UnaryOperation operation) {
            return new UnaryExpr(expr, operation);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ConvolutionExprBuilder : ExprBuilder {
        public override Expr Unary(Expr expr, UnaryOperation operation) {
            Expr defaultResult = Expr.Unary(expr, operation);
            double? constant = GetConstValue(defaultResult);
            if(constant != null) {
                return Expr.Constant(constant.Value);
            }
            return defaultResult;
        }
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return ConstantConvolution(left, right, operation)
                ?? EqualityConvolution(left, right, operation) 
                ?? Expr.Binary(left, right, operation);
        }
        Expr ConstantConvolution(Expr left, Expr right, BinaryOperation operation) {
            double? leftConst = GetConstValue(left);

            if(leftConst == 0) {
                if(operation == BinaryOperation.Add)
                    return right;
                if(operation == BinaryOperation.Multiply || operation == BinaryOperation.Power)
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
                if(operation == BinaryOperation.Add)
                    return left;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Zero;
                if(operation == BinaryOperation.Power)
                    return Expr.One;
            }
            if(rightConst == 1) {
                if(operation == BinaryOperation.Multiply)
                    return left;
                if(operation == BinaryOperation.Power)
                    return left;
            }

            if(rightConst != null && leftConst != null)
                return Expr.Constant(ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst.Value, rightConst.Value));
            return null;
        }
        Expr EqualityConvolution(Expr left, Expr right, BinaryOperation operation) {
            UnaryExpressionInfo leftInfo = UnaryExpressionExtractor.ExtractUnaryInfo(left, operation);
            UnaryExpressionInfo rightInfo = UnaryExpressionExtractor.ExtractUnaryInfo(right, operation);
            if(rightInfo.Expr.ExprEquals(leftInfo.Expr)) {
                int coeff = (ExpressionEvaluator.IsInvertedOperation(leftInfo.Operation) ? -1 : 1) +
                    (ExpressionEvaluator.IsInvertedOperation(rightInfo.Operation) ? -1 : 1);
                if(operation == BinaryOperation.Add)
                    return Multiply(Expr.Constant(coeff), rightInfo.Expr);
                if(operation == BinaryOperation.Multiply)
                    return Power(left, Expr.Constant(coeff));
            }
            return null;
        }
        static double? GetConstValue(Expr expr) {
            UnaryExpr unary = expr as UnaryExpr;
            if((unary != null && unary.Expr is ConstantExpr) || expr is ConstantExpr) {
                return expr.Evaluate(new Context());
            }
            return null;
        }
    }
}
