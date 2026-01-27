namespace Variable.Curve;

/// <summary>
///     Stateless logic for evaluating easing curves.
///     Layer B: Core Logic.
/// </summary>
public static class CurveLogic
{
    private const float PI = MathF.PI;
    private const float HalfPI = PI / 2f;

    /// <summary>
    ///     Evaluates the easing function at time t (normalized 0-1).
    /// </summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="type">The type of easing to apply.</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(in float t, in EaseType type, out float result)
    {
        // Clamp input t to [0, 1] for safety
        float clampedT;
        CoreMath.Clamp(t, 0f, 1f, out clampedT);

        switch (type)
        {
            case EaseType.Linear: result = clampedT; break;
            case EaseType.InQuad: InQuad(in clampedT, out result); break;
            case EaseType.OutQuad: OutQuad(in clampedT, out result); break;
            case EaseType.InOutQuad: InOutQuad(in clampedT, out result); break;
            case EaseType.InCubic: InCubic(in clampedT, out result); break;
            case EaseType.OutCubic: OutCubic(in clampedT, out result); break;
            case EaseType.InOutCubic: InOutCubic(in clampedT, out result); break;
            case EaseType.InQuart: InQuart(in clampedT, out result); break;
            case EaseType.OutQuart: OutQuart(in clampedT, out result); break;
            case EaseType.InOutQuart: InOutQuart(in clampedT, out result); break;
            case EaseType.InQuint: InQuint(in clampedT, out result); break;
            case EaseType.OutQuint: OutQuint(in clampedT, out result); break;
            case EaseType.InOutQuint: InOutQuint(in clampedT, out result); break;
            case EaseType.InSine: InSine(in clampedT, out result); break;
            case EaseType.OutSine: OutSine(in clampedT, out result); break;
            case EaseType.InOutSine: InOutSine(in clampedT, out result); break;
            case EaseType.InExpo: InExpo(in clampedT, out result); break;
            case EaseType.OutExpo: OutExpo(in clampedT, out result); break;
            case EaseType.InOutExpo: InOutExpo(in clampedT, out result); break;
            case EaseType.InCirc: InCirc(in clampedT, out result); break;
            case EaseType.OutCirc: OutCirc(in clampedT, out result); break;
            case EaseType.InOutCirc: InOutCirc(in clampedT, out result); break;
            case EaseType.InBack: InBack(in clampedT, out result); break;
            case EaseType.OutBack: OutBack(in clampedT, out result); break;
            case EaseType.InOutBack: InOutBack(in clampedT, out result); break;
            case EaseType.InElastic: InElastic(in clampedT, out result); break;
            case EaseType.OutElastic: OutElastic(in clampedT, out result); break;
            case EaseType.InOutElastic: InOutElastic(in clampedT, out result); break;
            case EaseType.InBounce: InBounce(in clampedT, out result); break;
            case EaseType.OutBounce: OutBounce(in clampedT, out result); break;
            case EaseType.InOutBounce: InOutBounce(in clampedT, out result); break;
            default: result = clampedT; break;
        }
    }

    /// <summary>Quadratic ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuad(in float t, out float result)
    {
        result = t * t;
    }

    /// <summary>Quadratic ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuad(in float t, out float result)
    {
        result = t * (2f - t);
    }

    /// <summary>Quadratic ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuad(in float t, out float result)
    {
        result = t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }

    /// <summary>Cubic ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InCubic(in float t, out float result)
    {
        result = t * t * t;
    }

    /// <summary>Cubic ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutCubic(in float t, out float result)
    {
        float t1 = t - 1f;
        result = t1 * t1 * t1 + 1f;
    }

    /// <summary>Cubic ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutCubic(in float t, out float result)
    {
        if (t < 0.5f)
        {
            result = 4f * t * t * t;
        }
        else
        {
            float t1 = t - 1f;
            result = (2f * t - 2f) * (2f * t - 2f) * (2f * t - 2f) / 2f + 1f;
        }
    }

    /// <summary>Quartic ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuart(in float t, out float result)
    {
        result = t * t * t * t;
    }

    /// <summary>Quartic ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuart(in float t, out float result)
    {
        float t1 = t - 1f;
        result = 1f - t1 * t1 * t1 * t1;
    }

    /// <summary>Quartic ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuart(in float t, out float result)
    {
        float t2 = 2f * t;
        if (t2 < 1f)
        {
            result = 0.5f * t2 * t2 * t2 * t2;
        }
        else
        {
            t2 -= 2f;
            result = -0.5f * (t2 * t2 * t2 * t2 - 2f);
        }
    }

    /// <summary>Quintic ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuint(in float t, out float result)
    {
        result = t * t * t * t * t;
    }

    /// <summary>Quintic ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuint(in float t, out float result)
    {
        float t1 = t - 1f;
        result = 1f + t1 * t1 * t1 * t1 * t1;
    }

    /// <summary>Quintic ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuint(in float t, out float result)
    {
        float t2 = 2f * t;
        if (t2 < 1f)
        {
            result = 0.5f * t2 * t2 * t2 * t2 * t2;
        }
        else
        {
            t2 -= 2f;
            result = 0.5f * (t2 * t2 * t2 * t2 * t2 + 2f);
        }
    }

    /// <summary>Sinusoidal ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InSine(in float t, out float result)
    {
        result = 1f - MathF.Cos(t * HalfPI);
    }

    /// <summary>Sinusoidal ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutSine(in float t, out float result)
    {
        result = MathF.Sin(t * HalfPI);
    }

