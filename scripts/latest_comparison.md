# Benchmark Comparison Report

**Baseline:** benchmark_2026-01-17_05-08-53.json
**Current:** benchmark_2026-01-17_05-13-48.json

| Benchmark | Mean (Current) | Mean (Baseline) | Diff % | Alloc (Current) | Alloc (Baseline) |
|---|---|---|---|---|---|
| Clamp_Float | 567.50 ns | 0.00 ns | N/A | 736 B | 0 B |
| Clamp_Int | 961.00 ns | 0.02 ns | 5426777.42% 🔴 | 64 B | 0 B |
| Clamp_Long | 489.00 ns | 0.76 ns | 63881.34% 🔴 | 736 B | 0 B |
| Clamp_Byte | 515.50 ns | 0.00 ns | 19752207.54% 🔴 | 736 B | 0 B |
| Clamp_IntToByte | 338.00 ns | 0.00 ns | N/A | 736 B | 0 B |
| Min_Float | 399.00 ns | 0.01 ns | 3125875.21% 🔴 | 736 B | 0 B |
| Min_Int | 421.50 ns | 0.82 ns | 51593.11% 🔴 | 736 B | 0 B |
| Max_Float | 207.50 ns | 0.00 ns | N/A | 736 B | 0 B |
| Max_Int | 577.00 ns | 0.03 ns | 1909442.83% 🔴 | 736 B | 0 B |
| Abs_Float | 409.50 ns | 0.09 ns | 463455.78% 🔴 | 736 B | 0 B |
| Abs_Int | 202.50 ns | 0.01 ns | 1506366.16% 🔴 | 736 B | 0 B |
| Clamp_Float | 0.00 ns | 0.00 ns | N/A | 0 B | 0 B |
| Clamp_Int | 0.00 ns | 0.02 ns | -92.46% 🟢 | 0 B | 0 B |
| Clamp_Long | 0.13 ns | 0.76 ns | -83.60% 🟢 | 0 B | 0 B |
| Clamp_Byte | 0.02 ns | 0.00 ns | 650.68% 🔴 | 0 B | 0 B |
| Clamp_IntToByte | 0.00 ns | 0.00 ns | N/A | 0 B | 0 B |
| Min_Float | 0.00 ns | 0.01 ns | -100.00% 🟢 | 0 B | 0 B |
| Min_Int | 0.00 ns | 0.82 ns | -100.00% 🟢 | 0 B | 0 B |
| Max_Float | 0.00 ns | 0.00 ns | N/A | 0 B | 0 B |
| Max_Int | 0.00 ns | 0.03 ns | -100.00% 🟢 | 0 B | 0 B |
| Abs_Float | 0.00 ns | 0.09 ns | -100.00% 🟢 | 0 B | 0 B |
| Abs_Int | 0.05 ns | 0.01 ns | 252.62% 🔴 | 0 B | 0 B |
