namespace Variable.Curve;

/// <summary>
///     Extension methods for easing functions and curve utilities.
/// </summary>
public static class CurveExtensions
{
    /// <summary>
    ///     Applies an easing function to a normalized value (t).
    /// </summary>
    /// <param name="t">The normalized time value (usually 0 to 1).</param>
    /// <param name="type">The type of easing to apply.</param>
    /// <returns>The eased value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ease(this float t, EasingType type)
    {
        return type switch
        {
            EasingType.Linear => EasingLogic.Linear(t),
            EasingType.InSine => EasingLogic.InSine(t),
            EasingType.OutSine => EasingLogic.OutSine(t),
            EasingType.InOutSine => EasingLogic.InOutSine(t),
            EasingType.InQuad => EasingLogic.InQuad(t),
            EasingType.OutQuad => EasingLogic.OutQuad(t),
            EasingType.InOutQuad => EasingLogic.InOutQuad(t),
            EasingType.InCubic => EasingLogic.InCubic(t),
            EasingType.OutCubic => EasingLogic.OutCubic(t),
            EasingType.InOutCubic => EasingLogic.InOutCubic(t),
            EasingType.InQuart => EasingLogic.InQuart(t),
            EasingType.OutQuart => EasingLogic.OutQuart(t),
            EasingType.InOutQuart => EasingLogic.InOutQuart(t),
            EasingType.InQuint => EasingLogic.InQuint(t),
            EasingType.OutQuint => EasingLogic.OutQuint(t),
            EasingType.InOutQuint => EasingLogic.InOutQuint(t),
            EasingType.InExpo => EasingLogic.InExpo(t),
            EasingType.OutExpo => EasingLogic.OutExpo(t),
            EasingType.InOutExpo => EasingLogic.InOutExpo(t),
            EasingType.InCirc => EasingLogic.InCirc(t),
            EasingType.OutCirc => EasingLogic.OutCirc(t),
            EasingType.InOutCirc => EasingLogic.InOutCirc(t),
            EasingType.InBack => EasingLogic.InBack(t),
            EasingType.OutBack => EasingLogic.OutBack(t),
            EasingType.InOutBack => EasingLogic.InOutBack(t),
            EasingType.InElastic => EasingLogic.InElastic(t),
            EasingType.OutElastic => EasingLogic.OutElastic(t),
            EasingType.InOutElastic => EasingLogic.InOutElastic(t),
            EasingType.InBounce => EasingLogic.InBounce(t),
            EasingType.OutBounce => EasingLogic.OutBounce(t),
            EasingType.InOutBounce => EasingLogic.InOutBounce(t),
            _ => t
        };
    }

    /// <summary>
    ///     Linearly interpolates between two values.
    ///     Equivalent to (1 - t) * a + t * b.
    /// </summary>
    /// <param name="t">The normalized weight (usually 0 to 1).</param>
    /// <param name="a">The start value.</param>
    /// <param name="b">The end value.</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(this float t, float a, float b)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    ///     Linearly interpolates between two values using a specific easing type.
    /// </summary>
    /// <param name="t">The normalized time (usually 0 to 1).</param>
    /// <param name="type">The easing type.</param>
    /// <param name="a">The start value.</param>
    /// <param name="b">The end value.</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(this float t, EasingType type, float a, float b)
    {
        return t.Ease(type).Lerp(a, b);
    }
}
