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
        public Expr Multi(MultiExpr multi) {
            switch(multi.Operation) {
                case BinaryOperation.Add:
                    return VisitAdditive(multi);
                case BinaryOperation.Multiply:
                    return VisitMultiply(multi);
                default:
                    throw new NotImplementedException();
            }
        }
        public Expr Power(PowerExpr power) {
            if(!(power.Right is ConstantExpr))
                throw new NotImplementedException(); //TODO when ln() is ready
            return Expr.Multiply(power.Right, builder.Multiply(power.Left.Visit(this), builder.Power(power.Left, builder.Subtract(power.Right, Expr.One)))); //TODO convolution when ln() is ready
        }
        Expr VisitAdditive(MultiExpr multi) {
            Expr result = null;
            multi.Args.Accumulate(x => {
                result = x.Visit(this);
            }, x => {
                result = builder.Add(result, x.Visit(this));
            });
            return result;
        }
        Expr VisitMultiply(MultiExpr expr) {
            var tail = expr.Tail();
            var expr1 = builder.Multiply(expr.Args.First().Visit(this), tail);
            var expr2 = builder.Multiply(expr.Args.First(), tail.Visit(this));
            return builder.Add(expr1, expr2);
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
