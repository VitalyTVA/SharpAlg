using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
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
        public static bool SetEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer) {
            var list = second.ToList();
            foreach(var item in first) {
                bool found = false;
                foreach(var item2 in list) {
                    if(comparer(item, item2)) {
                        list.Remove(item2);
                        found = true;
                        break;
                    }
                }
                if(found == false)
                    return false;
            }
            return list.Count == 0;
        }
        public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> source, int index) {
            var en = source.GetEnumerator();
            while(en.MoveNext()) {
                if(index != 0)
                    yield return en.Current;
                index--;
            }
            if(index > 0)
                throw new IndexOutOfRangeException("index");
        }
        public static void Accumulate<T>(this IEnumerable<T> source, Action<T> init, Action<T> next) {
            var enumerator = source.GetEnumerator();
            if(enumerator.MoveNext())
                init(enumerator.Current);
            else
                throw new InvalidOperationException();
            while(enumerator.MoveNext()) {
                next(enumerator.Current);
            }
        }
        public static TAccumulate Accumulate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) { //TODO implement in jsclr
            var enumerator = source.GetEnumerator();
            while(enumerator.MoveNext()) {
                seed = func(seed, enumerator.Current);
            }
            return seed;
        }
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            var enumerator = source.GetEnumerator();
            while(enumerator.MoveNext()) {
                action(enumerator.Current);
            }
        }
        //public static bool Equal<T>(this IEnumerable<T> first, params T[] second) {
        //    return Equal(first, (IEnumerable<T>)second);
        //}
        public static IEnumerable<T> Tail<T>(this IEnumerable<T> source) {
            return source.Skip(1);
        }
    }
}