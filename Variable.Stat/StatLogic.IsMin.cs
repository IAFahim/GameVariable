using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Checks if the current value is at the minimum (default 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMin(float current, float min = 0f)
    {
        return current <= min + TOLERANCE;
    }
}