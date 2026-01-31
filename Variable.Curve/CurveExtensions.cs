namespace Variable.Curve;

/// <summary>
///     Fluent API for Curve operations.
/// </summary>
public static class CurveExtensions
{
    #region BezierCurve Extensions

    /// <summary>Evaluates the Bezier curve at the given progress.</summary>
    /// <param name="curve">The curve to evaluate.</param>
    /// <param name="progress">The progress (0-1).</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Evaluate(in this BezierCurve curve, float progress)
    {
        BezierLogic.Evaluate(in curve.Start, in curve.Control1, in curve.Control2, in curve.End, in progress, out float result);
        return result;
    }

    #endregion

    #region Float Easing Extensions

    /// <summary>Applies Sine In easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseSineIn(this float progress)
    {
        EasingLogic.SineIn(in progress, out float result);
        return result;
    }

    /// <summary>Applies Sine Out easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseSineOut(this float progress)
    {
        EasingLogic.SineOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Sine InOut easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseSineInOut(this float progress)
    {
        EasingLogic.SineInOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Quadratic In easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseQuadIn(this float progress)
    {
        EasingLogic.QuadIn(in progress, out float result);
        return result;
    }

    /// <summary>Applies Quadratic Out easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseQuadOut(this float progress)
    {
        EasingLogic.QuadOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Quadratic InOut easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseQuadInOut(this float progress)
    {
        EasingLogic.QuadInOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Cubic In easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseCubicIn(this float progress)
    {
        EasingLogic.CubicIn(in progress, out float result);
        return result;
    }

    /// <summary>Applies Cubic Out easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseCubicOut(this float progress)
    {
        EasingLogic.CubicOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Cubic InOut easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseCubicInOut(this float progress)
    {
        EasingLogic.CubicInOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Back In easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseBackIn(this float progress)
    {
        EasingLogic.BackIn(in progress, out float result);
        return result;
    }

    /// <summary>Applies Back Out easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseBackOut(this float progress)
    {
        EasingLogic.BackOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Back InOut easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseBackInOut(this float progress)
    {
        EasingLogic.BackInOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Elastic In easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseElasticIn(this float progress)
    {
        EasingLogic.ElasticIn(in progress, out float result);
        return result;
    }

    /// <summary>Applies Elastic Out easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseElasticOut(this float progress)
    {
        EasingLogic.ElasticOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Elastic InOut easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseElasticInOut(this float progress)
    {
        EasingLogic.ElasticInOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Bounce In easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseBounceIn(this float progress)
    {
        EasingLogic.BounceIn(in progress, out float result);
        return result;
    }

    /// <summary>Applies Bounce Out easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseBounceOut(this float progress)
    {
        EasingLogic.BounceOut(in progress, out float result);
        return result;
    }

    /// <summary>Applies Bounce InOut easing to the value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseBounceInOut(this float progress)
    {
        EasingLogic.BounceInOut(in progress, out float result);
        return result;
    }

    #endregion
}
