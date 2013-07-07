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
            return string.Format("({0} {2} {1})", binary.Left.Visit(this), binary.Right.Visit(this), GetBinaryOperationSymbol(binary.Operation));
        }
        public string Parameter(ParameterExpr parameter) {
            return parameter.ParameterName;
        }
        static string GetBinaryOperationSymbol(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return "+";
                case BinaryOperation.Subtract:
                    return "-";
                case BinaryOperation.Multiply:
                    return "*";
                case BinaryOperation.Divide:
                    return "/";
                default:
                    throw new NotImplementedException();
            }

        }

    }
}
