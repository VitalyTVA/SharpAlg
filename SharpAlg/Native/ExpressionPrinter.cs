using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
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
            return constant.Value >= 0 ? stringValue : Wrap(stringValue, OperationPriority.Add);
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
        //public string Unary(UnaryExpr unary) {
        //    var newPriority = GetPriority(unary.Operation);
        //    return Wrap(String.Format("{0}{1}", GetUnaryOperationSymbol(unary.Operation), unary.Expr.Visit(new ExpressionPrinter(newPriority))), newPriority);
        //}
        public string Parameter(ParameterExpr parameter) {
            return parameter.ParameterName;
        }

        //static string GetUnaryOperationSymbol(UnaryOperation operation) {
        //    switch(operation) {
        //        case UnaryOperation.Minus:
        //            return "-";
        //        case UnaryOperation.Inverse:
        //            return "1 / ";
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
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
        //static OperationPriority GetPriority(UnaryOperation operation) {
        //    switch(operation) {
        //        case UnaryOperation.Minus:
        //            return OperationPriority.Add;
        //        case UnaryOperation.Inverse:
        //            return OperationPriority.Multiply;
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
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
    public class UnaryExpressionExtractor : IExpressionVisitor<UnaryExpressionInfo> {
        public static UnaryExpressionInfo ExtractUnaryInfo(Expr expr, BinaryOperation operation) {
            return expr.Visit(new UnaryExpressionExtractor(operation));
        }
        readonly BinaryOperation operation;
        UnaryExpressionExtractor(BinaryOperation operation) {
            this.operation = operation;
        }
        public UnaryExpressionInfo Constant(ConstantExpr constant) {
            return constant.Value >= 0 || operation != BinaryOperation.Add ? 
                GetDefaultInfo(constant) : 
                new UnaryExpressionInfo(Expr.Constant(-constant.Value), BinaryOperationEx.Subtract);
        }
        public UnaryExpressionInfo Parameter(ParameterExpr parameter) {
            return GetDefaultInfo(parameter);
        }
        public UnaryExpressionInfo Multi(MultiExpr multi) {
            if(operation == BinaryOperation.Add && IsMinusExpression(multi)) {
                return new UnaryExpressionInfo(multi.Args.ElementAt(1), BinaryOperationEx.Subtract);
            }
            return GetDefaultInfo(multi);
        }
        public UnaryExpressionInfo Power(PowerExpr power) {
            if(operation == BinaryOperation.Multiply && IsInverseExpression(power)) {
                return new UnaryExpressionInfo(power.Left, BinaryOperationEx.Divide);
            }
            return GetDefaultInfo(power);
        }
        //public UnaryExpressionInfo Unary(UnaryExpr unary) {
        //    if(operation == BinaryOperation.Add && unary.Operation == UnaryOperation.Minus) {
        //        return new UnaryExpressionInfo(unary.Expr, BinaryOperationEx.Subtract);
        //    }
        //    if(operation == BinaryOperation.Multiply && unary.Operation == UnaryOperation.Inverse) {
        //        return new UnaryExpressionInfo(unary.Expr, BinaryOperationEx.Divide);
        //    }
        //    return GetDefaultInfo(unary);
        //}
        UnaryExpressionInfo GetDefaultInfo(Expr expr) {
            return new UnaryExpressionInfo(expr, ExpressionEvaluator.GetBinaryOperationEx(operation));
        }
        public static bool IsMinusExpression(MultiExpr multi) {
            return multi.Args.Count() == 2 && Expr.MinusOne.ExprEquals(multi.Args.ElementAt(0));
        }
        public static bool IsInverseExpression(PowerExpr power) {
            return Expr.MinusOne.ExprEquals(power.Right);
        }
    }
}
