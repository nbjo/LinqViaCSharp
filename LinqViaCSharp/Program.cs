using System;
using static LinqViaCSharp.ChurchBoolean;

namespace LinqViaCSharp
{
    public class Program
    {
        // Unit<T> is the alias of Func<T, T>.
        public delegate T Unit<T>(T value);

        public static class Functions<T>
        {
            public static readonly Unit<T> Id = x => x;
        }

        private static void CallAnd()
        {
            // Note that True and False has 2 parameters and can be curried
            var result1 = True.And(True);
            var x = True;
            var y = False;
            var result2 = x.And(y);
        }

        static void Main(string[] args)
        {
            Func<double, double> sqrt = Math.Sqrt;
            Func<double, double> abs = Math.Abs;
            Func<double, double> log = Math.Log;

            CallAnd();

            var absSqrt = sqrt.Compose(abs);
            Console.WriteLine("absSqrt = " + absSqrt(-2));

            var absSqrtLog1 = log.Compose(sqrt).Compose(abs);
            var absSqrtLog2 = log.Compose(sqrt.Compose(abs));

            Console.WriteLine("absSqrtLog1 = " + absSqrtLog1(-2));
            Console.WriteLine("absSqrtLog2 = " + absSqrtLog2(-2));



            Console.ReadKey();
        }
    }
}
