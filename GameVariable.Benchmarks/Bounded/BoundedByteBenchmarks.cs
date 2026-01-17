using BenchmarkDotNet.Attributes;
using Variable.Bounded;

namespace GameVariable.Benchmarks.Bounded;

[MemoryDiagnoser]
[ShortRunJob]
public class BoundedByteBenchmarks
{
    private BoundedByte _value = new(100, 50);
    private const int Delta = 10;

    [Benchmark]
    public BoundedByte Construction()
    {
        return new BoundedByte(100, 50);
    }

    [Benchmark]
    public BoundedByte Addition()
    {
        return _value + Delta;
    }

    [Benchmark]
    public BoundedByte Subtraction()
    {
        return _value - Delta;
    }

    [Benchmark]
    public BoundedByte Increment()
    {
        var v = _value;
        v++;
        return v;
    }

    [Benchmark]
    public BoundedByte Decrement()
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
}
