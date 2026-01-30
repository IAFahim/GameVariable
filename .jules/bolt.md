## 2025-05-15 - [Efficient ToString in netstandard2.1]
**Learning:** In `netstandard2.1`, string interpolation `$"{a}/{b}"` for value types boxes them into objects and uses `string.Format`, which parses the format string at runtime. Both cause unnecessary allocations and CPU overhead.
**Action:** Use `stackalloc char` buffer with `TryFormat` and `new string(ReadOnlySpan<char>)` for `ToString()` overrides in high-performance structs. This avoids boxing and reduces formatting time significantly.
