using NaturalMerging.Algorithm;
using NaturalMerging.Generators;
using System.Diagnostics;

namespace NaturalMerging
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            //FileGenerator gen = new FileGenerator(@"C:\Users\boyko\Desktop\Generated.csv", 10000000);
            //stopwatch.Start();
            //gen.Generate();
            //stopwatch.Stop();
            //TimeSpan timeSpan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            //Console.WriteLine(timeSpan.TotalSeconds);

            RecordSorter rgen = new RecordSorter(@"C:\Users\boyko\Desktop\Generated.csv");
            stopwatch.Start();
            rgen.Sort();
            stopwatch.Stop();
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(timeSpan.TotalSeconds);
        }
    }
}