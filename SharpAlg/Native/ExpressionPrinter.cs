using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionPrinter : IExpressionVisitor<string> {
        public ExpressionPrinter() {
        }
        public string Constant(ConstantExpr constant) {
            return constant.Value.ToString();
        }
        public string Binary(BinaryExpr binary) {
            UnaryExpressionInfo info = UnaryExpressionExtractor.ExtractUnaryInfo(binary.Right, binary.Operation);
            return string.Format("({0} {1} {2})", binary.Left.Visit(this), GetBinaryOperationSymbol(info.Operation), info.Expr.Visit(this)); //TODO singleton
        }
        public string Unary(UnaryExpr unary) {
            return String.Format("({0}{1})", GetUnaryOperationSymbol(unary.Operation), unary.Expr.Visit(this));
        }
        public string Parameter(ParameterExpr parameter) {
            return parameter.ParameterName;
        }

        static string GetUnaryOperationSymbol(UnaryOperation operation) {
            switch(operation) {
                case UnaryOperation.Minus:
                    return "-";
                case UnaryOperation.Inverse:
                    return "1 / ";
                default:
                    throw new NotImplementedException();
            }
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
                case BinaryOperationEx.Power:
                    return "^";
                default:
                    throw new NotImplementedException();
            }
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
            return constant.Value >= 0 ? 
                GetDefaultInfo(constant) : 
                new UnaryExpressionInfo(Expr.Constant(-constant.Value), BinaryOperationEx.Subtract);
        }
        public UnaryExpressionInfo Parameter(ParameterExpr parameter) {
            return GetDefaultInfo(parameter);
        }
        public UnaryExpressionInfo Binary(BinaryExpr binary) {
            return GetDefaultInfo(binary);
        }
        public UnaryExpressionInfo Unary(UnaryExpr unary) {
            if(operation == BinaryOperation.Add && unary.Operation == UnaryOperation.Minus) {
                return new UnaryExpressionInfo(unary.Expr, BinaryOperationEx.Subtract);
            }
            if(operation == BinaryOperation.Multiply && unary.Operation == UnaryOperation.Inverse) {
                return new UnaryExpressionInfo(unary.Expr, BinaryOperationEx.Divide);
            }
            return GetDefaultInfo(unary);
        }
        UnaryExpressionInfo GetDefaultInfo(Expr expr) {
            return new UnaryExpressionInfo(expr, ExpressionEvaluator.GetBinaryOperationEx(operation));
        }
    }
}
