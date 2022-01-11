using BenchmarkDotNet.Running;

namespace MagicNumber.Tests.Benchmarks
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<CheckFileExtensionBenchmarks>();
        }
    }
}
