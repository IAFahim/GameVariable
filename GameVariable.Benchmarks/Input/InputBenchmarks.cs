using BenchmarkDotNet.Attributes;
using Variable.Input;

namespace GameVariable.Benchmarks.Input;

[MemoryDiagnoser]
[ShortRunJob]
public class InputRingBufferBenchmarks
{
    private InputRingBuffer _buffer = default;

    [GlobalSetup]
    public void Setup()
    {
        _buffer = new InputRingBuffer();
    }

    [Benchmark]
    public void RegisterInput()
    {
        for (int i = 0; i < 8; i++)
        {
            _buffer.RegisterInput(i);
        }
    }

    [Benchmark]
    public bool RegisterInput_Single()
    {
        return _buffer.RegisterInput(42);
    }

    [Benchmark]
    public bool Peek()
    {
        return _buffer.Peek(out _);
    }

    [Benchmark]
    public bool TryDequeue()
    {
        return _buffer.TryDequeue(out _);
    }

    [Benchmark]
    public void Clear()
    {
        _buffer.Clear();
    }
}
