namespace Variable.Curve;

/// <summary>
///     Stateless logic for evaluating easing functions.
///     Implementations based on Robert Penner's Easing Functions.
/// </summary>
public static class EasingLogic
{
    private const float PI = MathF.PI;
    private const float HalfPI = MathF.PI / 2f;
    private const float C1 = 1.70158f;
    private const float C2 = C1 * 1.525f;
    private const float C3 = C1 + 1f;
    private const float C4 = (2f * PI) / 3f;
    private const float C5 = (2f * PI) / 4.5f;
    private const float N1 = 7.5625f;
    private const float D1 = 2.75f;

    /// <summary>
    ///     Evaluates the specified easing function at time t.
    /// </summary>
    /// <param name="t">Normalized time (usually 0 to 1).</param>
    /// <param name="type">The easing type.</param>
    /// <param name="result">The resulting value.</param>
    public static void Evaluate(float t, EaseType type, out float result)
    {
        switch (type)
        {
            case EaseType.Linear: Linear(t, out result); break;
            case EaseType.InQuad: InQuad(t, out result); break;
            case EaseType.OutQuad: OutQuad(t, out result); break;
            case EaseType.InOutQuad: InOutQuad(t, out result); break;
            case EaseType.InCubic: InCubic(t, out result); break;
            case EaseType.OutCubic: OutCubic(t, out result); break;
            case EaseType.InOutCubic: InOutCubic(t, out result); break;
            case EaseType.InQuart: InQuart(t, out result); break;
            case EaseType.OutQuart: OutQuart(t, out result); break;
            case EaseType.InOutQuart: InOutQuart(t, out result); break;
            case EaseType.InQuint: InQuint(t, out result); break;
            case EaseType.OutQuint: OutQuint(t, out result); break;
            case EaseType.InOutQuint: InOutQuint(t, out result); break;
            case EaseType.InSine: InSine(t, out result); break;
            case EaseType.OutSine: OutSine(t, out result); break;
            case EaseType.InOutSine: InOutSine(t, out result); break;
            case EaseType.InExpo: InExpo(t, out result); break;
            case EaseType.OutExpo: OutExpo(t, out result); break;
            case EaseType.InOutExpo: InOutExpo(t, out result); break;
            case EaseType.InCirc: InCirc(t, out result); break;
            case EaseType.OutCirc: OutCirc(t, out result); break;
            case EaseType.InOutCirc: InOutCirc(t, out result); break;
            case EaseType.InBack: InBack(t, out result); break;
            case EaseType.OutBack: OutBack(t, out result); break;
            case EaseType.InOutBack: InOutBack(t, out result); break;
            case EaseType.InElastic: InElastic(t, out result); break;
            case EaseType.OutElastic: OutElastic(t, out result); break;
            case EaseType.InOutElastic: InOutElastic(t, out result); break;
            case EaseType.InBounce: InBounce(t, out result); break;
            case EaseType.OutBounce: OutBounce(t, out result); break;
            case EaseType.InOutBounce: InOutBounce(t, out result); break;
            default: Linear(t, out result); break;
        }
    }

    /// <summary>Linear easing (t).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Linear(float t, out float result) => result = t;

    /// <summary>Quadratic ease-in (t^2).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuad(float t, out float result) => result = t * t;

    /// <summary>Quadratic ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuad(float t, out float result) => result = 1f - (1f - t) * (1f - t);

    /// <summary>Quadratic ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuad(float t, out float result)
    {
        result = t < 0.5f ? 2f * t * t : 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;
    }

    /// <summary>Cubic ease-in (t^3).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InCubic(float t, out float result) => result = t * t * t;

    /// <summary>Cubic ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutCubic(float t, out float result) => result = 1f - MathF.Pow(1f - t, 3f);

