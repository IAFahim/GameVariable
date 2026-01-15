using BenchmarkDotNet.Attributes;
using SysTimer = Variable.Timer.Timer;

namespace GameVariable.Benchmarks.TimerBenchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class TimerBenchmarks
{
    [Benchmark]
    public SysTimer Construction()
    {
        return new SysTimer(2.0f);
    }
}
