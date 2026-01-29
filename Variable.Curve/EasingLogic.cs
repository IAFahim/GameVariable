namespace Variable.Curve;

/// <summary>
///     Pure logic for standard easing functions.
///     All methods are stateless, Burst-compatible, and use the 'void + out' pattern.
/// </summary>
public static class EasingLogic
{
    /// <summary>Linear easing (no change).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Linear(in float progress, out float result)
    {
        result = progress;
    }

    #region Sine

    /// <summary>Sine In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SineIn(in float progress, out float result)
    {
        result = 1f - MathF.Cos((progress * MathF.PI) / 2f);
    }

    /// <summary>Sine Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SineOut(in float progress, out float result)
    {
        result = MathF.Sin((progress * MathF.PI) / 2f);
    }

    /// <summary>Sine InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SineInOut(in float progress, out float result)
    {
        result = -(MathF.Cos(MathF.PI * progress) - 1f) / 2f;
    }

    #endregion

    #region Quad

    /// <summary>Quadratic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuadIn(in float progress, out float result)
    {
        result = progress * progress;
    }

    /// <summary>Quadratic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuadOut(in float progress, out float result)
    {
        result = 1f - (1f - progress) * (1f - progress);
    }

    /// <summary>Quadratic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuadInOut(in float progress, out float result)
    {
        result = progress < 0.5f
            ? 2f * progress * progress
            : 1f - MathF.Pow(-2f * progress + 2f, 2f) / 2f;
    }

    #endregion

    #region Cubic

    /// <summary>Cubic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CubicIn(in float progress, out float result)
    {
        result = progress * progress * progress;
    }

    /// <summary>Cubic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CubicOut(in float progress, out float result)
    {
        result = 1f - MathF.Pow(1f - progress, 3f);
    }

    /// <summary>Cubic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CubicInOut(in float progress, out float result)
    {
        result = progress < 0.5f
            ? 4f * progress * progress * progress
            : 1f - MathF.Pow(-2f * progress + 2f, 3f) / 2f;
    }

    #endregion

    #region Back

    /// <summary>Back In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BackIn(in float progress, out float result)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        result = c3 * progress * progress * progress - c1 * progress * progress;
    }

    /// <summary>Back Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BackOut(in float progress, out float result)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        result = 1f + c3 * MathF.Pow(progress - 1f, 3f) + c1 * MathF.Pow(progress - 1f, 2f);
    }

    /// <summary>Back InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BackInOut(in float progress, out float result)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        result = progress < 0.5f
            ? (MathF.Pow(2f * progress, 2f) * ((c2 + 1f) * 2f * progress - c2)) / 2f
            : (MathF.Pow(2f * progress - 2f, 2f) * ((c2 + 1f) * (progress * 2f - 2f) + c2) + 2f) / 2f;
    }

    #endregion

    #region Elastic

    /// <summary>Elastic In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ElasticIn(in float progress, out float result)
    {
        if (progress == 0f) { result = 0f; return; }
        if (progress == 1f) { result = 1f; return; }

        result = -MathF.Pow(2f, 10f * progress - 10f) * MathF.Sin((progress * 10f - 10.75f) * ((2f * MathF.PI) / 3f));
    }

    /// <summary>Elastic Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ElasticOut(in float progress, out float result)
    {
        if (progress == 0f) { result = 0f; return; }
        if (progress == 1f) { result = 1f; return; }

        result = MathF.Pow(2f, -10f * progress) * MathF.Sin((progress * 10f - 0.75f) * ((2f * MathF.PI) / 3f)) + 1f;
    }

    /// <summary>Elastic InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ElasticInOut(in float progress, out float result)
    {
        if (progress == 0f) { result = 0f; return; }
        if (progress == 1f) { result = 1f; return; }

        const float c5 = (2f * MathF.PI) / 4.5f;

        result = progress < 0.5f
            ? -(MathF.Pow(2f, 20f * progress - 10f) * MathF.Sin((20f * progress - 11.125f) * c5)) / 2f
            : (MathF.Pow(2f, -20f * progress + 10f) * MathF.Sin((20f * progress - 11.125f) * c5)) / 2f + 1f;
    }

    #endregion

    #region Bounce

    /// <summary>Bounce Out easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BounceOut(in float progress, out float result)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (progress < 1f / d1)
        {
            result = n1 * progress * progress;
        }
        else if (progress < 2f / d1)
        {
            float p = progress - 1.5f / d1;
            result = n1 * p * p + 0.75f;
        }
        else if (progress < 2.5f / d1)
        {
            float p = progress - 2.25f / d1;
            result = n1 * p * p + 0.9375f;
        }
        else
        {
            float p = progress - 2.625f / d1;
            result = n1 * p * p + 0.984375f;
        }
    }

    /// <summary>Bounce In easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BounceIn(in float progress, out float result)
    {
        BounceOut(1f - progress, out float bounceOut);
        result = 1f - bounceOut;
    }

    /// <summary>Bounce InOut easing.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BounceInOut(in float progress, out float result)
    {
        if (progress < 0.5f)
        {
            BounceOut(1f - 2f * progress, out float bo);
            result = (1f - bo) * 0.5f;
        }
        else
        {
            BounceOut(2f * progress - 1f, out float bo);
            result = (1f + bo) * 0.5f;
        }
    }

    #endregion
}
