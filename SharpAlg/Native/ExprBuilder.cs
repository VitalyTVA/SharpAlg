using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public abstract class ExprBuilder {
        public abstract Expr Binary(Expr left, Expr right, BinaryOperation operation);
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class TrivialExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            return Expr.Binary(left, right, operation);
        }
    }
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public class ConvolutionExprBuilder : ExprBuilder {
        public override Expr Binary(Expr left, Expr right, BinaryOperation operation) {
            double? leftConst = GetConstValue(left);
            ////if(leftConst == 0)
            ////    return right;
            double? rightConst = GetConstValue(right);
            ////if(rightConst == 0)
            ////    return left;
            if(rightConst != null && leftConst != null)
                return Expr.Constant(rightConst.Value + leftConst.Value);
            return Expr.Binary(left, right, operation);
        }
        //public static Expression Mult(Expression left, Expression right) {
        //    double? leftConst = GetConstValue(left);
        //    double? rightConst = GetConstValue(right);
        //    if(leftConst == 0 || rightConst == 0)
        //        return Expression.Constant(0d);
        //    if(leftConst == 1)
        //        return right;
        //    if(rightConst == 1)
        //        return left;
        //    if(rightConst.HasValue && leftConst.HasValue)
        //        return Expression.Constant(rightConst.Value * leftConst.Value);
        //    return Expression.Multiply(left, right);
        //}
        static double? GetConstValue(Expr expr) {
            var constant = expr as ConstantExpr;
            return constant != null ? (double)constant.Value : (double?)null;
        }
    }
}
