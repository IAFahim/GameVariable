namespace Variable.Regen;

/// <summary>
///     Extension methods for <see cref="RegenFloat" /> providing logic operations.
///     These methods bridge from structs to primitive-only logic.
/// </summary>
public static class RegenExtensions
{
    /// <summary>
    ///     Advances the regeneration by the specified time delta.
    /// </summary>
    /// <param name="regen">The regen struct to tick.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this RegenFloat regen, float deltaTime)
    {
        // Decompose struct into primitives for logic
        RegenLogic.Tick(
            ref regen.Value.Current,
            regen.Value.Min,
            regen.Value.Max,
            regen.Rate,
            deltaTime
        );
    }

    /// <summary>
    ///     Determines whether the value is at maximum.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this RegenFloat regen, float tolerance = MathConstants.DefaultTolerance)
    {
        return regen.Value.Current >= regen.Value.Max - tolerance;
    }

    /// <summary>
    ///     Determines whether the value is at minimum (empty).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this RegenFloat regen, float tolerance = MathConstants.DefaultTolerance)
    {
        return regen.Value.Current <= regen.Value.Min + tolerance;
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this RegenFloat regen)
    {
        var range = regen.Value.Max - regen.Value.Min;
        return Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (regen.Value.Current - regen.Value.Min) / range;
    }
}