namespace Variable.Curve;

/// <summary>
///     Extension methods for <see cref="CurveFloat"/>.
///     Layer C: The Adapter.
/// </summary>
public static class CurveExtensions
{
    /// <summary>
    ///     Advances the curve's time by deltaTime.
    ///     Clamps CurrentTime to Duration.
    /// </summary>
    /// <param name="curve">The curve instance.</param>
    /// <param name="deltaTime">Time to advance by.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this CurveFloat curve, float deltaTime)
    {
        curve.CurrentTime += deltaTime;
        if (curve.CurrentTime > curve.Duration)
        {
            curve.CurrentTime = curve.Duration;
        }
    }

    /// <summary>
    ///     Gets the current interpolated value of the curve.
    /// </summary>
    /// <param name="curve">The curve instance.</param>
    /// <returns>The calculated value based on easing.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValue(in this CurveFloat curve)
    {
        float t;
        if (curve.Duration <= 0f)
        {
            t = 1f;
        }
        else
        {
            t = curve.CurrentTime / curve.Duration;
        }

        CurveLogic.Evaluate(in t, in curve.Type, out float easedT);
        CurveLogic.Lerp(in curve.StartValue, in curve.EndValue, in easedT, out float result);
        return result;
    }

    /// <summary>
    ///     Gets the normalized progress (0-1) based on time.
    ///     Does not apply easing.
    /// </summary>
    /// <param name="curve">The curve instance.</param>
    /// <returns>Normalized time [0, 1].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRatio(in this CurveFloat curve)
    {
        if (curve.Duration <= 0f) return 1f;
        float t = curve.CurrentTime / curve.Duration;
        CoreMath.Clamp(t, 0f, 1f, out float result);
        return result;
    }

    /// <summary>
    ///     Returns true if the curve has completed (CurrentTime >= Duration).
    /// </summary>
    /// <param name="curve">The curve instance.</param>
    /// <returns>True if finished.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFinished(in this CurveFloat curve)
    {
        return curve.CurrentTime >= curve.Duration;
    }

    /// <summary>
    ///     Resets the curve time to 0.
    /// </summary>
    /// <param name="curve">The curve instance.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this CurveFloat curve)
    {
        curve.CurrentTime = 0f;
    }
}
