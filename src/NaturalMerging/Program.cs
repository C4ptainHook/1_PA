using NaturalMerging.Algorithm;
using NaturalMerging.Generators;
using System.Diagnostics;

namespace NaturalMerging
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            bool generate = false;

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timeSpan;
            if (generate)
            {
                FileGenerator gen = new FileGenerator(@"C:\Users\boyko\Desktop\Generated.csv", 1000000000);
                stopwatch.Start();
                gen.Generate();
                stopwatch.Stop();
                timeSpan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
                Console.WriteLine(timeSpan.TotalSeconds);
            }

            stopwatch.Reset();
            RecordSorter rgen = new RecordSorter(@"C:\Users\boyko\Desktop\Generated.csv");
            stopwatch.Start();
            rgen.Sort();
            stopwatch.Stop();
            timeSpan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(timeSpan.TotalSeconds);
        }
    }
}