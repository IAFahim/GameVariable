using BenchmarkDotNet.Running;

namespace GameVariable.Intent.Tests
{
    public class BenchmarkProgram
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<IntentBenc/hmarks>();
        }
    }
}
