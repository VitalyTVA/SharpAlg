using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class ExprBuilder {
        public abstract Expr Binary(Expr left, Expr right, BinaryOperation operation);
        //public abstract Expr Unary(Expr expr, UnaryOperation operation);
        public abstract Expr Power(Expr left, Expr right);
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
        public Expr Minus(Expr expr) {
            return Multiply(Expr.MinusOne, expr);
        }
        public Expr Inverse(Expr expr) {
            return Power(expr, Expr.MinusOne);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class TrivialExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return Expr.Binary(left, right, operation);
        }
        public override Expr Power(Expr left, Expr right) {
            return Expr.Power(left, right);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ConvolutionExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return EqualityConvolution(left, right, operation)
                ?? MultiConvolution(left, right, operation)
                ?? Expr.Binary(left, right, operation);
        }
        public override Expr Power(Expr left, Expr right) {
            return ConstantPowerConvolution(left, right) 
                ?? Expr.Power(left, right);
        }
        Expr MultiConvolution(Expr left, Expr right, BinaryOperation operation) {
            var args = GetArgs(left, operation).Concat(GetArgs(right, operation)).ToList();
            for(int i = 0; i < args.Count; i++) {
                for(int j = i + 1; j < args.Count; j++) {
                    var convoluted = ConstantConvolution(args[i], args[j], operation) ?? EqualityConvolution(args[i], args[j], operation);
                    if(convoluted != null) {
                        args[i] = convoluted;
                        args.RemoveAt(j);
                        j--;
                    }
                }
            }
            return args.Count > 1 ? Expr.Multi(args, operation) : args[0];
        }
        static IEnumerable<Expr> GetArgs(Expr expr, BinaryOperation operation) {
            if(expr is MultiExpr && ((MultiExpr)expr).Operation == operation)
                return ((MultiExpr)expr).Args;
            return new[] { expr };
        }
        Expr ConstantConvolution(Expr left, Expr right, BinaryOperation operation) {
            double? leftConst = GetConstValue(left);

            if(leftConst == 0) {
                if(operation == BinaryOperation.Add)
                    return right;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Zero;
            }
            if(leftConst == 1) {
                if(operation == BinaryOperation.Multiply)
                    return right;
            }

            double? rightConst = GetConstValue(right);

            if(rightConst == 0) {
                if(operation == BinaryOperation.Add)
                    return left;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Zero;
            }
            if(rightConst == 1) {
                if(operation == BinaryOperation.Multiply)
                    return left;
            }

            if(rightConst != null && leftConst != null)
                return Expr.Constant(ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst.Value, rightConst.Value));
            return null;
        }
        Expr ConstantPowerConvolution(Expr left, Expr right) {
            double? leftConst = GetConstValue(left);

            if(leftConst == 0) {
                    return Expr.Zero;
            }
            if(leftConst == 1) {
                return Expr.One;
            }

            double? rightConst = GetConstValue(right);

            if(rightConst == 0) {
                return Expr.One;
            }
            if(rightConst == 1) {
                return left;
            }

            if(rightConst != null && leftConst != null)
                return Expr.Constant(Math.Pow(leftConst.Value, rightConst.Value));
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
            if(CanEvaluate(expr)) {
                return expr.Evaluate(new Context());
            }
            return null;
        }
        static bool CanEvaluate(Expr expr) {
            if(expr is ConstantExpr)
                return true;
            PowerExpr power = expr as PowerExpr;
            if(power != null && power.Left is ConstantExpr && UnaryExpressionExtractor.IsInverseExpression(power))
                return true;
            MultiExpr multi = expr as MultiExpr;
            if(multi != null && UnaryExpressionExtractor.IsMinusExpression(multi) && multi.Args.ElementAt(1) is ConstantExpr)
                return true;
            return false;
        }
    }
}
