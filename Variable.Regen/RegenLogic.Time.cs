namespace Variable.Regen;

/// <summary>
///     Contains time-related calculations for regeneration logic (Time to Full/Empty).
/// </summary>
public static partial class RegenLogic
{
    /// <summary>
    ///     Calculates the time required to fully regenerate from current to max.
    ///     Returns float.PositiveInfinity if rate is &lt;= 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetTimeToFull(in float current, in float max, in float rate, out float result)
    {
        if (rate <= 0f)
        {
            result = float.PositiveInfinity;
            return;
        }

        var missing = max - current;
        if (missing <= 0f)
        {
            result = 0f;
            return;
        }

        result = missing / rate;
    }

    /// <summary>
    ///     Calculates the time required to fully decay from current to 0.
    ///     Returns float.PositiveInfinity if rate is 0.
    ///     Use positive rate for magnitude of decay.
    ///     If rate is passed as positive (speed of decay), use GetTimeToEmpty(current, decayRate).
    ///     This method assumes 'rate' is the signed change per second.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetTimeToEmpty(in float current, in float rate, out float result)
    {
        if (rate == 0f)
        {
            result = float.PositiveInfinity;
            return;
        }

        if (current <= 0f)
        {
            result = 0f;
            return;
        }

        result = current / Math.Abs(rate);
    }
}