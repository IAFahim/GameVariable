namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Calculates the ratio of current value to maximum value (0.0 to 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRatio(float current, float max, out float result)
    {
        if (max <= MathConstants.Tolerance)
        {
            result = 0f;
            return;
        }

        var ratio = current / max;
        if (ratio < 0f) result = 0f;
        else if (ratio > 1f) result = 1f;
        else result = ratio;
    }
}