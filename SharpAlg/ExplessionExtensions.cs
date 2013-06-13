using SharpAlg.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpAlg {
    public static class ExplessionExtensions {
        public static Expression<Func<T, T>> Diff<T>(this Expression<Func<T, T>> expression) {
            return new DiffExpressionTreeVisitor<T>().Visit(expression);
        }
    }
}
