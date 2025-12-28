namespace Variable.Core;

/// <summary>
///     Shared constants for mathematical operations and logic across the library.
/// </summary>
public static class MathConstants
{
    /// <summary>
    ///     The default tolerance for floating-point comparisons to handle precision errors.
    ///     Used to snap values to min/max and determine equality.
    /// </summary>
    public const float Tolerance = 0.001f;
}