    /// <summary>Cubic ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutCubic(float t, out float result)
    {
        result = t < 0.5f ? 4f * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 3f) / 2f;
    }

    /// <summary>Quartic ease-in (t^4).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuart(float t, out float result) => result = t * t * t * t;

    /// <summary>Quartic ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuart(float t, out float result) => result = 1f - MathF.Pow(1f - t, 4f);

    /// <summary>Quartic ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuart(float t, out float result)
    {
        result = t < 0.5f ? 8f * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 4f) / 2f;
    }

    /// <summary>Quintic ease-in (t^5).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuint(float t, out float result) => result = t * t * t * t * t;

    /// <summary>Quintic ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuint(float t, out float result) => result = 1f - MathF.Pow(1f - t, 5f);

    /// <summary>Quintic ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuint(float t, out float result)
    {
        result = t < 0.5f ? 16f * t * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 5f) / 2f;
    }

    /// <summary>Sinusoidal ease-in.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InSine(float t, out float result) => result = 1f - MathF.Cos(t * HalfPI);

    /// <summary>Sinusoidal ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutSine(float t, out float result) => result = MathF.Sin(t * HalfPI);

    /// <summary>Sinusoidal ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutSine(float t, out float result) => result = -(MathF.Cos(PI * t) - 1f) / 2f;

    /// <summary>Exponential ease-in.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InExpo(float t, out float result)
    {
        result = t == 0f ? 0f : MathF.Pow(2f, 10f * t - 10f);
    }

    /// <summary>Exponential ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutExpo(float t, out float result)
    {
        result = t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
    }

    /// <summary>Exponential ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutExpo(float t, out float result)
    {
        if (t == 0f) result = 0f;
        else if (t == 1f) result = 1f;
        else if (t < 0.5f) result = MathF.Pow(2f, 20f * t - 10f) / 2f;
        else result = (2f - MathF.Pow(2f, -20f * t + 10f)) / 2f;
    }

    /// <summary>Circular ease-in.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InCirc(float t, out float result) => result = 1f - MathF.Sqrt(1f - MathF.Pow(t, 2f));

    /// <summary>Circular ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutCirc(float t, out float result) => result = MathF.Sqrt(1f - MathF.Pow(t - 1f, 2f));

    /// <summary>Circular ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutCirc(float t, out float result)
    {
        result = t < 0.5f
            ? (1f - MathF.Sqrt(1f - MathF.Pow(2f * t, 2f))) / 2f
            : (MathF.Sqrt(1f - MathF.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
    }

    /// <summary>Back ease-in (overshoots).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InBack(float t, out float result)
    {
        result = C3 * t * t * t - C1 * t * t;
    }

    /// <summary>Back ease-out (overshoots).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutBack(float t, out float result)
    {
        result = 1f + C3 * MathF.Pow(t - 1f, 3f) + C1 * MathF.Pow(t - 1f, 2f);
    }

    /// <summary>Back ease-in-out (overshoots).</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutBack(float t, out float result)
    {
        result = t < 0.5f
            ? (MathF.Pow(2f * t, 2f) * ((C2 + 1f) * 2f * t - C2)) / 2f
            : (MathF.Pow(2f * t - 2f, 2f) * ((C2 + 1f) * (2f * t - 2f) + C2) + 2f) / 2f;
    }

    /// <summary>Elastic ease-in.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InElastic(float t, out float result)
    {
        if (t == 0f) result = 0f;
        else if (t == 1f) result = 1f;
        else result = -MathF.Pow(2f, 10f * t - 10f) * MathF.Sin((t * 10f - 10.75f) * C4);
    }

    /// <summary>Elastic ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutElastic(float t, out float result)
    {
        if (t == 0f) result = 0f;
        else if (t == 1f) result = 1f;
        else result = MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * C4) + 1f;
    }

    /// <summary>Elastic ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutElastic(float t, out float result)
    {
        if (t == 0f) result = 0f;
        else if (t == 1f) result = 1f;
        else if (t < 0.5f)
            result = -(MathF.Pow(2f, 20f * t - 10f) * MathF.Sin((20f * t - 11.125f) * C5)) / 2f;
        else
            result = (MathF.Pow(2f, -20f * t + 10f) * MathF.Sin((20f * t - 11.125f) * C5)) / 2f + 1f;
    }

    /// <summary>Bounce ease-in.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InBounce(float t, out float result)
    {
        OutBounce(1f - t, out var r);
        result = 1f - r;
    }

    /// <summary>Bounce ease-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutBounce(float t, out float result)
    {
        if (t < 1f / D1)
        {
            result = N1 * t * t;
        }
        else if (t < 2f / D1)
        {
            result = N1 * (t -= 1.5f / D1) * t + 0.75f;
        }
        else if (t < 2.5f / D1)
        {
            result = N1 * (t -= 2.25f / D1) * t + 0.9375f;
        }
        else
        {
            result = N1 * (t -= 2.625f / D1) * t + 0.984375f;
        }
    }

    /// <summary>Bounce ease-in-out.</summary>
    /// <param name="t">Time (0-1).</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutBounce(float t, out float result)
    {
        if (t < 0.5f)
        {
            OutBounce(1f - 2f * t, out var r);
            result = (1f - r) * 0.5f;
        }
        else
        {
            OutBounce(2f * t - 1f, out var r);
            result = (1f + r) * 0.5f;
        }
    }
}
