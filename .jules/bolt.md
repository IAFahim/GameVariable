## 2026-01-31 - Allocation-Free String Formatting Pattern
**Learning:** In high-performance .NET code (like this project), overriding `ToString()` with `string.Format` is a significant source of allocations and latency. Using `stackalloc char` buffers combined with the `TryFormat` API reduces allocations by ~54% and improves performance by up to ~63% for integer-based types.

**Action:** Always implement `TryFormat` for performance-critical structs and use it in `ToString()` overrides to minimize heap pressure. Ensure `CultureInfo.InvariantCulture` is used as the default to maintain consistent output across different system locales.
