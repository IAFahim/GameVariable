using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    /// Calculates the ratio of current value to maximum value (0.0 to 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRatio(float current, float max)
    {
        if (max <= TOLERANCE) return 0f;
        float ratio = current / max;
        if (ratio < 0f) return 0f;
        if (ratio > 1f) return 1f;
        return ratio;
    }
}
