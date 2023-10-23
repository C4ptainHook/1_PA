using NaturalMerging.Generators;
using System.Diagnostics;

namespace NaturalMerging
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            FileGenerator gen = new FileGenerator(@"C:\Users\boyko\Desktop\Generated.csv", 10);
            stopwatch.Start();
            gen.Generate();
            stopwatch.Stop();
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(timeSpan.TotalSeconds);
        }
    }
}