namespace Variable.Curve;

/// <summary>
///     Provides raw mathematical easing functions.
///     All functions accept a normalized time 't' usually in range [0, 1].
/// </summary>
public static class CurveLogic
{
    private const float PI = MathF.PI;
    private const float HalfPI = PI / 2f;

    /// <summary>
    ///     Evaluates the specified easing function at time t.
    ///     Clamps t between 0 and 1 before evaluation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Evaluate(float t, EaseType type)
    {
        // Clamp t to ensure math doesn't break (e.g. Sqrt of negative)
        CoreMath.Clamp(t, 0f, 1f, out var c);
        t = c;

        return type switch
        {
            EaseType.Linear => t,
            EaseType.InSine => InSine(t),
            EaseType.OutSine => OutSine(t),
            EaseType.InOutSine => InOutSine(t),
            EaseType.InQuad => InQuad(t),
            EaseType.OutQuad => OutQuad(t),
            EaseType.InOutQuad => InOutQuad(t),
            EaseType.InCubic => InCubic(t),
            EaseType.OutCubic => OutCubic(t),
            EaseType.InOutCubic => InOutCubic(t),
            EaseType.InQuart => InQuart(t),
            EaseType.OutQuart => OutQuart(t),
            EaseType.InOutQuart => InOutQuart(t),
            EaseType.InQuint => InQuint(t),
            EaseType.OutQuint => OutQuint(t),
            EaseType.InOutQuint => InOutQuint(t),
            EaseType.InExpo => InExpo(t),
            EaseType.OutExpo => OutExpo(t),
            EaseType.InOutExpo => InOutExpo(t),
            EaseType.InCirc => InCirc(t),
            EaseType.OutCirc => OutCirc(t),
            EaseType.InOutCirc => InOutCirc(t),
            EaseType.InBack => InBack(t),
            EaseType.OutBack => OutBack(t),
            EaseType.InOutBack => InOutBack(t),
            EaseType.InElastic => InElastic(t),
            EaseType.OutElastic => OutElastic(t),
            EaseType.InOutElastic => InOutElastic(t),
            EaseType.InBounce => InBounce(t),
            EaseType.OutBounce => OutBounce(t),
            EaseType.InOutBounce => InOutBounce(t),
            _ => t
        };
    }

    // --- Sine ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InSine(float t) => 1f - MathF.Cos(t * HalfPI);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutSine(float t) => MathF.Sin(t * HalfPI);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutSine(float t) => -(MathF.Cos(PI * t) - 1f) / 2f;

    // --- Quad ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InQuad(float t) => t * t;

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutQuad(float t) => 1f - (1f - t) * (1f - t);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;

    // --- Cubic ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InCubic(float t) => t * t * t;

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutCubic(float t) => 1f - MathF.Pow(1f - t, 3f);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 3f) / 2f;

    // --- Quart ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InQuart(float t) => t * t * t * t;

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutQuart(float t) => 1f - MathF.Pow(1f - t, 4f);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutQuart(float t) => t < 0.5f ? 8f * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 4f) / 2f;

    // --- Quint ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InQuint(float t) => t * t * t * t * t;

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutQuint(float t) => 1f - MathF.Pow(1f - t, 5f);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutQuint(float t) => t < 0.5f ? 16f * t * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 5f) / 2f;

    // --- Expo ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InExpo(float t) => t == 0f ? 0f : MathF.Pow(2f, 10f * t - 10f);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutExpo(float t) => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutExpo(float t)
    {
        return t == 0f ? 0f
            : t == 1f ? 1f
            : t < 0.5f ? MathF.Pow(2f, 20f * t - 10f) / 2f
            : (2f - MathF.Pow(2f, -20f * t + 10f)) / 2f;
    }

    // --- Circ ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InCirc(float t) => 1f - MathF.Sqrt(1f - MathF.Pow(t, 2f));

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutCirc(float t) => MathF.Sqrt(1f - MathF.Pow(t - 1f, 2f));

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutCirc(float t)
    {
        return t < 0.5f
            ? (1f - MathF.Sqrt(1f - MathF.Pow(2f * t, 2f))) / 2f
            : (MathF.Sqrt(1f - MathF.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
    }

    // --- Back ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return c3 * t * t * t - c1 * t * t;
    }

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * MathF.Pow(t - 1f, 3f) + c1 * MathF.Pow(t - 1f, 2f);
    }

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        return t < 0.5f
            ? MathF.Pow(2f * t, 2f) * ((c2 + 1f) * 2f * t - c2) / 2f
            : (MathF.Pow(2f * t - 2f, 2f) * ((c2 + 1f) * (2f * t - 2f) + c2) + 2f) / 2f;
    }

    // --- Elastic ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InElastic(float t)
    {
        const float c4 = 2f * MathF.PI / 3f;
        return t == 0f ? 0f
            : t == 1f ? 1f
            : -MathF.Pow(2f, 10f * t - 10f) * MathF.Sin((t * 10f - 10.75f) * c4);
    }

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutElastic(float t)
    {
        const float c4 = 2f * MathF.PI / 3f;
        return t == 0f ? 0f
            : t == 1f ? 1f
            : MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutElastic(float t)
    {
        const float c5 = 2f * MathF.PI / 4.5f;
        return t == 0f ? 0f
            : t == 1f ? 1f
            : t < 0.5f
                ? -(MathF.Pow(2f, 20f * t - 10f) * MathF.Sin((20f * t - 11.125f) * c5)) / 2f
                : (MathF.Pow(2f, -20f * t + 10f) * MathF.Sin((20f * t - 11.125f) * c5)) / 2f + 1f;
    }

    // --- Bounce ---
    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InBounce(float t) => 1f - OutBounce(1f - t);

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float OutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1f / d1)
        {
            return n1 * t * t;
        }
        if (t < 2f / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        }
        if (t < 2.5f / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        }
        return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    /// <summary>Evaluates the easing function.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InOutBounce(float t)
    {
        return t < 0.5f
            ? (1f - OutBounce(1f - 2f * t)) / 2f
            : (1f + OutBounce(2f * t - 1f)) / 2f;
    }
}
