using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Sets the current value, clamping it between min and max.
    /// </summary>
    /// <param name="current">The reference to the current value.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="min">The minimum value (default 0).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float value, float max, float min = 0f)
    {
        if (value > max) current = max;
        else if (value < min) current = min;
        else current = value;
    }
}