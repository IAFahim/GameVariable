namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Calculates the missing amount to reach the maximum value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetMissing(float current, float max, out float result)
    {
        var missing = max - current;
        result = missing < 0f ? 0f : missing;
    }
}