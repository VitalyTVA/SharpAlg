using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpAlg.Native {
    [JsType(JsMode.Prototype, Filename = SR.JSNativeName)]
    public static class FunctionalExtensions {
        const string STR_InputSequencesHaveDifferentLength = "Input sequences have different length.";
        //public static IEnumerable<TOut> Map<TIn, TOut>(this Func<TIn, TOut> function, IEnumerable<TIn> input) {
        //    return input.Select(x => function(x));
        //}
        //public static IEnumerable<TOut> Map<TIn, TOut>(this Func<TIn, TOut> function, params TIn[] input) {
        //    return function.Map((IEnumerable<TIn>)input);
        //}

        //public static IEnumerable<TOut> Map<TIn1, TIn2, TOut>(this Func<TIn1, TIn2, TOut> function, IEnumerable<TIn1> input1, IEnumerable<TIn2> input2) {
        //    var result = new List<TOut>();
        //    Map((x, y) => result.Add(function(x, y)), input1, input2);
        //    return result;
        //}
        public static void Map<TIn1, TIn2>(this Action<TIn1, TIn2> action, IEnumerable<TIn1> input1, IEnumerable<TIn2> input2) {
            var enumerator1 = input1.GetEnumerator();
            var enumerator2 = input2.GetEnumerator();
            while(enumerator1.MoveNext()) {
                if(!enumerator2.MoveNext())
                    throw new ArgumentException(SR.STR_InputSequencesHaveDifferentLength);
                action(enumerator1.Current, enumerator2.Current);
            }
            if(enumerator2.MoveNext())
                throw new ArgumentException(SR.STR_InputSequencesHaveDifferentLength);
        }

        public static bool EnumerableEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer) {
            var en1 = first.GetEnumerator();
            var en2 = second.GetEnumerator();
            while(en1.MoveNext()) {
                if(!en2.MoveNext())
                    return false;
                if(!comparer(en1.Current, en2.Current))
                    return false;
            }
            return !en2.MoveNext();
        }
        //public static bool Equal<T>(this IEnumerable<T> first, params T[] second) {
        //    return Equal(first, (IEnumerable<T>)second);
        //}
    }
}