    /// <summary>Sinusoidal ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutSine(in float t, out float result)
    {
        result = -(MathF.Cos(PI * t) - 1f) / 2f;
    }

    /// <summary>Exponential ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InExpo(in float t, out float result)
    {
        result = (t == 0f) ? 0f : MathF.Pow(2f, 10f * (t - 1f));
    }

    /// <summary>Exponential ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutExpo(in float t, out float result)
    {
        result = (t == 1f) ? 1f : 1f - MathF.Pow(2f, -10f * t);
    }

    /// <summary>Exponential ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutExpo(in float t, out float result)
    {
        if (t == 0f) { result = 0f; return; }
        if (t == 1f) { result = 1f; return; }

        float t2 = 2f * t;
        if (t2 < 1f)
        {
            result = 0.5f * MathF.Pow(2f, 10f * (t2 - 1f));
        }
        else
        {
            result = 0.5f * (-MathF.Pow(2f, -10f * (t2 - 1f)) + 2f);
        }
    }

    /// <summary>Circular ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InCirc(in float t, out float result)
    {
        result = 1f - MathF.Sqrt(1f - t * t);
    }

    /// <summary>Circular ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutCirc(in float t, out float result)
    {
        float t1 = t - 1f;
        result = MathF.Sqrt(1f - t1 * t1);
    }

    /// <summary>Circular ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutCirc(in float t, out float result)
    {
        float t2 = 2f * t;
        if (t2 < 1f)
        {
            result = -0.5f * (MathF.Sqrt(1f - t2 * t2) - 1f);
        }
        else
        {
            t2 -= 2f;
            result = 0.5f * (MathF.Sqrt(1f - t2 * t2) + 1f);
        }
    }

    /// <summary>Back ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InBack(in float t, out float result)
    {
        const float s = 1.70158f;
        result = t * t * ((s + 1f) * t - s);
    }

    /// <summary>Back ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutBack(in float t, out float result)
    {
        const float s = 1.70158f;
        float t1 = t - 1f;
        result = t1 * t1 * ((s + 1f) * t1 + s) + 1f;
    }

    /// <summary>Back ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutBack(in float t, out float result)
    {
        const float s = 1.70158f * 1.525f;
        float t2 = 2f * t;
        if (t2 < 1f)
        {
            result = 0.5f * (t2 * t2 * ((s + 1f) * t2 - s));
        }
        else
        {
            t2 -= 2f;
            result = 0.5f * (t2 * t2 * ((s + 1f) * t2 + s) + 2f);
        }
    }

    /// <summary>Elastic ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InElastic(in float t, out float result)
    {
        if (t == 0f) { result = 0f; return; }
        if (t == 1f) { result = 1f; return; }

        float t1 = t - 1f;
        result = -MathF.Pow(2f, 10f * t1) * MathF.Sin((t1 - 1.1f / 4f) * (2f * PI) / 0.3f);
    }

    /// <summary>Elastic ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutElastic(in float t, out float result)
    {
        if (t == 0f) { result = 0f; return; }
        if (t == 1f) { result = 1f; return; }

        result = MathF.Pow(2f, -10f * t) * MathF.Sin((t - 0.3f / 4f) * (2f * PI) / 0.3f) + 1f;
    }

    /// <summary>Elastic ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutElastic(in float t, out float result)
    {
        if (t == 0f) { result = 0f; return; }
        if (t == 1f) { result = 1f; return; }

        float t2 = 2f * t;
        if (t2 < 1f)
        {
            float t1 = t2 - 1f;
            result = -0.5f * MathF.Pow(2f, 10f * t1) * MathF.Sin((t1 - 0.45f / 4f) * (2f * PI) / 0.45f);
        }
        else
        {
            float t1 = t2 - 1f;
            result = MathF.Pow(2f, -10f * t1) * MathF.Sin((t1 - 0.45f / 4f) * (2f * PI) / 0.45f) * 0.5f + 1f;
        }
    }

    /// <summary>Bounce ease-in.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InBounce(in float t, out float result)
    {
        float t1 = 1f - t;
        OutBounce(in t1, out float outRes);
        result = 1f - outRes;
    }

    /// <summary>Bounce ease-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutBounce(in float t, out float result)
    {
        if (t < (1f / 2.75f))
        {
            result = 7.5625f * t * t;
        }
        else if (t < (2f / 2.75f))
        {
            float t1 = t - (1.5f / 2.75f);
            result = 7.5625f * t1 * t1 + 0.75f;
        }
        else if (t < (2.5f / 2.75f))
        {
            float t1 = t - (2.25f / 2.75f);
            result = 7.5625f * t1 * t1 + 0.9375f;
        }
        else
        {
            float t1 = t - (2.625f / 2.75f);
            result = 7.5625f * t1 * t1 + 0.984375f;
        }
    }

    /// <summary>Bounce ease-in-out.</summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutBounce(in float t, out float result)
    {
        if (t < 0.5f)
        {
            float t2 = t * 2f;
            InBounce(in t2, out float res);
            result = res * 0.5f;
        }
        else
        {
            float t2 = t * 2f - 1f;
            OutBounce(in t2, out float res);
            result = res * 0.5f + 0.5f;
        }
    }

    /// <summary>
    ///     Interpolates between start and end using value t.
    /// </summary>
    /// <param name="start">Start value.</param>
    /// <param name="end">End value.</param>
    /// <param name="t">Interpolation factor [0, 1].</param>
    /// <param name="result">Interpolated result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Lerp(in float start, in float end, in float t, out float result)
    {
        result = start + (end - start) * t;
    }
}
