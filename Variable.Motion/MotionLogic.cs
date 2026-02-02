namespace Variable.Motion;

/// <summary>
///     Pure logic for motion, easing, and spring physics.
///     Contains standard Robert Penner easing functions and spring integration.
/// </summary>
public static class MotionLogic
{
    private const float PI = MathF.PI;
    private const float HalfPI = MathF.PI / 2f;

    /// <summary>
    ///     Interpolates between start and end based on time t (0 to 1) and easing type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Interpolate(float start, float end, float t, EasingType easing)
    {
        var ratio = Ease(t, easing);
        return Lerp(start, end, ratio);
    }

    /// <summary>
    ///     Linear interpolation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    ///     Calculates the eased value of t based on the easing type.
    /// </summary>
    /// <param name="t">Normalized time (0 to 1).</param>
    /// <param name="type">The easing function to use.</param>
    /// <returns>The eased value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ease(float t, EasingType type)
    {
        // Clamp t to 0-1 range to avoid wild extrapolation unless intentional
        // Usually tweens clamp time. We'll assume t is handled by caller, but safe easing expects 0-1.
        // Let's rely on the formula.

        switch (type)
        {
            case EasingType.Linear: return t;
            case EasingType.QuadIn: return t * t;
            case EasingType.QuadOut: return t * (2f - t);
            case EasingType.QuadInOut: return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;

            case EasingType.CubicIn: return t * t * t;
            case EasingType.CubicOut: return (--t) * t * t + 1f;
            case EasingType.CubicInOut: return t < 0.5f ? 4f * t * t * t : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;

            case EasingType.QuartIn: return t * t * t * t;
            case EasingType.QuartOut: return 1f - (--t) * t * t * t;
            case EasingType.QuartInOut: return t < 0.5f ? 8f * t * t * t * t : 1f - 8f * (--t) * t * t * t;

            case EasingType.QuintIn: return t * t * t * t * t;
            case EasingType.QuintOut: return 1f + (--t) * t * t * t * t;
            case EasingType.QuintInOut: return t < 0.5f ? 16f * t * t * t * t * t : 1f + 16f * (--t) * t * t * t * t;

            case EasingType.SineIn: return 1f - MathF.Cos(t * HalfPI);
            case EasingType.SineOut: return MathF.Sin(t * HalfPI);
            case EasingType.SineInOut: return -(MathF.Cos(PI * t) - 1f) / 2f;

            case EasingType.ExpoIn: return t == 0f ? 0f : MathF.Pow(2f, 10f * (t - 1f));
            case EasingType.ExpoOut: return t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
            case EasingType.ExpoInOut:
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                if ((t *= 2f) < 1f) return 0.5f * MathF.Pow(2f, 10f * (t - 1f));
                return 0.5f * (-MathF.Pow(2f, -10f * --t) + 2f);

            case EasingType.CircIn: return 1f - MathF.Sqrt(1f - t * t);
            case EasingType.CircOut: return MathF.Sqrt(1f - (--t) * t);
            case EasingType.CircInOut:
                if ((t *= 2f) < 1f) return -0.5f * (MathF.Sqrt(1f - t * t) - 1f);
                return 0.5f * (MathF.Sqrt(1f - (t -= 2f) * t) + 1f);

            case EasingType.BackIn:
                const float s1 = 1.70158f;
                const float s2 = s1 + 1f;
                return s2 * t * t * t - s1 * t * t;
            case EasingType.BackOut:
                const float s3 = 1.70158f;
                const float s4 = s3 + 1f;
                return 1f + s4 * MathF.Pow(t - 1f, 3f) + s3 * MathF.Pow(t - 1f, 2f);
            case EasingType.BackInOut:
                const float s5 = 1.70158f * 1.525f;
                if ((t *= 2f) < 1f) return 0.5f * (t * t * ((s5 + 1f) * t - s5));
                return 0.5f * ((t -= 2f) * t * ((s5 + 1f) * t + s5) + 2f);

            case EasingType.ElasticIn:
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                return -MathF.Pow(2f, 10f * (t -= 1f)) * MathF.Sin((t * 10f - 0.75f) * (2f * PI) / 3f);
            case EasingType.ElasticOut:
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                return MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * (2f * PI) / 3f) + 1f;
            case EasingType.ElasticInOut:
                const float c5 = (2f * PI) / 4.5f;
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                if ((t *= 2f) < 1f) return -0.5f * (MathF.Pow(2f, 10f * (t -= 1f)) * MathF.Sin((t * 10f - 0.75f) * c5));
                return MathF.Pow(2f, -10f * (t -= 1f)) * MathF.Sin((t * 10f - 0.75f) * c5) * 0.5f + 1f;

            case EasingType.BounceIn: return 1f - Ease(1f - t, EasingType.BounceOut);
            case EasingType.BounceOut:
                const float n1 = 7.5625f;
                const float d1 = 2.75f;
                if (t < 1f / d1) return n1 * t * t;
                if (t < 2f / d1) return n1 * (t -= 1.5f / d1) * t + 0.75f;
                if (t < 2.5f / d1) return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            case EasingType.BounceInOut:
                return t < 0.5f
                    ? (1f - Ease(1f - 2f * t, EasingType.BounceOut)) * 0.5f
                    : (1f + Ease(2f * t - 1f, EasingType.BounceOut)) * 0.5f;

            default: return t;
        }
    }

    /// <summary>
    ///     Integrates a damped spring using semi-implicit Euler integration.
    /// </summary>
    /// <param name="current">Current position.</param>
    /// <param name="target">Target position.</param>
    /// <param name="velocity">Current velocity (ref to update).</param>
    /// <param name="stiffness">Spring stiffness (k). Higher = faster oscillation.</param>
    /// <param name="damping">Spring damping (c). Higher = less oscillation.</param>
    /// <param name="dt">Delta time.</param>
    /// <param name="newCurrent">Output new position.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IntegrateSpring(
        float current,
        float target,
        ref float velocity,
        float stiffness,
        float damping,
        float dt,
        out float newCurrent)
    {
        // F = -k * x - c * v
        // a = F / m (m=1)
        // a = -k * (current - target) - c * velocity

        float displacement = current - target;
        float acceleration = -stiffness * displacement - damping * velocity;

        // Semi-implicit Euler
        velocity += acceleration * dt;
        newCurrent = current + velocity * dt;
    }
}
