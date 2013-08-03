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
            return MultiConvolution(left, right, operation)
                ?? Expr.Binary(left, right, operation);
        }
        public override Expr Power(Expr left, Expr right) {
            return ConstantPowerConvolution(left, right)
                ?? ExpressionPowerConvolution(left, right)
                ?? Expr.Power(left, right);
        }
        IEnumerable<Expr> GetOpenParensArgs(Expr left, Expr right, BinaryOperation operation) {
            if(operation == BinaryOperation.Multiply) {
                Number rightConst = GetConstValue(right);
                if(rightConst != null) {
                    Expr tmp = left;
                    left = right;
                    right = tmp;
                }

                Number leftConst = GetConstValue(left);
                AddExpr rightAddExpr = right as AddExpr;
                if(leftConst != null && rightAddExpr != null) {
                    return rightAddExpr.Args.Select(x => Multiply(Expr.Constant(leftConst), x));
                }
            }
            return null;
        }
        Expr MultiConvolution(Expr left, Expr right, BinaryOperation operation) {
            IEnumerable<Expr> openParensArgs = GetOpenParensArgs(left, right, operation);
            if(openParensArgs != null) {
                operation = BinaryOperation.Add;
            }
            var args = openParensArgs ?? GetSortedArgs(left, right, operation);
            return MultiConvolutionCore(args, operation);
        }
        Expr MultiConvolutionCore(IEnumerable<Expr> args, BinaryOperation operation) {
            List<Expr> argsList = args.ToList();
            for(int i = 0; i < argsList.Count; i++) {
                bool convolutionOccured = false;
                for(int j = i + 1; j < argsList.Count; j++) {
                    var convoluted = ConstantConvolution(argsList[i], argsList[j], operation)
                        ?? PowerConvolution(argsList[i], argsList[j], operation)
                        ?? MultiplyConvolution(argsList[i], argsList[j], operation);
                    if(convoluted != null) {
                        argsList[i] = convoluted;
                        argsList.RemoveAt(j);
                        j--;
                        convolutionOccured = true;
                    }
                }
                if(convolutionOccured)
                    i--;
            }
            return Expr.Multi(argsList, operation);
        }
        IEnumerable<Expr> GetSortedArgs(Expr left, Expr right, BinaryOperation operation) {
            IEnumerable<Expr> args = GetArgs(left, operation).Concat(GetArgs(right, operation));
            if(operation == BinaryOperation.Multiply) {
                args = args.Where(x => x is ConstantExpr).Concat(args.Where(x => !(x is ConstantExpr)));
            }
            return args;
        }
        IEnumerable<Expr> GetArgs(Expr expr, BinaryOperation operation) {
            var multiExpr = expr as MultiExpr;
            if(multiExpr != null) {
                if(multiExpr.Operation == operation)
                    return ((MultiExpr)expr).Args;
            }
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
                var leftMultiplyExpr = left as MultiplyExpr;
                if(leftMultiplyExpr != null) {
                    return Expr.Multi(leftMultiplyExpr.Args.Select(x => Power(x, Expr.Constant(rightConst))), BinaryOperation.Multiply);
                }
                var power = PowerExpressionExtractor.ExtractPower(left);
                Number leftConst = GetConstValue(power.Right);
                if(leftConst != null)
                    return Expr.Power(power.Left, Expr.Constant(rightConst * leftConst));
            }
            return null;
        }
        static Number GetCoeff(UnaryExpressionInfo info) {
            return (ExpressionEvaluator.IsInvertedOperation(info.Operation) ? Number.MinusOne : Number.One);
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
