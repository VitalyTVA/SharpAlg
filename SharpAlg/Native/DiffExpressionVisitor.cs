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
        public Expr Binary(BinaryExpr binary) {
            switch(binary.Operation) {
                case BinaryOperation.Add:
                case BinaryOperation.Subtract:
                    return VisitAdditive(binary);
                case BinaryOperation.Multiply:
                    return VisitMultiply(binary);
                case BinaryOperation.Divide:
                    return VisitDivide(binary);
                case BinaryOperation.Power:
                    return VisitPower(binary);
                default:
                    throw new NotImplementedException();
            }
        }
        Expr VisitAdditive(BinaryExpr expr) {
            return builder.Binary(expr.Left.Visit(this), expr.Right.Visit(this), expr.Operation);
        }
        Expr VisitMultiply(BinaryExpr expr) {
            var expr1 = builder.Multiply(expr.Left.Visit(this), expr.Right);
            var expr2 = builder.Multiply(expr.Left, expr.Right.Visit(this));
            return builder.Add(expr1, expr2);
        }
        Expr VisitDivide(BinaryExpr expr) {
            var expr1 = builder.Multiply(expr.Left.Visit(this), expr.Right);
            var expr2 = builder.Multiply(expr.Left, expr.Right.Visit(this));
            var expr3 = Expr.Multiply(expr.Right, expr.Right);//TODO convolution
            return Expr.Divide(builder.Subtract(expr1, expr2), expr3);//TODO convolution
        }
        Expr VisitPower(BinaryExpr binary) {
            if(!(binary.Right is ConstantExpr))
                throw new NotImplementedException(); //TODO when ln() is ready
            return Expr.Multiply(binary.Right, builder.Multiply(binary.Left.Visit(this), builder.Power(binary.Left, builder.Subtract(binary.Right, Expr.One)))); //TODO convolution when ln() is ready
        }
    }
}
