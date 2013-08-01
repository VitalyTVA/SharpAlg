using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionPrinter : IExpressionVisitor<string> {
        public ExpressionPrinter() {
        }
        public string Constant(ConstantExpr constant) {
            return constant.Value.ToString();
        }
        public string Multi(MultiExpr multi) {
            if(multi.Operation == BinaryOperation.Multiply)
                return Multiply(multi.Args);
            return Add(multi.Args);
        }
        string Add(IEnumerable<Expr> args) {
            var sb = new StringBuilder();
            args.Accumulate(x => {
                sb.Append(x.Visit(this));
            }, x => {
                UnaryExpressionInfo info;
                MultiExpr multiExpr = x as MultiExpr;
                if(multiExpr != null && multiExpr.Operation == BinaryOperation.Multiply && multiExpr.Args.First().ExprEquals(Expr.MinusOne)) {
                    info = new UnaryExpressionInfo(Expr.Multi(multiExpr.Args.Skip(1), BinaryOperation.Multiply), BinaryOperationEx.Subtract);
                } else if(multiExpr != null && multiExpr.Operation == BinaryOperation.Multiply && multiExpr.Args.First().With(y => y as ConstantExpr).Return(y => y.Value < Number.Zero, () => false)) {
                    ConstantExpr exprConstant = Expr.Constant(Number.Zero - multiExpr.Args.First().With(y => y as ConstantExpr).Value);
                    Expr expr = Expr.Multi((new Expr[] { exprConstant }).Concat(multiExpr.Args.Skip(1)), BinaryOperation.Multiply);
                    info = new UnaryExpressionInfo(expr, BinaryOperationEx.Subtract);
                } else {
                    info = UnaryExpressionExtractor.ExtractUnaryInfo(x, BinaryOperation.Add);
                }

                sb.Append(GetBinaryOperationSymbol(info.Operation));
                sb.Append(WrapFromAdd(info.Expr));
            });
            return sb.ToString();
        }
        string Multiply(IEnumerable<Expr> args) {
            if(args.First().ExprEquals(Expr.MinusOne)) {
                string exprText = WrapFromMultiply(Expr.Multi(args.Skip(1), BinaryOperation.Multiply), ExpressionOrder.Default);
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
                return String.Format("1 / {0}", power.Left.Visit(this));
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
            bool wrap = expr.Visit(new ExpressionWrapper(currentPriority, order));
            string s = expr.Visit(this);
            if(wrap)
                return "(" + s + ")";
            return s;
        }
    }
    public enum ExpressionOrder {
        Head, Default
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionWrapper : IExpressionVisitor<bool> {
        readonly ExpressionOrder order;
        readonly OperationPriority priority;
        public ExpressionWrapper(OperationPriority priority, ExpressionOrder order) {
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
        public bool Multi(MultiExpr multi) {
            if(priority == OperationPriority.Add)
                return UnaryExpressionExtractor.IsMinusExpression(multi);
            if(UnaryExpressionExtractor.IsMinusExpression(multi))
                return true;
            return priority > ExpressionPrinter.GetPriority(multi.Operation);
        }
        public bool Power(PowerExpr power) {
            if(UnaryExpressionExtractor.IsInverseExpression(power)) {
                return priority >= OperationPriority.Multiply;
            }
            return priority == OperationPriority.Power;
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class UnaryExpressionInfo {
        public UnaryExpressionInfo(Expr expr, BinaryOperationEx operation) {
            Operation = operation;
            Expr = expr;
        }
        public Expr Expr { get; private set; }
        public BinaryOperationEx Operation { get; private set; }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class UnaryExpressionExtractor : DefaultExpressionVisitor<UnaryExpressionInfo> {
        public static UnaryExpressionInfo ExtractUnaryInfo(Expr expr, BinaryOperation operation) {
            return expr.Visit(new UnaryExpressionExtractor(operation));
        }
        public static bool IsMinusExpression(MultiExpr multi) {
            return multi.Args.Count() == 2 && Expr.MinusOne.ExprEquals(multi.Args.ElementAt(0));
        }
        public static bool IsInverseExpression(PowerExpr power) {
            return Expr.MinusOne.ExprEquals(power.Right);
        }

        readonly BinaryOperation operation;
        UnaryExpressionExtractor(BinaryOperation operation) {
            this.operation = operation;
        }
        public override UnaryExpressionInfo Constant(ConstantExpr constant) {
            return constant.Value >= Number.Zero || operation != BinaryOperation.Add ?
                base.Constant(constant) :
                new UnaryExpressionInfo(Expr.Constant(Number.Zero - constant.Value), BinaryOperationEx.Subtract);
        }
        public override UnaryExpressionInfo Multi(MultiExpr multi) {
            if(operation == BinaryOperation.Add && IsMinusExpression(multi)) {
                return new UnaryExpressionInfo(multi.Args.ElementAt(1), BinaryOperationEx.Subtract);
            }
            return base.Multi(multi);
        }
        public override UnaryExpressionInfo Power(PowerExpr power) {
            if(operation == BinaryOperation.Multiply && IsInverseExpression(power)) {
                return new UnaryExpressionInfo(power.Left, BinaryOperationEx.Divide);
            }
            return base.Power(power);
        }
        protected override UnaryExpressionInfo GetDefault(Expr expr) {
            return new UnaryExpressionInfo(expr, ExpressionEvaluator.GetBinaryOperationEx(operation));
        }
    }

    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class PowerExpressionExtractor : DefaultExpressionVisitor<PowerExpr> {
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
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class MultiplyExpressionExtractor : DefaultExpressionVisitor<Tuple<Expr, Expr>> {
        public static Tuple<Expr, Expr> ExtractMultiply(Expr expr) {
            return expr.Visit(new MultiplyExpressionExtractor()); //TODO singleton
        }
        MultiplyExpressionExtractor() { }
        public override Tuple<Expr, Expr> Multi(MultiExpr multi) {
            if(multi.Operation == BinaryOperation.Multiply && multi.Args.First() is ConstantExpr)
                return new Tuple<Expr, Expr>(multi.Args.First(), Expr.Multi(multi.Args.Skip(1), BinaryOperation.Multiply));
            return base.Multi(multi);
        }
        protected override Tuple<Expr, Expr> GetDefault(Expr expr) {
            return new Tuple<Expr, Expr>(Expr.One, expr);
        }
    }

    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class DefaultExpressionVisitor<T> : IExpressionVisitor<T> {
        protected DefaultExpressionVisitor() { }
        public virtual T Constant(ConstantExpr constant) {
            return GetDefault(constant);
        }
        public virtual T Parameter(ParameterExpr parameter) {
            return GetDefault(parameter);
        }
        public virtual T Multi(MultiExpr multi) {
            return GetDefault(multi);
        }
        public virtual T Power(PowerExpr power) {
            return GetDefault(power);
        }
        protected abstract T GetDefault(Expr expr);
    }
}
