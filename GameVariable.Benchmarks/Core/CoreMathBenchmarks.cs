using BenchmarkDotNet.Attributes;
using Variable.Core;

namespace GameVariable.Benchmarks.Core;

[MemoryDiagnoser]
[ShortRunJob]
public class CoreMathBenchmarks
{
    private const float FloatValue = 50f;
    private const float FloatMin = 0f;
    private const float FloatMax = 100f;

    private const int IntValue = 50;
    private const int IntMin = 0;
    private const int IntMax = 100;

    private const long LongValue = 50L;
    private const long LongMin = 0L;
    private const long LongMax = 100L;

    private const byte ByteValue = 200;
    private const byte ByteMax = 100;

    [Benchmark]
    public void Clamp_Float()
    {
        CoreMath.Clamp(FloatValue, FloatMin, FloatMax, out _);
    }

    [Benchmark]
    public void Clamp_Int()
    {
        CoreMath.Clamp(IntValue, IntMin, IntMax, out _);
    }

    [Benchmark]
    public void Clamp_Long()
    {
        CoreMath.Clamp(LongValue, LongMin, LongMax, out _);
    }

    [Benchmark]
    public void Clamp_Byte()
    {
        CoreMath.Clamp(ByteValue, ByteMax, out _);
    }

    [Benchmark]
    public void Clamp_IntToByte()
    {
        CoreMath.Clamp(IntValue, ByteMax, out _);
    }

    [Benchmark]
    public void Min_Float()
    {
        CoreMath.Min(3f, 5f, out _);
    }

    [Benchmark]
    public void Min_Int()
    {
        CoreMath.Min(3, 5, out _);
    }

    [Benchmark]
    public void Max_Float()
    {
        CoreMath.Max(3f, 5f, out _);
    }

    [Benchmark]
    public void Max_Int()
    {
        CoreMath.Max(3, 5, out _);
    }

    [Benchmark]
    public void Abs_Float()
    {
        CoreMath.Abs(-50f, out _);
    }

    [Benchmark]
    public void Abs_Int()
    {
        CoreMath.Abs(-50, out _);
    }
}
