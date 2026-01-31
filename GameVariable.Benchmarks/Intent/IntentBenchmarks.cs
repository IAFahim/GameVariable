using BenchmarkDotNet.Attributes;
using GameVariable.Intent;

namespace GameVariable.Benchmarks.Intent;

[MemoryDiagnoser]
[ShortRunJob]
public class IntentBenchmarks
{
    [Benchmark]
    public IntentState Construction()
    {
        return new IntentState();
    }

    [Benchmark]
    public void Start()
    {
        var state = new IntentState();
        state.Start();
    }

    [Benchmark]
    public void DispatchEvent_SingleTransition()
    {
        var state = new IntentState();
        state.Start();
        state.DispatchEvent(IntentState.EventId.ACTIVATE);
    }
}
