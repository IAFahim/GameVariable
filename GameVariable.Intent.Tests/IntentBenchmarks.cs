using System;
using System.Diagnostics;
using GameVariable.Intent;
using Xunit;
using Xunit.Abstractions;

namespace GameVariable.Intent.Tests;

public class IntentBenchmarks
{
    private readonly ITestOutputHelper _output;

    public IntentBenchmarks(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Benchmark_DispatchEvent_SingleTransition()
    {
        int iterations = 100_000;
        long startMem = GC.GetAllocatedBytesForCurrentThread();
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
        }

        sw.Stop();
        long endMem = GC.GetAllocatedBytesForCurrentThread();

        long totalBytes = endMem - startMem;
        double bytesPerOp = (double)totalBytes / iterations;
        double usPerOp = sw.Elapsed.TotalMilliseconds * 1000 / iterations; // microseconds

        _output.WriteLine($"Iterations: {iterations}");
        _output.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds:F2} ms");
        _output.WriteLine($"Total Alloc: {totalBytes:N0} bytes");
        _output.WriteLine($"Bytes/Op: {bytesPerOp:F2}");
        _output.WriteLine($"Time/Op: {usPerOp:F4} us");

        // Sanity check that the state machine actually works
        var checkState = new IntentState();
        checkState.Start();
        checkState.DispatchEvent(IntentState.EventId.ACTIVATED);
        Assert.Equal(IntentState.StateId.WAITING_TO_RUN, checkState.stateId);
    }

    [Fact]
    public void Benchmark_DispatchEvent_MultipleTransitions()
    {
        int iterations = 100_000;
        long startMem = GC.GetAllocatedBytesForCurrentThread();
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
            state.DispatchEvent(IntentState.EventId.START_RUNNING);
            state.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
        }

        sw.Stop();
        long endMem = GC.GetAllocatedBytesForCurrentThread();

        long totalBytes = endMem - startMem;
        double bytesPerOp = (double)totalBytes / iterations;
        double usPerOp = sw.Elapsed.TotalMilliseconds * 1000 / iterations; // microseconds

        _output.WriteLine($"Iterations: {iterations}");
        _output.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds:F2} ms");
        _output.WriteLine($"Total Alloc: {totalBytes:N0} bytes");
        _output.WriteLine($"Bytes/Op: {bytesPerOp:F2}");
        _output.WriteLine($"Time/Op: {usPerOp:F4} us");

        // Sanity check that the state machine actually works
        var checkState = new IntentState();
        checkState.Start();
        checkState.DispatchEvent(IntentState.EventId.ACTIVATED);
        checkState.DispatchEvent(IntentState.EventId.START_RUNNING);
        checkState.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
        Assert.Equal(IntentState.StateId.RAN_TO_COMPLETION, checkState.stateId);
    }

    [Fact]
    public void Benchmark_StateIdToString()
    {
        var state = new IntentState();
        state.Start();

        // Warmup
        for (int i = 0; i < 1000; i++)
        {
            state.StateIdToString(IntentState.StateId.RUNNING);
        }

        int iterations = 100_000;
        long startMem = GC.GetAllocatedBytesForCurrentThread();
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            state.StateIdToString(IntentState.StateId.RUNNING);
        }

        sw.Stop();
        long endMem = GC.GetAllocatedBytesForCurrentThread();

        long totalBytes = endMem - startMem;
        double bytesPerOp = (double)totalBytes / iterations;
        double usPerOp = sw.Elapsed.TotalMilliseconds * 1000 / iterations; // microseconds

        _output.WriteLine($"Iterations: {iterations}");
        _output.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds:F2} ms");
        _output.WriteLine($"Total Alloc: {totalBytes:N0} bytes");
        _output.WriteLine($"Bytes/Op: {bytesPerOp:F2}");
        _output.WriteLine($"Time/Op: {usPerOp:F4} us");

        // Sanity check
        Assert.Equal("RUNNING", state.StateIdToString(IntentState.StateId.RUNNING));
    }

    [Fact]
    public void Benchmark_EventIdToString()
    {
        var state = new IntentState();
        state.Start();

        // Warmup
        for (int i = 0; i < 1000; i++)
        {
            state.EventIdToString(IntentState.EventId.START_RUNNING);
        }

        int iterations = 100_000;
        long startMem = GC.GetAllocatedBytesForCurrentThread();
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            state.EventIdToString(IntentState.EventId.START_RUNNING);
        }

        sw.Stop();
        long endMem = GC.GetAllocatedBytesForCurrentThread();

        long totalBytes = endMem - startMem;
        double bytesPerOp = (double)totalBytes / iterations;
        double usPerOp = sw.Elapsed.TotalMilliseconds * 1000 / iterations; // microseconds

        _output.WriteLine($"Iterations: {iterations}");
        _output.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds:F2} ms");
        _output.WriteLine($"Total Alloc: {totalBytes:N0} bytes");
        _output.WriteLine($"Bytes/Op: {bytesPerOp:F2}");
        _output.WriteLine($"Time/Op: {usPerOp:F4} us");

        // Sanity check
        Assert.Equal("START_RUNNING", state.EventIdToString(IntentState.EventId.START_RUNNING));
    }
}
