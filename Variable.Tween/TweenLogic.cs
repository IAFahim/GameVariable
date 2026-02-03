namespace Variable.Tween;

/// <summary>
/// Core logic for tweening. Pure, stateless, and allocation-free.
/// </summary>
public static class TweenLogic
{
    private const float PI = MathF.PI;
    private const float HALF_PI = MathF.PI / 2f;

    /// <summary>
    /// Advances the time accumulator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(in float currentElapsed, in float duration, in float deltaTime, out float newElapsed, out bool isComplete)
    {
        newElapsed = currentElapsed + deltaTime;
        if (newElapsed >= duration)
        {
            newElapsed = duration;
            isComplete = true;
        }
        else
        {
            isComplete = false;
        }
    }

    /// <summary>
    /// Calculates the normalized time (0 to 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetNormalizedTime(in float elapsed, in float duration, out float t)
    {
        if (duration <= 0f)
        {
            t = 1f;
            return;
        }
        t = Math.Clamp(elapsed / duration, 0f, 1f);
    }

    /// <summary>
    /// Linearly interpolates between a and b by t.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Lerp(in float a, in float b, in float t, out float result)
    {
        result = a + (b - a) * t;
    }

    /// <summary>
    /// Evaluates the tween value based on parameters.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(in float start, in float end, in float t, in EasingType easing, out float result)
    {
        float easedT;
        ApplyEasing(in t, in easing, out easedT);
        Lerp(in start, in end, in easedT, out result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ApplyEasing(in float t, in EasingType easing, out float result)
    {
        switch (easing)
        {
            case EasingType.Linear: result = t; break;
            case EasingType.QuadIn: result = t * t; break;
            case EasingType.QuadOut: result = t * (2f - t); break;
            case EasingType.QuadInOut:
                result = t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
                break;
            case EasingType.CubicIn: result = t * t * t; break;
            case EasingType.CubicOut:
                float f = t - 1f;
                result = f * f * f + 1f;
                break;
            case EasingType.CubicInOut:
                result = t < 0.5f ? 4f * t * t * t : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;
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
            case EasingType.BounceIn:
                BounceOut(1f - t, out float bounceOut);
                result = 1f - bounceOut;
                break;
            case EasingType.BounceOut:
                BounceOut(in t, out result);
                break;
            case EasingType.BounceInOut:
                if (t < 0.5f)
                {
                    BounceOut(1f - t * 2f, out float bo);
                    result = (1f - bo) * 0.5f;
                }
                else
                {
                    BounceOut(t * 2f - 1f, out float bo);
                    result = bo * 0.5f + 0.5f;
                }
                break;
            case EasingType.ElasticIn:
                if (t == 0f) { result = 0f; return; }
                if (t == 1f) { result = 1f; return; }
                result = -MathF.Pow(2f, 10f * (t - 1f)) * MathF.Sin((t - 1.1f) * (2f * PI) / 0.4f);
                break;
            case EasingType.ElasticOut:
                if (t == 0f) { result = 0f; return; }
                if (t == 1f) { result = 1f; return; }
                result = MathF.Pow(2f, -10f * t) * MathF.Sin((t - 0.1f) * (2f * PI) / 0.4f) + 1f;
                break;
            case EasingType.ElasticInOut:
                 if (t == 0f) { result = 0f; return; }
                 if (t == 1f) { result = 1f; return; }
                 if (t < 0.5f)
                 {
                     result = -(MathF.Pow(2f, 20f * t - 10f) * MathF.Sin((20f * t - 11.125f) * (2f * PI) / 4.5f)) / 2f;
                 }
                 else
                 {
                     result = (MathF.Pow(2f, -20f * t + 10f) * MathF.Sin((20f * t - 11.125f) * (2f * PI) / 4.5f)) / 2f + 1f;
                 }
                 break;
            default: result = t; break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void BounceOut(in float t, out float result)
    {
        if (t < 1f / 2.75f)
        {
            result = 7.5625f * t * t;
        }
        else if (t < 2f / 2.75f)
        {
            float f = t - 1.5f / 2.75f;
            result = 7.5625f * f * f + 0.75f;
        }
        else if (t < 2.5f / 2.75f)
        {
            float f = t - 2.25f / 2.75f;
            result = 7.5625f * f * f + 0.9375f;
        }
        else
        {
            float f = t - 2.625f / 2.75f;
            result = 7.5625f * f * f + 0.984375f;
        }
    }
}
