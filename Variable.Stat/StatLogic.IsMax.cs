using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Checks if the current value is at the maximum.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMax(float current, float max)
    {
        return current >= max - TOLERANCE;
    }
}