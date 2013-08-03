using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native.Builder {
    [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
    public class ConvolutionExprBuilder : ExprBuilder {
        #region inner classes
        [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
        class ExpressionArgumentsExtractor : DefaultExpressionVisitor<IEnumerable<Expr>> {
            public static IEnumerable<Expr> ExtractArguments(Expr expr, BinaryOperation operation) {
                return expr.Visit(new ExpressionArgumentsExtractor(operation)); //TODO singleton
            }
            readonly BinaryOperation operation;
            ExpressionArgumentsExtractor(BinaryOperation operation) { this.operation = operation; }
            public override IEnumerable<Expr> Add(AddExpr multi) {
                if(operation == BinaryOperation.Add)
                    return multi.Args;
                return base.Add(multi);
            }
            public override IEnumerable<Expr> Multiply(MultiplyExpr multi) {
                if(operation == BinaryOperation.Multiply)
                    return multi.Args;
                return base.Multiply(multi);
            }
            protected override IEnumerable<Expr> GetDefault(Expr expr) {
                return new Expr[] { expr };
            }
        }
        [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
        class MultiplyExpressionExtractor : DefaultExpressionVisitor<Tuple<Expr, Expr>> {
            public static Tuple<Expr, Expr> ExtractMultiply(Expr expr) {
                return expr.Visit(new MultiplyExpressionExtractor()); //TODO singleton
            }
            MultiplyExpressionExtractor() { }
            public override Tuple<Expr, Expr> Multiply(MultiplyExpr multi) {
                if(multi.Args.First() is ConstantExpr)
                    return new Tuple<Expr, Expr>(multi.Args.First(), Expr.Multiply(multi.Args.Skip(1)));
                return base.Multiply(multi);
            }
            protected override Tuple<Expr, Expr> GetDefault(Expr expr) {
                return new Tuple<Expr, Expr>(Expr.One, expr);
            }
        }
        [JsType(JsMode.Prototype, Filename = SR.JSBuilderName)]
        class PowerExpressionExtractor : DefaultExpressionVisitor<PowerExpr> {
            public static PowerExpr ExtractPower(Expr expr) {
                return expr.Visit(new PowerExpressionExtractor()); //TODO singleton
            }
            PowerExpressionExtractor() { }
            public override PowerExpr Power(PowerExpr power) {
                return power;
            }
            protected override PowerExpr GetDefault(Expr expr) {
                return Expr.Power(expr, Expr.One);
            }
        }
        #endregion

        public override Expr Add(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Add);
        }
        public override Expr Multiply(Expr left, Expr right) {
            return Binary(left, right, BinaryOperation.Multiply);
        }
        public override Expr Power(Expr left, Expr right) {
            return ConstantPowerConvolution(left, right)
                ?? ExpressionPowerConvolution(left, right)
                ?? Expr.Power(left, right);
        }
        Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return MultiConvolution(left, right, operation)
                ?? Expr.Binary(left, right, operation);
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
            return ExpressionArgumentsExtractor.ExtractArguments(expr, operation);
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
                    return Expr.Multiply(leftMultiplyExpr.Args.Select(x => Power(x, Expr.Constant(rightConst))));
                }
                var power = PowerExpressionExtractor.ExtractPower(left);
                Number leftConst = GetConstValue(power.Right);
                if(leftConst != null)
                    return Expr.Power(power.Left, Expr.Constant(rightConst * leftConst));
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
            return (expr as ConstantExpr).With(x => x.Value);
        }
    }
}