using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpAlg.Native.Printer {
    [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
    public class ExpressionPrinter : IExpressionVisitor<string> {
        public ExpressionPrinter() {
        }
        public string Constant(ConstantExpr constant) {
            return constant.Value.ToString();
        }
        public string Add(AddExpr multi) {
            return AddCore(multi.Args);
        }
        public string Multiply(MultiplyExpr multi) {
            return MultiplyCore(multi.Args);
        }
        string AddCore(IEnumerable<Expr> args) {
            var sb = new StringBuilder();
            args.Accumulate(x => {
                sb.Append(x.Visit(this));
            }, x => {
                UnaryExpressionInfo info;
                MultiplyExpr multiExpr = x as MultiplyExpr;
                if(multiExpr != null && multiExpr.Args.First().ExprEquals(Expr.MinusOne)) {
                    info = new UnaryExpressionInfo(Expr.Multiply(multiExpr.Args.Skip(1)), BinaryOperationEx.Subtract);
                } else if(multiExpr != null && multiExpr.Args.First().With(y => y as ConstantExpr).Return(y => y.Value < Number.Zero, () => false)) {
                    ConstantExpr exprConstant = Expr.Constant(Number.Zero - multiExpr.Args.First().With(y => y as ConstantExpr).Value);
                    Expr expr = Expr.Multiply((new Expr[] { exprConstant }).Concat(multiExpr.Args.Skip(1)));
                    info = new UnaryExpressionInfo(expr, BinaryOperationEx.Subtract);
                } else {
                    info = UnaryExpressionExtractor.ExtractUnaryInfo(x, BinaryOperation.Add);
                }

                sb.Append(GetBinaryOperationSymbol(info.Operation));
                sb.Append(WrapFromAdd(info.Expr));
            });
            return sb.ToString();
        }
        string MultiplyCore(IEnumerable<Expr> args) {
            if(args.First().ExprEquals(Expr.MinusOne)) {
                string exprText = WrapFromAdd(Expr.Multiply(args.Skip(1)));
                return String.Format("-{0}", exprText);
            }
            var sb = new StringBuilder();
            args.Accumulate(x => {
                sb.Append(WrapFromMultiply(x, ExpressionOrder.Head));
            }, x => {
                UnaryExpressionInfo info = UnaryExpressionExtractor.ExtractUnaryInfo(x, BinaryOperation.Multiply);
                sb.Append(GetBinaryOperationSymbol(info.Operation));
                sb.Append(WrapFromMultiply(info.Expr, ExpressionOrder.Default));
            });
            return sb.ToString();
        }
        public string Power(PowerExpr power) {
            if(UnaryExpressionExtractor.IsInverseExpression(power)) {
                return String.Format("1 / {0}", WrapFromMultiply(power.Left, ExpressionOrder.Default));
            }
            return string.Format("{0} ^ {1}", WrapFromPower(power.Left), WrapFromPower(power.Right));
        }
        public string Parameter(ParameterExpr parameter) {
            return parameter.ParameterName;
        }
        static string GetBinaryOperationSymbol(BinaryOperationEx operation) {
            switch(operation) {
                case BinaryOperationEx.Add:
                    return " + ";
                case BinaryOperationEx.Subtract:
                    return " - ";
                case BinaryOperationEx.Multiply:
                    return " * ";
                case BinaryOperationEx.Divide:
                    return " / ";
                default:
                    throw new NotImplementedException();
            }
        }
        public static OperationPriority GetPriority(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return OperationPriority.Add;
                case BinaryOperation.Multiply:
                    return OperationPriority.Multiply;
                default:
                    throw new NotImplementedException();
            }
        }
        string WrapFromAdd(Expr expr) {
            return Wrap(expr, OperationPriority.Add, ExpressionOrder.Default);
        }
        string WrapFromMultiply(Expr expr, ExpressionOrder order) {
            return Wrap(expr, OperationPriority.Multiply, order);
        }
        string WrapFromPower(Expr expr) {
            return Wrap(expr, OperationPriority.Power, ExpressionOrder.Default);
        }
        string Wrap(Expr expr, OperationPriority currentPriority, ExpressionOrder order) {
            bool wrap = expr.Visit(new ExpressionWrapperVisitor(currentPriority, order));
            string s = expr.Visit(this);
            if(wrap)
                return "(" + s + ")";
            return s;
        }
    }
    public enum ExpressionOrder {
        Head, Default
    }

}