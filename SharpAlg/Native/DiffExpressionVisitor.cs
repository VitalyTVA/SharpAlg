using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    public class DiffExpressionVisitor : IExpressionVisitor<Expr> {
        public Expr Constant(ConstantExpr constant) {
            return Expr.Constant(0);
        }
        public Expr Parameter(ParameterExpr parameter) {
            return Expr.Constant(1);
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
                default:
                    throw new NotImplementedException();
            }
        }
        Expr VisitAdditive(BinaryExpr expr) {
            return Expr.Binary(expr.Left.Visit(this), expr.Right.Visit(this), expr.Operation);
        }
        Expr VisitMultiply(BinaryExpr expr) {
            var expr1 = Expr.Binary(expr.Left.Visit(this), expr.Right, BinaryOperation.Multiply);
            var expr2 = Expr.Binary(expr.Left, expr.Right.Visit(this), BinaryOperation.Multiply);
            return Expr.Binary(expr1, expr2, BinaryOperation.Add);
        }
        Expr VisitDivide(BinaryExpr expr) {
            var expr1 = Expr.Binary(expr.Left.Visit(this), expr.Right, BinaryOperation.Multiply);
            var expr2 = Expr.Binary(expr.Left, expr.Right.Visit(this), BinaryOperation.Multiply);
            var expr3 = Expr.Binary(expr.Right, expr.Right, BinaryOperation.Multiply);
            return Expr.Binary(Expr.Binary(expr1, expr2, BinaryOperation.Subtract), expr3, BinaryOperation.Divide);
        }
    }
}
