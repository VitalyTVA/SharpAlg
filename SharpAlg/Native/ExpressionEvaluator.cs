using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ExpressionEvaluator : IExpressionVisitor<Number> {
        readonly Context context;
        public ExpressionEvaluator(Context context) {
            this.context = context;
        }
        public Number Constant(ConstantExpr constant) {
            return constant.Value;
        }
        public Number Add(AddExpr multi) {
            return EvaluateMulti(multi, (x1, x2) => x1 + x2);
        }
        public Number Multiply(MultiplyExpr multi) {
            return EvaluateMulti(multi, (x1, x2) => x1 * x2);
        }
        Number EvaluateMulti(MultiExpr multi, Func<Number, Number, Number> evaluator) {
            Number result = Number.Zero;
            multi.Args.Accumulate(x => {
                result = x.Visit(this);
            }, x => {
                result = evaluator(result, x.Visit(this));
            });
            return result;
        }
        public Number Power(PowerExpr power) {
            return power.Left.Visit(this) ^ power.Right.Visit(this);
        }
        public Number Parameter(ParameterExpr parameter) {
            var parameterValue = context.GetValue(parameter.ParameterName);
            if(parameterValue == null)
                throw new ExpressionEvaluationException(string.Format("{0} value is undefined", parameter.ParameterName));
            return parameterValue.Visit(this);
        }
        public static Func<Number, Number, Number> GetBinaryOperationEvaluator(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return (x1, x2) => x1 + x2;
                case BinaryOperation.Multiply:
                    return (x1, x2) => x1 * x2;
                default:
                    throw new NotImplementedException();
            }
        }
        public static BinaryOperationEx GetBinaryOperationEx(BinaryOperation operation) {
            switch(operation) {
                case BinaryOperation.Add:
                    return BinaryOperationEx.Add;
                case BinaryOperation.Multiply:
                    return BinaryOperationEx.Multiply;
                default:
                    throw new NotImplementedException();
            }
        }
        public static bool IsInvertedOperation(BinaryOperationEx operation) {
            switch(operation) {
                case BinaryOperationEx.Subtract:
                case BinaryOperationEx.Divide:
                    return true;
            }
            return false;
        }
        public Number Function(FunctionExpr functionExpr) {
            var func = context.GetFunction(functionExpr.FunctionName);
            if(func != null) {
                return func.Evaluate(functionExpr.Args.Select(x => x.Visit(this)));
            }
            throw new NotImplementedException(); //TODO correct exception
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionEvaluationException : Exception {
        public ExpressionEvaluationException()
            : base() {
        }
        public ExpressionEvaluationException(string message)
            : base(message) {
        }
    }
}
