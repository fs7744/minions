using BenchmarkDotNet.Attributes;
using Minions;

namespace BenchmarkTest
{
    [BenchmarkDotNet.Attributes.AllStatisticsColumn]
    public class RadixTreeTest
    {
        private RadixTree<string> t;

        public RadixTreeTest()
        {
            t = new RadixTree<string>();
            int count = 0;
            foreach (var item in File.ReadAllLines("data.txt"))
            {
                t.AddNode(item, item);
                count++;
            }
            Console.WriteLine($"load {count} done");
        }

        [Benchmark]
        public void Search() => t.Search("sanitation", (k, o) => true, null);
    }
}