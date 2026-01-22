# Benchmark Run Notes - 2026-01-21

## Execution Context
This benchmark run was executed manually as part of the daily routine.
Due to execution time constraints in the environment, the run was filtered to `*Clamp_Int*` and `*Clamp_Float*`.

## Observations
- **Zero Measurement Warnings:** BenchmarkDotNet reported that `Clamp_Int` and `Clamp_Float` durations were indistinguishable from empty method duration (approx 0.00 ns). This suggests that the JIT compiler (RyuJIT AVX2) may be optimizing away the method calls or the operations are sub-nanosecond and swallowed by overhead calculation.
- **Comparison:** The comparison report reflects these near-zero values.

## Future Recommendations
- Investigate if `CoreMath.Clamp` calls in benchmarks are being dead-code eliminated.
- Consider using `[MethodImpl(MethodImplOptions.NoInlining)]` or ensuring side effects in the benchmark (e.g., returning the value instead of `out _` or accumulating the result).
