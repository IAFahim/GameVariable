using System.Globalization;
using BenchmarkDotNet.Attributes;
using Variable.Bounded;

namespace GameVariable.Benchmarks.Bounded;

[MemoryDiagnoser]
[ShortRunJob]
public class BoundedIntBenchmarks
{
    private BoundedInt _value = new(100, 0, 50);
    private const int Delta = 10;

    [Benchmark]
    public BoundedInt Construction()
    {
        return new BoundedInt(100, 0, 50);
    }

    [Benchmark]
    public BoundedInt Addition()
    {
        return _value + Delta;
    }

    [Benchmark]
    public BoundedInt Subtraction()
    {
        return _value - Delta;
    }

    [Benchmark]
    public BoundedInt Increment()
    {
        var v = _value;
        v++;
        return v;
    }

    [Benchmark]
    public BoundedInt Decrement()
    {
        var v = _value;
        v--;
        return v;
    }

    [Benchmark]
    public string ToString()
    {
        return _value.ToString();
    }

    [Benchmark]
    public bool TryFormat()
    {
        Span<char> buffer = stackalloc char[32];
        return _value.TryFormat(buffer, out _);
    }

    [Benchmark]
    public bool TryFormat_Ratio()
    {
        Span<char> buffer = stackalloc char[32];
        return _value.TryFormat(buffer, out _, "R");
    }

    [Benchmark(Baseline = true)]
    public string StringFormat_Manual()
    {
        return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", _value.Current, _value.Max);
    }
}
