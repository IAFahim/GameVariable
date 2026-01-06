namespace Variable.Core;

/// <summary>
///     Shared constants for mathematical operations and logic across the library.
/// </summary>
public static class MathConstants
{
    /// <summary>
    ///     The default tolerance constant (immutable).
    ///     Use this when you need a compile-time constant.
    /// </summary>
    public const float DefaultTolerance = 0.001f;

    /// <summary>
    ///     The default tolerance for floating-point comparisons to handle precision errors.
    ///     Used to snap values to min/max and determine equality.
    ///     This value is used across all packages for consistency.
    /// </summary>
    /// <remarks>
    ///     For most game scenarios, 0.001f (1/1000th) is sufficient.
    ///     Override this value at runtime if your application requires different precision:
    ///     - Higher precision: Use 0.0001f or smaller
    ///     - Lower precision/performance: Use 0.01f
    ///     - Large-scale values: Consider scaling tolerance proportionally
    /// </remarks>
    public static float Tolerance = 0.001f;
}