using SharpAlg.Native.Builder;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class DiffExpressionVisitor : IExpressionVisitor<Expr> {
        readonly ExprBuilder builder;
        string parameterName;
        bool autoParameterName;
        private bool HasParameter { get { return !string.IsNullOrEmpty(parameterName); } }
        public DiffExpressionVisitor(ExprBuilder builder, string parameterName) {
            this.builder = builder;
            this.parameterName = parameterName;
            autoParameterName = !HasParameter;
        }
        public Expr Constant(ConstantExpr constant) {
            return Expr.Zero;
        }
        public Expr Parameter(ParameterExpr parameter) {
            if(!HasParameter) {
                parameterName = parameter.ParameterName;
                autoParameterName = true;
            }
            if(parameterName == parameter.ParameterName) {
                return Expr.One;
            } else {
                if(autoParameterName)
                    throw new ExpressionDefferentiationException("Expression contains more than one independent variable");
                return Expr.Zero;
            }
        }
        public Expr Add(AddExpr multi) {
            Expr result = null;
            multi.Args.Accumulate(x => {
                result = x.Visit(this);
            }, x => {
                result = builder.Add(result, x.Visit(this));
            });
            return result;
        }
        public Expr Multiply(MultiplyExpr multi) {
            var tail = multi.Tail();
            var expr1 = builder.Multiply(multi.Args.First().Visit(this), tail);
            var expr2 = builder.Multiply(multi.Args.First(), tail.Visit(this));
            return builder.Add(expr1, expr2);
        }
        public Expr Power(PowerExpr power) {
            Expr sum1 = builder.Multiply(power.Right.Visit(this), Expr.Ln(power.Left));
            Expr sum2 = builder.Divide(builder.Multiply(power.Right, power.Left.Visit(this)), power.Left);
            Expr sum = builder.Add(sum1, sum2);
            return builder.Multiply(power, sum);
        }
        public Expr Function(FunctionExpr functionExpr) {
            //TODO function differentiation
            throw new NotImplementedException(); //TODO factorial differentiation
        }
    }
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public class ExpressionDefferentiationException : Exception {
        public ExpressionDefferentiationException()
            : base() {
        }
        public ExpressionDefferentiationException(string message)
            : base(message) {
        }
    }

}
