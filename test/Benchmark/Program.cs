using BenchmarkDotNet.Running;

namespace BenchmarkTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //new RadixTreeTest().Search();
            var summary = BenchmarkRunner.Run<RadixTreeTest>();
        }
    }
}