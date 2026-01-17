using BenchmarkDotNet.Attributes;
using Variable.Experience;

namespace GameVariable.Benchmarks.Experience;

[MemoryDiagnoser]
[ShortRunJob]
public class ExperienceIntBenchmarks
{
    private ExperienceInt _xp = new(1000, 500, 5);
    private const int XpGain = 100;

    [Benchmark]
    public ExperienceInt Construction()
    {
        return new ExperienceInt(1000);
    }

    [Benchmark]
    public ExperienceInt Addition()
    {
        return _xp + XpGain;
    }

    [Benchmark]
    public bool IsFull()
    {
        return _xp.IsFull();
    }

    [Benchmark]
    public bool IsEmpty()
    {
        return _xp.IsEmpty();
    }

    [Benchmark]
    public double GetRatio()
    {
        return _xp.GetRatio();
    }

    [Benchmark]
    public override string ToString()
    {
        return _xp.ToString();
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class ExperienceLongBenchmarks
{
    private ExperienceLong _xp = new(10000L, 5000L, 5);
    private const long XpGain = 1000L;

    [Benchmark]
    public ExperienceLong Construction()
    {
        return new ExperienceLong(10000L);
    }

    [Benchmark]
    public ExperienceLong Addition()
    {
        return _xp + XpGain;
    }

    [Benchmark]
    public bool IsFull()
    {
        return _xp.IsFull();
    }

    [Benchmark]
    public bool IsEmpty()
    {
        return _xp.IsEmpty();
    }

    [Benchmark]
    public double GetRatio()
    {
        return _xp.GetRatio();
    }

    [Benchmark]
    public override string ToString()
    {
        return _xp.ToString();
    }
}
