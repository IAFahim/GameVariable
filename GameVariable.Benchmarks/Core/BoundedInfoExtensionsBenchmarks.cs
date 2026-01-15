using BenchmarkDotNet.Attributes;
using Variable.Bounded;
using Variable.Core;

namespace GameVariable.Benchmarks.Core;

[MemoryDiagnoser]
[ShortRunJob]
public class BoundedInfoExtensionsBenchmarks
{
    private readonly BoundedFloat _fullValue = new(100f, 100f);
    private readonly BoundedFloat _emptyValue = new(100f, 0f);
    private readonly BoundedFloat _halfValue = new(100f, 50f);
    private readonly BoundedFloat _almostFull = new(100f, 99.99f);
    private readonly BoundedFloat _almostEmpty = new(100f, 0.01f);

    [Benchmark]
    public bool IsFull_Full()
    {
        return _fullValue.IsFull();
    }

    [Benchmark]
    public bool IsEmpty_Empty()
    {
        return _emptyValue.IsEmpty();
    }

    [Benchmark]
    public double GetRatio_Half()
    {
        return _halfValue.GetRatio();
    }

    [Benchmark]
    public bool IsFull_WithTolerance()
    {
        return _almostFull.IsFull(0.01f);
    }

    [Benchmark]
    public bool IsEmpty_WithTolerance()
    {
        return _almostEmpty.IsEmpty(0.01f);
    }
}
