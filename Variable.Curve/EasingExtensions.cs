namespace Variable.Curve;

/// <summary>
///     Extension methods for easier usage of easing functions.
/// </summary>
public static class EasingExtensions
{
    /// <summary>
    ///     Applies the specified easing function to the normalized value t.
    /// </summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="type">The easing type.</param>
    /// <returns>The eased value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ease(this float t, EaseType type)
    {
        return CurveLogic.Evaluate(t, type);
    }

    /// <summary>
    ///     Interpolates between start and end using the specified easing function.
    /// </summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="start">The start value.</param>
    /// <param name="end">The end value.</param>
    /// <param name="type">The easing type.</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseLerp(this float t, float start, float end, EaseType type)
    {
        float easedT = CurveLogic.Evaluate(t, type);
        return start + (end - start) * easedT;
    }
}
