using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static LinqViaCSharp.ChurchBoolean;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;

//using System.Reactive.Interfaces;

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

        internal static void QueryMethods()
        {
            IEnumerable<int> source = new[] { 4, 3, 2, 1, 0, -1 }; // Get source.
            var query = source.Where(int32 => int32 > 0)
                .OrderBy(int32 => int32)
                .Select(int32 => Math.Sqrt(int32)); // Create query.
            foreach (var result in query) // Execute query.
            {
                Trace.WriteLine(result);
            }
        }

        internal static void ComposeLinq()
        {
            //var filterSortMap = new Func<IEnumerable<int>, IEnumerable<int>>(source => source.Where(int32 => int32 > 0))
            //    .Then(filtered => filtered.OrderBy(int32 => int32))
            //    .Then(sorted => sorted.Select(int32 => Math.Sqrt(int32)));
            var filterSortMap = new Func<IEnumerable<int>, IEnumerable<double>>(source => source.Where(int32 => int32 > -2).OrderBy(int32 => int32).Select(int32 => Math.Sqrt(int32)));
            var query = filterSortMap(new[] { 4, 3, 2, 1, 0, -1, -2 });
            foreach (var result in query) // Execute query.
            {
                if (!double.IsNaN(result))
                    Console.WriteLine(result);
            }
        }

        public class FluentList<T> : List<T>
        {
            public new FluentList<T> Add(T item) { base.Add(item); return this; }
            public new FluentList<T> Clear() { base.Clear(); return this; }
            public new FluentList<T> ForEach(Action<T> action)
            {
                base.ForEach(action); return this;
            }
            public new FluentList<T> Insert(int index, T item)
            {
                base.Insert(index, item); return this;
            }
            public new FluentList<T> RemoveAt(int index)
            {
                base.RemoveAt(index);
                return this;
            }
            public new FluentList<T> Reverse() { base.Reverse(); return this; }
        }

        internal static void FluentListHandler()
        {
            var fluentlist = new FluentList<int> { 1, 2, 3, 4, 5 }
            .Add(1)
            .Insert(0, 0)
            .RemoveAt(1)
            .Reverse()
            .ForEach(Console.WriteLine)
            .Clear();
        }

        internal class Generic<T>
        {
            internal Generic(T value)
            {
            }
        }
        internal class Generic // Not Generic<T>.
        {
            internal static Generic<T> Create<T>(T value) => new Generic<T>(value);
            // T can be inferred.
        }
        internal static class Functions
        {
            internal static void CreateTask(string readPath, string writePath)
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId); // 10
                var task = new Task<string>(() =>
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId); // 8
                    return File.ReadAllText(readPath);
                });
                task.Start();
                var continuationTask = task.ContinueWith(antecedentTask =>
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId); // 9
                    Console.WriteLine(ReferenceEquals(antecedentTask, task));
                    if (antecedentTask.IsFaulted)
                    {
                        Console.WriteLine(antecedentTask.Exception);
                    }
                    else
                    {
                        File.WriteAllText(writePath, antecedentTask.Result);
                    }
                });
                continuationTask.Wait();
            }
        }


        private static readonly int[] Rows = { 1, 2, 3 };
        private static readonly string[] Columns = { "A", "B", "C", "D" };

        private static void CrossJoin()
        {
            //var cells = Rows.SelectMany(row => Columns, (row, column) => $"{column}{row}"); //Define query.
            var cells = Columns.SelectMany(row => Rows, (row, column) => $"{column}{row}"); //Define query.
            //var cells = from column in Columns
            //            from row in Rows
            //            select $"{column}{row}";
            var cellIndex = 0;
            var columnCount = Rows.Length;
            foreach (var cell in cells) // Execute query.
            {
                Console.Write($"{cell} ");
                if (cellIndex++ > 0 && cellIndex % columnCount == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        internal static partial class LinqToTwitter
        {
            internal static void QueryExpression()
            {
                var credentials = new SingleUserAuthorizer()
                {
                    CredentialStore = new InMemoryCredentialStore()
                    {
                        ConsumerKey = "YDgWzSxxBaLTT2eSWS6jLRQxd",
                        ConsumerSecret = "XYZGuT04E93J2OtlJOEt7stnrM78dtPSJlQlL81C1O4JKoGbLP",
                        OAuthToken = "881590855630508032-xeDEfO0prBkC6YGZKEkrzCSD0sOzbyW",
                        OAuthTokenSecret = "GsrpjXIlvfWPO3rMSI258AsDGKBHTgvVkkUxiVj1OAqFp"
                    }
                };
                using (var twitter = new TwitterContext(credentials))
                {
                    IQueryable<Search> source = twitter.Search; // Get source.
                    IQueryable<List<Status>> query = from search in source
                                                     where search.Type == SearchType.Search && search.Query == "LINQ"
                                                     orderby
                                                     search.SearchMetaData.Count
                                                     select search.Statuses; //Define query.
                    foreach (var search in query) // Execute query.
                    {
                        foreach (Status status in search)
                        {
                            Trace.WriteLine(status.Text);
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            //CrossJoin();
            LinqToTwitter.QueryExpression();

            //Functions.CreateTask(@"d:\temp\infile.txt", @"d:\temp\outfile.txt");
            //Tuple<string, decimal> tuple1 = new Tuple<string, decimal>("Surface Pro 4", 899M);
            //var generic = Generic.Create(new
            //{
            //    Name = "Surface Book",
            //    Price = 1349.00M
            //});
            //var generic2 = new
            //{
            //    Name = "Surface Book",
            //    Price = 1349.00M
            //};
            //var immutableDevice = new { Name = "Surface Book", Price = 1349.00M };
            ////var mapped1 = from zero in new[] { 0 } select zero; // 0
            //var mapped1 = from zero in 0 select zero; // 0
            //var mapped2 = from three in 1 + 2 select Math.Sqrt(three + 1);
            //var mapped3 = new[] { 0 }.Select(zero => zero); // 0
            //string mapped = from newGuid in Guid.NewGuid() select newGuid.ToString();
            //FluentListHandler();
            //ComposeLinq();
            //QueryMethods();

            //open a 4GB file for asynchronous reading in blocks of 64K
            //var inFile = new FileStream(@"d:\temp\4GBfile.txt",
            //            FileMode.Open, FileAccess.Read, FileShare.Read, 2 << 15, true);
            //open a file for asynchronous writing in blocks of 64K
            //var outFile = new FileStream(@"d:\temp\Encrypted.txt",
            //    FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 2 << 15, true);

            //inFile.AsyncRead(2 << 15).Select(Encrypt).WriteToStream(outFile).Subscribe(
            //    _ => Console.WriteLine("Successfully encrypted the file."),
            //    error => Console.WriteLine("An error occurred while encrypting the file: {0}", error.Message));


            //Func<double, double> sqrt = Math.Sqrt;
            //Func<double, double> abs = Math.Abs;
            //Func<double, double> log = Math.Log;

            //CallAnd();

            //var absSqrt = sqrt.Compose(abs);
            //Console.WriteLine("absSqrt = " + absSqrt(-2));

            //var absSqrtLog1 = log.Compose(sqrt).Compose(abs);
            //var absSqrtLog2 = log.Compose(sqrt.Compose(abs));

            //Console.WriteLine("absSqrtLog1 = " + absSqrtLog1(-2));
            //Console.WriteLine("absSqrtLog2 = " + absSqrtLog2(-2));



            Console.ReadKey();
        }
    }
}
