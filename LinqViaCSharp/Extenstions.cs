using System;

namespace LinqViaCSharp
{
    static class Extenstions
    {
        public static Func<T, TResult2> After<T, TResult1, TResult2>(this Func<TResult1, TResult2> function2, Func<T, TResult1> function1)
        {
            return value => function2(function1(value));
        }

        public static Func<T, TResult2> Compose<T, TResult1, TResult2>(this Func<TResult1, TResult2> function2, Func<T, TResult1> function1)
            => value => function2(function1(value));

        public static Func<T, TResult2> Then<T, TResult1, TResult2>( // Before.
            this Func<T, TResult1> function1, Func<TResult1, TResult2> function2) => value => function2(function1(value));

        internal static TResult Select<TSource, TResult>(this TSource value, Func<TSource, TResult> selector) => selector(value);
    }
}
