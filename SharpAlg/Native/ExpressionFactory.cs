using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpAlg.Native {
    public static class ExpressionFactory {
        public static Expression Add(Expression left, Expression right) {
            double? leftConst = GetConstValue(left);
            if(leftConst == 0)
                return right;
            double? rightConst = GetConstValue(right);
            if(rightConst == 0)
                return left;
            if(rightConst.HasValue && leftConst.HasValue)
                return Expression.Constant(rightConst.Value + leftConst.Value);
            return Expression.Add(left, right);
        }
        public static Expression Mult(Expression left, Expression right) {
            double? leftConst = GetConstValue(left);
            double? rightConst = GetConstValue(right);
            if(leftConst == 0 || rightConst == 0)
                return Expression.Constant(0d);
            if(leftConst == 1)
                return right;
            if(rightConst == 1)
                return left;
            if(rightConst.HasValue && leftConst.HasValue)
                return Expression.Constant(rightConst.Value * leftConst.Value);
            return Expression.Multiply(left, right);
        }
        static double? GetConstValue(Expression expr) {
            var constant = expr as ConstantExpression;
            return constant != null ? (double)constant.Value : (double?)null;
        }
    }
}
