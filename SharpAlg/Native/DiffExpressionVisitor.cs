using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class DiffExpressionVisitor : IExpressionVisitor<Expr> {
        readonly ExprBuilder builder;
        public DiffExpressionVisitor(ExprBuilder builder) {
            this.builder = builder;
        }
        public Expr Constant(ConstantExpr constant) {
            return Expr.Zero;
        }
        public Expr Parameter(ParameterExpr parameter) {
            return Expr.One;
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
        //public Expr Unary(UnaryExpr unary) {
        //    switch(unary.Operation) {
        //        case UnaryOperation.Minus:
        //            return Expr.Unary(unary.Expr.Visit(this), unary.Operation);
        //        case UnaryOperation.Inverse:
        //            return Expr.Divide(builder.Unary(unary.Expr.Visit(this), UnaryOperation.Minus), builder.Multiply(unary.Expr, unary.Expr));
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
        Expr VisitAdditive(MultiExpr multi) {
            Expr result = null;
            multi.Accumulate(x => {
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
        //Expr VisitDivide(BinaryExpr expr) {
        //    var expr1 = builder.Multiply(expr.Left.Visit(this), expr.Right);
        //    var expr2 = builder.Multiply(expr.Left, expr.Right.Visit(this));
        //    var expr3 = Expr.Multiply(expr.Right, expr.Right);//TODO convolution
        //    return Expr.Divide(builder.Subtract(expr1, expr2), expr3);//TODO convolution
        //}
    }
}
