using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

namespace GameVariable.Benchmarks;

public static class BenchmarkConfig
{
    public static IConfig Default => ManualConfig.CreateEmpty()
        .AddLogger(ConsoleLogger.Default)
        .AddJob(Job.Default
            .WithWarmupCount(2)
            .WithIterationCount(10)
        )
        .AddDiagnoser(MemoryDiagnoser.Default);
}
