using BenchmarkDotNet.Attributes;
using Variable.Regen;

namespace GameVariable.Benchmarks.Regen;

[MemoryDiagnoser]
[ShortRunJob]
public class RegenFloatBenchmarks
{
    private RegenFloat _positiveRegen = new(100f, 50f, 10f);
    private RegenFloat _negativeRegen = new(100f, 50f, -10f);
    private RegenFloat _zeroRegen = new(100f, 50f, 0f);
    private const float DeltaTime = 0.016f; // 60 FPS

    [Benchmark]
    public RegenFloat Construction()
    {
        return new RegenFloat(100f, 50f, 5f);
    }

    [Benchmark]
    public void Tick_PositiveRate()
    {
        var r = _positiveRegen;
        r.Tick(DeltaTime);
    }

    [Benchmark]
    public void Tick_NegativeRate()
    {
        var r = _negativeRegen;
        r.Tick(DeltaTime);
    }

    [Benchmark]
    public void Tick_ZeroRate()
    {
        var r = _zeroRegen;
        r.Tick(DeltaTime);
    }

    [Benchmark]
    public bool IsFull()
    {
        return _positiveRegen.IsFull();
    }

    [Benchmark]
    public bool IsEmpty()
    {
        return _positiveRegen.IsEmpty();
    }

    [Benchmark]
    public float GetRatio()
    {
        return _positiveRegen.GetRatio();
    }
}
