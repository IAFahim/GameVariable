using BenchmarkDotNet.Attributes;
using Variable.Reservoir;

namespace GameVariable.Benchmarks.Reservoir;

[MemoryDiagnoser]
[ShortRunJob]
public class ReservoirFloatBenchmarks
{
    private ReservoirFloat _reservoir = new(12f, 6f, 36f);

    [Benchmark]
    public ReservoirFloat Construction()
    {
        return new ReservoirFloat(12f, 12f, 36f);
    }

    [Benchmark]
    public float Refill()
    {
        var r = _reservoir;
        return r.Refill();
    }

    [Benchmark]
    public ReservoirFloat Volume_Subtraction()
    {
        var r = _reservoir;
        r.Volume = r.Volume - 1f;
        return r;
    }

    [Benchmark]
    public ReservoirFloat Volume_Addition()
    {
        var r = _reservoir;
        r.Volume = r.Volume + 1f;
        return r;
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class ReservoirIntBenchmarks
{
    private ReservoirInt _reservoir = new(12, 6, 36);

    [Benchmark]
    public ReservoirInt Construction()
    {
        return new ReservoirInt(12, 12, 36);
    }

    [Benchmark]
    public int Refill()
    {
        var r = _reservoir;
        return r.Refill();
    }

    [Benchmark]
    public ReservoirInt Volume_Subtraction()
    {
        var r = _reservoir;
        r.Volume = r.Volume - 1;
        return r;
    }

    [Benchmark]
    public ReservoirInt Volume_Addition()
    {
        var r = _reservoir;
        r.Volume = r.Volume + 1;
        return r;
    }
}
