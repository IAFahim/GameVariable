using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    /// Calculates the missing amount to reach the maximum value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMissing(float current, float max)
    {
        float missing = max - current;
        return missing < 0f ? 0f : missing;
    }
}
