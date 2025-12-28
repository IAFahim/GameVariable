using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    /// Attempts to consume the specified amount from the current value.
    /// Returns true if successful (enough value existed), false otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConsume(ref float current, float amount)
    {
        if (HasEnough(current, amount))
        {
            current -= amount;
            if (current < TOLERANCE) current = 0f;
            return true;
        }
        return false;
    }
}
