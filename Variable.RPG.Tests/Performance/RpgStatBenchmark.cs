using System;
using System.Diagnostics;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;
using Variable.RPG;

namespace Variable.RPG.Tests.Performance;

public class RpgStatBenchmark
{
    private readonly ITestOutputHelper _output;

    public RpgStatBenchmark(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void BenchmarkToStringCompact()
    {
        var stat = new RpgStat(100f);
        stat.AddModifier(50f, 0.1f); // Value roughly 165
        stat.Recalculate();

        // Warmup
        for (int i = 0; i < 1000; i++)
        {
            var s = stat.ToStringCompact();
        }

        int iterations = 100_000;
        long startMem = GC.GetAllocatedBytesForCurrentThread();
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            var s = stat.ToStringCompact();
        }

        sw.Stop();
        long endMem = GC.GetAllocatedBytesForCurrentThread();

        long totalBytes = endMem - startMem;
        double bytesPerOp = (double)totalBytes / iterations;
        double msPerOp = sw.Elapsed.TotalMilliseconds * 1000 / iterations; // microseconds

        _output.WriteLine($"Iterations: {iterations}");
        _output.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds:F2} ms");
        _output.WriteLine($"Total Alloc: {totalBytes:N0} bytes");
        _output.WriteLine($"Bytes/Op: {bytesPerOp:F2}");
        _output.WriteLine($"Time/Op: {msPerOp:F4} us");

        // Also verify the output format hasn't changed (sanity check during benchmark)
        // "165.0 ◀ [100.0 + 50.0] × 110%"
        string expected = string.Format(CultureInfo.InvariantCulture,
            "{0:F1} ◀ [{1:F1} + {2:F1}] × {3:P0}", stat.Value, stat.Base, stat.ModAdd, stat.ModMult);
        string actual = stat.ToStringCompact();
        Assert.Equal(expected, actual);
    }
}
