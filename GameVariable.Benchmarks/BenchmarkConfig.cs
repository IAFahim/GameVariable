using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

namespace GameVariable.Benchmarks;

public static class BenchmarkConfig
{
    public static IConfig Default => ManualConfig.Create(DefaultConfig.Instance)
        .AddExporter(JsonExporter.Full)
        .AddExporter(MarkdownExporter.GitHub)
        .AddJob(Job.Default
            .WithWarmupCount(2)
            .WithIterationCount(10)
        )
        .AddDiagnoser(MemoryDiagnoser.Default);
}
