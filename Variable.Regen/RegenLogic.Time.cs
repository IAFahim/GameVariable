namespace Variable.Regen;

public static partial class RegenLogic
{
    /// <summary>
    ///     Calculates the time required to fully regenerate from current to max.
    ///     Returns float.PositiveInfinity if rate is &lt;= 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetTimeToFull(float current, float max, float rate)
    {
        if (rate <= 0f) return float.PositiveInfinity;
        var missing = max - current;
        if (missing <= 0f) return 0f;
        return missing / rate;
    }

    /// <summary>
    ///     Calculates the time required to fully decay from current to 0.
    ///     Returns float.PositiveInfinity if rate is 0.
    ///     Use positive rate for magnitude of decay.
    ///     If rate is passed as positive (speed of decay), use GetTimeToEmpty(current, decayRate).
    ///     This method assumes 'rate' is the signed change per second.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetTimeToEmpty(float current, float rate)
    {
        if (rate == 0f) return float.PositiveInfinity;
        if (current <= 0f) return 0f;
        return current / Math.Abs(rate);
    }
}