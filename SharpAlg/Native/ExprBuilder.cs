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
        public Expr Unary(Expr expr, UnaryOperation operation) {
            return new UnaryExpr(expr, operation);
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
        static Expr EqualityConvolution(Expr left, Expr right, BinaryOperation operation_) {
            UnaryExpressionInfo info = UnaryExpressionExtractor.ExtractUnaryInfo(right, operation_);
            right = info.Expr;
            BinaryOperationEx operation = info.Operation;

            if(left.ExprEquals(right)) {
                if(operation == BinaryOperationEx.Add)
                    return Expr.Multiply(Expr.Constant(2), left);
                if(operation == BinaryOperationEx.Multiply)
                    return Expr.Power(left, Expr.Constant(2));
                if(operation == BinaryOperationEx.Subtract)
                    return Expr.Zero;
                if(operation == BinaryOperationEx.Divide)
                    return Expr.One;
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
