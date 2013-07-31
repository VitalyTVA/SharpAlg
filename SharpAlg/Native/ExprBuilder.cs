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
            return EqualityConvolution(left, right, operation) //TODO kill EqualityConvolution!!!!!!!!!!!!!
                ?? MultiConvolution(left, right, operation)
                ?? Expr.Binary(left, right, operation);
        }
        public override Expr Power(Expr left, Expr right) {
            return ConstantPowerConvolution(left, right)
                ?? ExpressionPowerConvolution(left, right)
                ?? Expr.Power(left, right);
        }
        Expr MultiConvolution(Expr left, Expr right, BinaryOperation operation) {
            var args = GetSortedArgs(left, right, operation);
            for(int i = 0; i < args.Count; i++) {
                for(int j = i + 1; j < args.Count; j++) {
                    var convoluted = ConstantConvolution(args[i], args[j], operation)
                        ?? PowerConvolution(args[i], args[j], operation)
                        ?? MultiplyConvolution(args[i], args[j], operation);
                    if(convoluted != null) {
                        args[i] = convoluted;
                        args.RemoveAt(j);
                        j--;
                    }
                }
            }
            return Expr.Multi(args, operation);
        }
        static List<Expr> GetSortedArgs(Expr left, Expr right, BinaryOperation operation) {
            IEnumerable<Expr> args = GetArgs(left, operation).Concat(GetArgs(right, operation));
            if(operation == BinaryOperation.Multiply) {
                args = args.Where(x => x is ConstantExpr).Concat(args.Where(x => !(x is ConstantExpr)));
            }
            return args.ToList();
        }
        static IEnumerable<Expr> GetArgs(Expr expr, BinaryOperation operation) {
            if(expr is MultiExpr && ((MultiExpr)expr).Operation == operation)
                return ((MultiExpr)expr).Args;
            return new[] { expr };
        }
        Expr ConstantConvolution(Expr left, Expr right, BinaryOperation operation) {
            Number leftConst = GetConstValue(left);

            if(leftConst == Number.Zero) {
                if(operation == BinaryOperation.Add)
                    return right;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Zero;
            }
            if(leftConst == Number.One) {
                if(operation == BinaryOperation.Multiply)
                    return right;
            }

            Number rightConst = GetConstValue(right);

            if(rightConst == Number.Zero) {
                if(operation == BinaryOperation.Add)
                    return left;
                if(operation == BinaryOperation.Multiply)
                    return Expr.Zero;
            }
            if(rightConst == Number.One) {
                if(operation == BinaryOperation.Multiply)
                    return left;
            }

            if(rightConst != null && leftConst != null) {
                return Expr.Constant(ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst, rightConst));
            }
            return null;
        }
        Expr ConstantPowerConvolution(Expr left, Expr right) {
            Number leftConst = GetConstValue(left);

            if(leftConst == Number.Zero) {
                    return Expr.Zero;
            }
            if(leftConst == Number.One) {
                return Expr.One;
            }

            Number rightConst = GetConstValue(right);

            if(rightConst == Number.Zero) {
                return Expr.One;
            }
            if(rightConst == Number.One) {
                return left;
            }

            if(rightConst != null && leftConst != null)
                return Expr.Constant(leftConst ^ rightConst);
            return null;
        }
        Expr ExpressionPowerConvolution(Expr left, Expr right) {
            Number rightConst = GetConstValue(right);
            if(rightConst != null) {
                var power = PowerExpressionExtractor.ExtractPower(left);
                Number leftConst = GetConstValue(power.Right);
                if(leftConst != null)
                    return Expr.Power(power.Left, Expr.Constant(rightConst * leftConst));
            }
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
        Expr PowerConvolution(Expr left, Expr right, BinaryOperation operation) {
            if(operation == BinaryOperation.Multiply) {
                PowerExpr leftPower = PowerExpressionExtractor.ExtractPower(left);
                PowerExpr rightPower = PowerExpressionExtractor.ExtractPower(right);
                if(leftPower.Left.ExprEquivalent(rightPower.Left)) {
                    return Power(leftPower.Left, Add(leftPower.Right, rightPower.Right));
                }
            }
            return null;
        }
        Expr MultiplyConvolution(Expr left, Expr right, BinaryOperation operation) {
            if(operation == BinaryOperation.Add) {
                Tuple<Expr, Expr> leftMultiply = MultiplyExpressionExtractor.ExtractMultiply(left);
                Tuple<Expr, Expr> rightMultiply = MultiplyExpressionExtractor.ExtractMultiply(right);
                if(leftMultiply.Item2.ExprEquivalent(rightMultiply.Item2)) {
                    return Multiply(Add(leftMultiply.Item1, rightMultiply.Item1), leftMultiply.Item2);
                }
            }
            return null;
        }
        static Number GetConstValue(Expr expr) {
            if(CanEvaluate(expr)) {
                return expr.Evaluate(new Context());
            }
            return null;
        }
        static bool CanEvaluate(Expr expr) {//TODO use evaluator itself instead??
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
