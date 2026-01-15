using System;
using System.Runtime.CompilerServices;

namespace Variable.Tween;

/// <summary>
///     Core logic for tweening and easing functions.
///     Stateless, zero-allocation, and Burst-compatible.
/// </summary>
public static class TweenLogic
{
    private const float PI = MathF.PI;
    private const float HalfPI = MathF.PI / 2f;

    /// <summary>
    ///     Evaluates a tween at a specific ratio.
    /// </summary>
    /// <param name="start">The start value.</param>
    /// <param name="end">The end value.</param>
    /// <param name="ratio">The normalized time ratio (0.0 to 1.0).</param>
    /// <param name="easing">The easing function to apply.</param>
    /// <param name="result">The interpolated value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(float start, float end, float ratio, EasingType easing, out float result)
    {
        Ease(ratio, easing, out var t);
        result = start + (end - start) * t;
    }

    /// <summary>
    ///     Applies an easing function to a normalized value (0.0 to 1.0).
    /// </summary>
    /// <param name="t">The input value (0.0 to 1.0).</param>
    /// <param name="easing">The easing type.</param>
    /// <param name="result">The eased value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Ease(float t, EasingType easing, out float result)
    {
        switch (easing)
        {
            case EasingType.Linear:
                result = t;
                break;
            case EasingType.QuadIn:
                result = t * t;
                break;
            case EasingType.QuadOut:
                result = t * (2f - t);
                break;
            case EasingType.QuadInOut:
                if ((t *= 2f) < 1f) result = 0.5f * t * t;
                else result = -0.5f * ((--t) * (t - 2f) - 1f);
                break;
            case EasingType.CubicIn:
                result = t * t * t;
                break;
            case EasingType.CubicOut:
                result = (--t) * t * t + 1f;
                break;
            case EasingType.CubicInOut:
                if ((t *= 2f) < 1f) result = 0.5f * t * t * t;
                else result = 0.5f * ((t -= 2f) * t * t + 2f);
                break;
            case EasingType.QuartIn:
                result = t * t * t * t;
                break;
            case EasingType.QuartOut:
                result = 1f - (--t) * t * t * t;
                break;
            case EasingType.QuartInOut:
                if ((t *= 2f) < 1f) result = 0.5f * t * t * t * t;
                else result = -0.5f * ((t -= 2f) * t * t * t - 2f);
                break;
            case EasingType.QuintIn:
                result = t * t * t * t * t;
                break;
            case EasingType.QuintOut:
                result = (--t) * t * t * t * t + 1f;
                break;
            case EasingType.QuintInOut:
                if ((t *= 2f) < 1f) result = 0.5f * t * t * t * t * t;
                else result = 0.5f * ((t -= 2f) * t * t * t * t + 2f);
                break;
            case EasingType.SineIn:
                result = 1f - MathF.Cos(t * HalfPI);
                break;
            case EasingType.SineOut:
                result = MathF.Sin(t * HalfPI);
                break;
            case EasingType.SineInOut:
                result = -0.5f * (MathF.Cos(PI * t) - 1f);
                break;
            case EasingType.ExpoIn:
                result = t == 0f ? 0f : MathF.Pow(2f, 10f * (t - 1f));
                break;
            case EasingType.ExpoOut:
                result = t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
                break;
            case EasingType.ExpoInOut:
                if (t == 0f) { result = 0f; return; }
                if (t == 1f) { result = 1f; return; }
                if ((t *= 2f) < 1f) result = 0.5f * MathF.Pow(2f, 10f * (t - 1f));
                else result = 0.5f * (-MathF.Pow(2f, -10f * --t) + 2f);
                break;
            case EasingType.CircIn:
                result = 1f - MathF.Sqrt(1f - t * t);
                break;
            case EasingType.CircOut:
                result = MathF.Sqrt(1f - (--t) * t);
                break;
            case EasingType.CircInOut:
                if ((t *= 2f) < 1f) result = -0.5f * (MathF.Sqrt(1f - t * t) - 1f);
                else result = 0.5f * (MathF.Sqrt(1f - (t -= 2f) * t) + 1f);
                break;
            case EasingType.BackIn:
                {
                    const float s = 1.70158f;
                    result = t * t * ((s + 1f) * t - s);
                }
                break;
            case EasingType.BackOut:
                {
                    const float s = 1.70158f;
                    result = (--t) * t * ((s + 1f) * t + s) + 1f;
                }
                break;
            case EasingType.BackInOut:
                {
                    const float s = 1.70158f * 1.525f;
                    if ((t *= 2f) < 1f) result = 0.5f * (t * t * ((s + 1f) * t - s));
                    else result = 0.5f * ((t -= 2f) * t * ((s + 1f) * t + s) + 2f);
                }
                break;
            case EasingType.ElasticIn:
                if (t == 0f) { result = 0f; return; }
                if (t == 1f) { result = 1f; return; }
                result = -MathF.Pow(2f, 10f * (t -= 1f)) * MathF.Sin((t - 0.3f / 4f) * (2f * PI) / 0.3f);
                break;
            case EasingType.ElasticOut:
                if (t == 0f) { result = 0f; return; }
                if (t == 1f) { result = 1f; return; }
                result = MathF.Pow(2f, -10f * t) * MathF.Sin((t - 0.3f / 4f) * (2f * PI) / 0.3f) + 1f;
                break;
            case EasingType.ElasticInOut:
                if (t == 0f) { result = 0f; return; }
                if ((t *= 2f) == 2f) { result = 1f; return; }
                if (t < 1f) result = -0.5f * (MathF.Pow(2f, 10f * (t -= 1f)) * MathF.Sin((t - 0.45f / 4f) * (2f * PI) / 0.45f));
                else result = MathF.Pow(2f, -10f * (t -= 1f)) * MathF.Sin((t - 0.45f / 4f) * (2f * PI) / 0.45f) * 0.5f + 1f;
                break;
            case EasingType.BounceIn:
                BounceOut(1f - t, out var bo);
                result = 1f - bo;
                break;
            case EasingType.BounceOut:
                BounceOut(t, out result);
                break;
            case EasingType.BounceInOut:
                if (t < 0.5f)
                {
                    BounceOut(1f - t * 2f, out var boIn);
                    result = (1f - boIn) * 0.5f;
                }
                else
                {
                    BounceOut(t * 2f - 1f, out var boOut);
                    result = boOut * 0.5f + 0.5f;
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
        if (t < 1f / 2.75f)
        {
            result = 7.5625f * t * t;
        }
        else if (t < 2f / 2.75f)
        {
            result = 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
        }
        else if (t < 2.5f / 2.75f)
        {
            result = 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
        }
        else
        {
            result = 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }
    }
}
