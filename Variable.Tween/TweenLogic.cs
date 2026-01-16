namespace Variable.Tween;

/// <summary>
///     Core logic for tweening and easing functions.
///     Contains pure, stateless, zero-allocation algorithms.
/// </summary>
public static class TweenLogic
{
    private const float PI = MathF.PI;
    private const float C1 = 1.70158f;
    private const float C2 = C1 * 1.525f;
    private const float C3 = C1 + 1f;
    private const float C4 = (2f * PI) / 3f;
    private const float C5 = (2f * PI) / 4.5f;
    private const float N1 = 7.5625f;
    private const float D1 = 2.75f;

    /// <summary>
    ///     Advances the tween state by a delta time.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The target value.</param>
    /// <param name="duration">The total duration.</param>
    /// <param name="elapsed">The currently elapsed time.</param>
    /// <param name="easing">The easing function to apply.</param>
    /// <param name="dt">The time to advance by.</param>
    /// <param name="newElapsed">The updated elapsed time.</param>
    /// <param name="newCurrent">The updated current value.</param>
    /// <param name="isFinished">True if the tween has completed.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(
        float start,
        float end,
        float duration,
        float elapsed,
        EasingType easing,
        float dt,
        out float newElapsed,
        out float newCurrent,
        out bool isFinished)
    {
        if (duration <= 0f)
        {
            newElapsed = duration;
            newCurrent = end;
            isFinished = true;
            return;
        }

        newElapsed = elapsed + dt;

        if (newElapsed >= duration)
        {
            newElapsed = duration;
            newCurrent = end;
            isFinished = true;
            return;
        }
        else if (newElapsed < 0f)
        {
            newElapsed = 0f;
            newCurrent = start;
            isFinished = false;
            return;
        }

        isFinished = false;
        float t = newElapsed / duration;
        Evaluate(t, easing, out float easedT);

        newCurrent = start + (end - start) * easedT;
    }

    /// <summary>
    ///     Evaluates an easing function at a specific time ratio (0 to 1).
    /// </summary>
    /// <param name="t">The time ratio (0 to 1).</param>
    /// <param name="type">The easing type.</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(float t, EasingType type, out float result)
    {
        switch (type)
        {
            case EasingType.Linear:
                result = t;
                break;
            case EasingType.QuadIn:
                result = t * t;
                break;
            case EasingType.QuadOut:
                result = 1f - (1f - t) * (1f - t);
                break;
            case EasingType.QuadInOut:
                result = t < 0.5f ? 2f * t * t : 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f;
                break;
            case EasingType.CubicIn:
                result = t * t * t;
                break;
            case EasingType.CubicOut:
                result = 1f - MathF.Pow(1f - t, 3f);
                break;
            case EasingType.CubicInOut:
                result = t < 0.5f ? 4f * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 3f) / 2f;
                break;
            case EasingType.QuartIn:
                result = t * t * t * t;
                break;
            case EasingType.QuartOut:
                result = 1f - MathF.Pow(1f - t, 4f);
                break;
            case EasingType.QuartInOut:
                result = t < 0.5f ? 8f * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 4f) / 2f;
                break;
            case EasingType.QuintIn:
                result = t * t * t * t * t;
                break;
            case EasingType.QuintOut:
                result = 1f - MathF.Pow(1f - t, 5f);
                break;
            case EasingType.QuintInOut:
                result = t < 0.5f ? 16f * t * t * t * t * t : 1f - MathF.Pow(-2f * t + 2f, 5f) / 2f;
                break;
            case EasingType.SineIn:
                result = 1f - MathF.Cos((t * PI) / 2f);
                break;
            case EasingType.SineOut:
                result = MathF.Sin((t * PI) / 2f);
                break;
            case EasingType.SineInOut:
                result = -(MathF.Cos(PI * t) - 1f) / 2f;
                break;
            case EasingType.ExpoIn:
                result = t == 0f ? 0f : MathF.Pow(2f, 10f * t - 10f);
                break;
            case EasingType.ExpoOut:
                result = t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
                break;
            case EasingType.ExpoInOut:
                if (t == 0f) result = 0f;
                else if (t == 1f) result = 1f;
                else if (t < 0.5f) result = MathF.Pow(2f, 20f * t - 10f) / 2f;
                else result = (2f - MathF.Pow(2f, -20f * t + 10f)) / 2f;
                break;
            case EasingType.CircIn:
                result = 1f - MathF.Sqrt(1f - MathF.Pow(t, 2f));
                break;
            case EasingType.CircOut:
                result = MathF.Sqrt(1f - MathF.Pow(t - 1f, 2f));
                break;
            case EasingType.CircInOut:
                result = t < 0.5f
                    ? (1f - MathF.Sqrt(1f - MathF.Pow(2f * t, 2f))) / 2f
                    : (MathF.Sqrt(1f - MathF.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
                break;
            case EasingType.BackIn:
                result = C3 * t * t * t - C1 * t * t;
                break;
            case EasingType.BackOut:
                result = 1f + C3 * MathF.Pow(t - 1f, 3f) + C1 * MathF.Pow(t - 1f, 2f);
                break;
            case EasingType.BackInOut:
                float t2 = t * 2f;
                if (t < 0.5f)
                {
                    result = (MathF.Pow(t2, 2f) * ((C2 + 1f) * t2 - C2)) / 2f;
                }
                else
                {
                    t2 -= 2f;
                    result = (MathF.Pow(t2, 2f) * ((C2 + 1f) * t2 + C2) + 2f) / 2f;
                }
                break;
            case EasingType.ElasticIn:
                if (t == 0f) result = 0f;
                else if (t == 1f) result = 1f;
                else result = -MathF.Pow(2f, 10f * t - 10f) * MathF.Sin((t * 10f - 10.75f) * C4);
                break;
            case EasingType.ElasticOut:
                if (t == 0f) result = 0f;
                else if (t == 1f) result = 1f;
                else result = MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * C4) + 1f;
                break;
            case EasingType.ElasticInOut:
                if (t == 0f) result = 0f;
                else if (t == 1f) result = 1f;
                else if (t < 0.5f)
                    result = -(MathF.Pow(2f, 20f * t - 10f) * MathF.Sin((20f * t - 11.125f) * C5)) / 2f;
                else
                    result = (MathF.Pow(2f, -20f * t + 10f) * MathF.Sin((20f * t - 11.125f) * C5)) / 2f + 1f;
                break;
            case EasingType.BounceIn:
                BounceOut(1f - t, out float bOut);
                result = 1f - bOut;
                break;
            case EasingType.BounceOut:
                BounceOut(t, out result);
                break;
            case EasingType.BounceInOut:
                if (t < 0.5f)
                {
                    BounceOut(1f - 2f * t, out float bo);
                    result = (1f - bo) / 2f;
                }
                else
                {
                    BounceOut(2f * t - 1f, out float bo);
                    result = (1f + bo) / 2f;
                }
                break;
            default:
                result = t;
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void BounceOut(float t, out float result)
    {
        if (t < 1f / D1)
        {
            result = N1 * t * t;
        }
        else if (t < 2f / D1)
        {
            t -= 1.5f / D1;
            result = N1 * t * t + 0.75f;
        }
        else if (t < 2.5f / D1)
        {
            t -= 2.25f / D1;
            result = N1 * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / D1;
            result = N1 * t * t + 0.984375f;
        }
    }
}
