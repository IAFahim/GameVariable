namespace Variable.Stat;

/// <summary>
/// Provides static logic for calculating stat values from base values and modifiers.
/// Designed for use with ECS or other data-oriented architectures where state is separated from logic.
/// </summary>
public static partial class StatLogic
{
    /// <summary>
    /// A small tolerance value used for floating-point comparisons to handle precision errors.
    /// </summary>
    public const float TOLERANCE = 0.001f;

    /// <summary>
    /// A small tolerance value used for double-precision floating-point comparisons.
    /// </summary>
    public const double TOLERANCE_DOUBLE = 0.001;
}
