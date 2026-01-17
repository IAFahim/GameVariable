using System.Globalization;
using BenchmarkDotNet.Attributes;
using Variable.Bounded;

namespace GameVariable.Benchmarks.Bounded;

[MemoryDiagnoser]
[ShortRunJob]
public class BoundedFloatBenchmarks
{
    private BoundedFloat _value = new(100f, 0f, 50f);
    private const float Delta = 10f;
    private const float Scalar = 2f;

    [Benchmark]
    public BoundedFloat Construction()
    {
        return new BoundedFloat(100f, 0f, 50f);
    }

    [Benchmark]
    public BoundedFloat Addition()
    {
        return _value + Delta;
    }

    [Benchmark]
    public BoundedFloat Subtraction()
    {
        return _value - Delta;
    }

    [Benchmark]
    public BoundedFloat Multiplication()
    {
        return _value * Scalar;
    }

    [Benchmark]
    public BoundedFloat Division()
    {
        return _value / Scalar;
    }

    [Benchmark]
    public BoundedFloat Increment()
    {
        var v = _value;
        v++;
        return v;
    }

    [Benchmark]
    public BoundedFloat Decrement()
    {
        var v = _value;
        v--;
        return v;
    }

    [Benchmark]
    public override string ToString()
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
