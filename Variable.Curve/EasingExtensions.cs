namespace Variable.Curve;

/// <summary>
///     Extension methods for easing functions.
/// </summary>
public static class EasingExtensions
{
    /// <summary>
    ///     Applies the specified easing function to the value t.
    /// </summary>
    /// <param name="t">The normalized time (usually 0 to 1).</param>
    /// <param name="type">The easing type.</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ease(this float t, EaseType type)
    {
        EasingLogic.Evaluate(t, type, out var result);
        return result;
    }
}
