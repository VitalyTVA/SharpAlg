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
        #region inner classes
        [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
        class ExpressionWrapperVisitor : IExpressionVisitor<bool> {
            readonly ExpressionOrder order;
            readonly OperationPriority priority;
            public ExpressionWrapperVisitor(OperationPriority priority, ExpressionOrder order) {
                this.order = order;
                this.priority = priority;
            }
            public bool Constant(ConstantExpr constant) {
                if(order == ExpressionOrder.Head)
                    return false;
                return constant.Value < Number.Zero;
            }
            public bool Parameter(ParameterExpr parameter) {
                return false;
            }
            public bool Add(AddExpr multi) {
                return ShouldWrap(OperationPriority.Add);
            }
            public bool Multiply(MultiplyExpr multi) {
                if(IsMinusExpression(multi))
                    return true;
                return ShouldWrap(OperationPriority.Multiply);
            }
            public bool Power(PowerExpr power) {
                if(IsInverseExpression(power))
                    return priority >= OperationPriority.Multiply;
                return ShouldWrap(OperationPriority.Power);
            }
            public bool Function(FunctionExpr functionExpr) {
                if(IsFactorial(functionExpr))
                    return ShouldWrap(OperationPriority.Factorial);
                return false;
            }
            bool ShouldWrap(OperationPriority exprPriority) {
                return priority >= exprPriority;
            }
        }
        [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
        abstract class UnaryExpressionExtractor : DefaultExpressionVisitor<UnaryExpressionInfo> {
            protected abstract BinaryOperation Operation { get; }
            protected UnaryExpressionExtractor() {
            }
            public override UnaryExpressionInfo Constant(ConstantExpr constant) {
                return constant.Value >= Number.Zero || Operation != BinaryOperation.Add ?
                    base.Constant(constant) :
                    new UnaryExpressionInfo(Expr.Constant(Number.Zero - constant.Value), BinaryOperationEx.Subtract);
            }
            protected override UnaryExpressionInfo GetDefault(Expr expr) {
                return new UnaryExpressionInfo(expr, ExpressionEvaluator.GetBinaryOperationEx(Operation));
            }
        }
        [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
        class MultiplyUnaryExpressionExtractor : UnaryExpressionExtractor {
            public static readonly MultiplyUnaryExpressionExtractor MultiplyInstance = new MultiplyUnaryExpressionExtractor();
            protected override BinaryOperation Operation { get { return BinaryOperation.Multiply; } }
            protected MultiplyUnaryExpressionExtractor() {
            }
            public override UnaryExpressionInfo Power(PowerExpr power) {
                if(IsInverseExpression(power)) {
                    return new UnaryExpressionInfo(power.Left, BinaryOperationEx.Divide);
                }
                return base.Power(power);
            }
        }
        [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
        class AddUnaryExpressionExtractor : UnaryExpressionExtractor {
            public static readonly AddUnaryExpressionExtractor AddInstance = new AddUnaryExpressionExtractor();
            public static UnaryExpressionInfo ExtractAddUnaryInfo(Expr expr) {
                return expr.Visit(new AddUnaryExpressionExtractor());
            }
            protected override BinaryOperation Operation { get { return BinaryOperation.Add; } }
            AddUnaryExpressionExtractor() {
            }
            public override UnaryExpressionInfo Multiply(MultiplyExpr multi) {
                ConstantExpr headConstant = multi.Args.First() as ConstantExpr;
                if(headConstant.Return(x => x.Value < Number.Zero, () => false)) {
                    ConstantExpr exprConstant = Expr.Constant(Number.Zero - multi.Args.First().With(y => y as ConstantExpr).Value);
                    Expr expr = multi.Args.First().ExprEquals(Expr.MinusOne) ? 
                        multi.Tail() : 
                        Expr.Multiply((new Expr[] { exprConstant }).Concat(multi.Args.Tail()));
                    return new UnaryExpressionInfo(expr, BinaryOperationEx.Subtract);
                }
                return base.Multiply(multi);
            }
        }
        [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
        class UnaryExpressionInfo {
            public UnaryExpressionInfo(Expr expr, BinaryOperationEx operation) {
                Operation = operation;
                Expr = expr;
            }
            public Expr Expr { get; private set; }
            public BinaryOperationEx Operation { get; private set; }
        }
        #endregion
        static bool IsMinusExpression(MultiplyExpr multi) {
            return multi.Args.Count() == 2 && Expr.MinusOne.ExprEquals(multi.Args.ElementAt(0));
        }
        static bool IsInverseExpression(PowerExpr power) {
            return Expr.MinusOne.ExprEquals(power.Right);
        }
        static bool IsFactorial(FunctionExpr functionExpr) {
            return functionExpr.FunctionName == Expr.STR_Factorial;
        }
        public static ExpressionPrinter Instance = new ExpressionPrinter();
        ExpressionPrinter() {
        }
        public string Constant(ConstantExpr constant) {
            return constant.Value.ToString();
        }
        public string Add(AddExpr multi) {
            var sb = new StringBuilder();
            multi.Args.Accumulate(x => {
                sb.Append(x.Visit(this));
            }, x => {
                UnaryExpressionInfo info = x.Visit(AddUnaryExpressionExtractor.AddInstance);
                sb.Append(GetBinaryOperationSymbol(info.Operation));
                sb.Append(WrapFromAdd(info.Expr));
            });
            return sb.ToString();
        }
        public string Multiply(MultiplyExpr multi) {
            if(multi.Args.First().ExprEquals(Expr.MinusOne)) {
                string exprText = WrapFromAdd(multi.Tail());
                return String.Format("-{0}", exprText);
            }
            var sb = new StringBuilder();
            multi.Args.Accumulate(x => {
                sb.Append(WrapFromMultiply(x, ExpressionOrder.Head));
            }, x => {
                UnaryExpressionInfo info = x.Visit(MultiplyUnaryExpressionExtractor.MultiplyInstance);
                sb.Append(GetBinaryOperationSymbol(info.Operation));
                sb.Append(WrapFromMultiply(info.Expr, ExpressionOrder.Default));
            });
            return sb.ToString();
        }
        public string Power(PowerExpr power) {
            if(IsInverseExpression(power)) {
                return String.Format("1 / {0}", WrapFromMultiply(power.Left, ExpressionOrder.Default));
            }
            return string.Format("{0} ^ {1}", WrapFromPower(power.Left), WrapFromPower(power.Right));
        }
        public string Parameter(ParameterExpr parameter) {
            return parameter.ParameterName;
        }
        public string Function(FunctionExpr functionExpr) {
            if(IsFactorial(functionExpr))
                return string.Format("{0}!", WrapFromFactorial(functionExpr.Args.First()));

            var sb = new StringBuilder(functionExpr.FunctionName);
            sb.Append("(");
            functionExpr.Args.Accumulate(x => {
                sb.Append(x.Visit(this));
            }, x => {
                sb.Append(", ");
                sb.Append(x.Visit(this));
            });
            sb.Append(")");
            return sb.ToString();
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
        string WrapFromFactorial(Expr expr) {
            return Wrap(expr, OperationPriority.Factorial, ExpressionOrder.Default);
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