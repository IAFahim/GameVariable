namespace Variable.Regen;

/// <summary>
///     Provides static methods for regeneration and decay calculations.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
/// <remarks>
///     <para>This class contains the core logic for updating values based on rates.</para>
///     <para>All methods are stateless and work with ref parameters for zero allocation.</para>
/// </remarks>
public static partial class RegenLogic
{
    /// <summary>
    ///     Updates a value based on a rate and time delta.
    ///     Handles both regeneration (positive rate) and decay (negative rate).
    ///     Clamps the result between min and max.
    /// </summary>
    /// <param name="current">The current value to modify.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="rate">The rate of change per second.</param>
    /// <param name="deltaTime">The time elapsed since the last update.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref float current, float min, float max, float rate, float deltaTime)
    {
        if (rate == 0f || deltaTime == 0f) return;

        current += rate * deltaTime;

        if (current > max) current = max;
        else if (current < min) current = min;
    }
}