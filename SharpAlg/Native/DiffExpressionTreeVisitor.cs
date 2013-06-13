using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpAlg.Native {
    public class DiffExpressionTreeVisitor<T> {
        public Expression<Func<T, T>> Visit(Expression<Func<T, T>> expression) {
            return (Expression<Func<T,T>>)VisitCore(expression);
        }
        Expression VisitCore(Expression expression) {
            switch(expression.NodeType) {
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)expression);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)expression);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)expression);
                case ExpressionType.Add:
                    return VisitSum((BinaryExpression)expression);
                default:
                    throw new NotImplementedException(expression.NodeType + " ExpressionType is not supported");//TODO test
            }
        }
        LambdaExpression VisitLambda(LambdaExpression expression) {
            //if(expression.Parameters.Count != 1)
            //    throw new NotImplementedException(); //TODO test
            return Expression.Lambda(VisitCore(expression.Body), expression.Parameters);
        }
        Expression VisitConstant(ConstantExpression expression) {
            return Expression.Constant(0d);    
        }
        Expression VisitParameter(ParameterExpression expression) {
            return Expression.Constant(1d);
        }
        Expression VisitSum(BinaryExpression expression) {
            return Expression.Add(VisitCore(expression.Left), VisitCore(expression.Right));
        }
    }
}
