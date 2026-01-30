namespace Variable.Curve;

/// <summary>
///     Pure logic for standard easing functions.
///     All methods accept a normalized time 't' (usually 0 to 1) and return the eased value.
///     Inputs are NOT clamped, allowing for extrapolation (values &gt; 1 or &lt; 0).
/// </summary>
public static class EasingLogic
{
    private const float PI = MathF.PI;
    private const float HALF_PI = MathF.PI / 2f;

    /// <summary>Linear easing (no change).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Linear(float t) => t;

    #region Sine

    /// <summary>Sine In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InSine(float t) => 1f - MathF.Cos((t * PI) / 2f);

    /// <summary>Sine Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutSine(float t) => MathF.Sin((t * PI) / 2f);

    /// <summary>Sine InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutSine(float t) => -(MathF.Cos(PI * t) - 1f) / 2f;

    #endregion

    #region Quad

    /// <summary>Quadratic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InQuad(float t) => t * t;

    /// <summary>Quadratic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutQuad(float t) => 1f - (1f - t) * (1f - t);

    /// <summary>Quadratic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;

    #endregion

    #region Cubic

    /// <summary>Cubic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InCubic(float t) => t * t * t;

    /// <summary>Cubic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutCubic(float t) => 1f - MathF.Pow(1f - t, 3f);

    /// <summary>Cubic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 3f) / 2f;

    #endregion

    #region Quart

    /// <summary>Quartic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InQuart(float t) => t * t * t * t;

    /// <summary>Quartic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutQuart(float t) => 1f - MathF.Pow(1f - t, 4f);

    /// <summary>Quartic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutQuart(float t) => t < 0.5f ? 8f * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 4f) / 2f;

    #endregion

    #region Quint

    /// <summary>Quintic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InQuint(float t) => t * t * t * t * t;

    /// <summary>Quintic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutQuint(float t) => 1f - MathF.Pow(1f - t, 5f);

    /// <summary>Quintic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutQuint(float t) => t < 0.5f ? 16f * t * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 5f) / 2f;

    #endregion

    #region Expo

    /// <summary>Exponential In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InExpo(float t) => t == 0f ? 0f : MathF.Pow(2f, 10f * t - 10f);

    /// <summary>Exponential Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutExpo(float t) => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);

    /// <summary>Exponential InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutExpo(float t)
    {
        return t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : t < 0.5f
                    ? MathF.Pow(2f, 20f * t - 10f) / 2f
                    : (2f - MathF.Pow(2f, -20f * t + 10f)) / 2f;
    }

    #endregion

    #region Circ

    /// <summary>Circular In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InCirc(float t) => 1f - MathF.Sqrt(1f - MathF.Pow(t, 2f));

    /// <summary>Circular Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutCirc(float t) => MathF.Sqrt(1f - MathF.Pow(t - 1f, 2f));

    /// <summary>Circular InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutCirc(float t)
    {
        return t < 0.5f
            ? (1f - MathF.Sqrt(1f - MathF.Pow(2f * t, 2f))) / 2f
            : (MathF.Sqrt(1f - MathF.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
    }

    #endregion

    #region Back

    /// <summary>Back In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return c3 * t * t * t - c1 * t * t;
    }

    /// <summary>Back Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * MathF.Pow(t - 1f, 3f) + c1 * MathF.Pow(t - 1f, 2f);
    }

    /// <summary>Back InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return t < 0.5f
            ? (MathF.Pow(2f * t, 2f) * ((c2 + 1f) * 2f * t - c2)) / 2f
            : (MathF.Pow(2f * t - 2f, 2f) * ((c2 + 1f) * (2f * t - 2f) + c2) + 2f) / 2f;
    }

    #endregion

    #region Elastic

    /// <summary>Elastic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InElastic(float t)
    {
        const float c4 = (2f * PI) / 3f;

        return t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : -MathF.Pow(2f, 10f * t - 10f) * MathF.Sin((t * 10f - 10.75f) * c4);
    }

    /// <summary>Elastic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutElastic(float t)
    {
        const float c4 = (2f * PI) / 3f;

        return t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    /// <summary>Elastic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutElastic(float t)
    {
        const float c5 = (2f * PI) / 4.5f;

        return t == 0f
            ? 0f
            : t == 1f
                ? 1f
                : t < 0.5f
                    ? -(MathF.Pow(2f, 20f * t - 10f) * MathF.Sin((20f * t - 11.125f) * c5)) / 2f
                    : (MathF.Pow(2f, -20f * t + 10f) * MathF.Sin((20f * t - 11.125f) * c5)) / 2f + 1f;
    }

    #endregion

    #region Bounce

    /// <summary>Bounce Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1f / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2f / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        }
        else if (t < 2.5f / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        }
        else
        {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }

    /// <summary>Bounce In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InBounce(float t) => 1f - OutBounce(1f - t);

    /// <summary>Bounce InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutBounce(float t)
    {
        return t < 0.5f
            ? (1f - OutBounce(1f - 2f * t)) / 2f
            : (1f + OutBounce(2f * t - 1f)) / 2f;
    }

    #endregion
}
