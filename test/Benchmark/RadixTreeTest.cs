using BenchmarkDotNet.Attributes;
using Minions;
using System.Collections.Generic;

namespace BenchmarkTest
{
    [MemoryDiagnoser]
    [BenchmarkDotNet.Attributes.AllStatisticsColumn]
    public class RadixTreeTest
    {
        private RadixTree<string> t;
        private Dictionary<string, string> dict = new Dictionary<string, string>();
        private List<string> list = new List<string>();

        public RadixTreeTest()
        {
            t = new RadixTree<string>();
            int count = 0;
            foreach (var item in File.ReadAllLines("data.txt"))
            {
                t.AddNode(item, item);
                dict.Add(item, item);
                list.Add(item);
                count++;
            }
            list.Sort();
            Console.WriteLine($"load {count} done");
        }

        [Benchmark]
        public void Search() => t.Search("sanitation", (k, o) => true, null);

        [Benchmark]
        public void SearchDictionary() => dict.TryGetValue("sanitation", out var v);

        [Benchmark]
        public void ListBinarySearch() => list.BinarySearch("sanitation");
    }
}