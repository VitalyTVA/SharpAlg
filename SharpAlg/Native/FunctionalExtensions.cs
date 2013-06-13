using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpAlg.Native {
    public static class FunctionalExtensions {
        public static IEnumerable<TOut> Map<TIn, TOut>(this Func<TIn, TOut> funcion, IEnumerable<TIn> input) {
            return input.Select(x => funcion(x));
        }
        public static IEnumerable<TOut> Map<TIn, TOut>(this Func<TIn, TOut> funcion, params TIn[] input) {
            return funcion.Map((IEnumerable<TIn>)input);
        }

        public static bool Equal<T>(this IEnumerable<T> first, IEnumerable<T> second) {
            return Enumerable.SequenceEqual(first, second);
        }
        public static bool Equal<T>(this IEnumerable<T> first, params T[] second) {
            return Equal(first, (IEnumerable<T>)second);
        }
    }
}
