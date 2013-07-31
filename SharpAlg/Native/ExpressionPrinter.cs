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
        readonly OperationPriority priority;
        public ExpressionPrinter(OperationPriority priority = OperationPriority.None) {
            this.priority = priority;
        }
        public string Constant(ConstantExpr constant) {
            string stringValue = constant.Value.ToString();
            return constant.Value >= Number.Zero ? stringValue : Wrap(stringValue, OperationPriority.Add);
        }
        public string Multi(MultiExpr multi) {
            OperationPriority newPriority = GetPriority(multi.Operation);
            var nextPrinter = new ExpressionPrinter(newPriority);
            if(UnaryExpressionExtractor.IsMinusExpression(multi)) {
                return Wrap(String.Format("-{0}", multi.Args.ElementAt(1).Visit(nextPrinter)), OperationPriority.Add);
            }
            var sb = new StringBuilder();
            multi.Accumulate(x => {
                sb.Append(x.Visit(nextPrinter));
            }, x => {
                UnaryExpressionInfo info = UnaryExpressionExtractor.ExtractUnaryInfo(x, multi.Operation);
                sb.Append(" ");
                sb.Append(GetBinaryOperationSymbol(info.Operation));
                sb.Append(" ");
                sb.Append(info.Expr.Visit(nextPrinter));
            });
            return Wrap(sb.ToString(), GetPriority(multi.Operation));
        }
        public string Power(PowerExpr power) {
            var nextPrinter = new ExpressionPrinter(OperationPriority.Power);
            if(UnaryExpressionExtractor.IsInverseExpression(power)) {
                return Wrap(String.Format("1 / {0}", power.Left.Visit(nextPrinter)), OperationPriority.Multiply);
            }
            return Wrap(string.Format("{0} ^ {1}", power.Left.Visit(nextPrinter), power.Right.Visit(nextPrinter)), OperationPriority.Power);
        }
        public string Parameter(ParameterExpr parameter) {
            return parameter.ParameterName;
        }
        static string GetBinaryOperationSymbol(BinaryOperationEx operation) {
            switch(operation) {
                case BinaryOperationEx.Add:
                    return "+";
                case BinaryOperationEx.Subtract:
                    return "-";
                case BinaryOperationEx.Multiply:
                    return "*";
                case BinaryOperationEx.Divide:
                    return "/";
                default:
                    throw new NotImplementedException();
            }
        }
        static OperationPriority GetPriority(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return OperationPriority.Add;
                case BinaryOperation.Multiply:
                    return OperationPriority.Multiply;
                default:
                    throw new NotImplementedException();
            }
        }
        string Wrap(string s, OperationPriority newPriority) {
            if(newPriority <= priority)
                return "(" + s + ")";
            return s;
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
