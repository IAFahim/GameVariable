using BenchmarkDotNet.Attributes;

namespace GameVariable.Benchmarks.TimerBenchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class CooldownBenchmarks
{
    [Benchmark]
    public Variable.Timer.Cooldown Construction()
    {
        return new Variable.Timer.Cooldown(3.0f);
    }
}
