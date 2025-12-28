namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Calculates the percentage of current value to maximum value (0.0 to 100.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetPercent(float current, float max)
    {
        return GetRatio(current, max) * 100f;
    }
}