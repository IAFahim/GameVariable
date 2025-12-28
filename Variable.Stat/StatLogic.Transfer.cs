using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    /// Transfers as much of the requested amount as possible from a source to a destination.
    /// </summary>
    /// <param name="source">The reference to the source value.</param>
    /// <param name="target">The reference to the target value.</param>
    /// <param name="maxTarget">The maximum value of the target.</param>
    /// <param name="amount">The amount requested to transfer.</param>
    /// <returns>The actual amount transferred.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Transfer(ref float source, ref float target, float maxTarget, float amount)
    {
        if (source <= TOLERANCE || amount <= TOLERANCE) return 0f;

        float available = source < amount ? source : amount;

        float space = maxTarget - target;
        if (space < 0f) space = 0f;

        float actualMove = available < space ? available : space;

        if (actualMove > TOLERANCE)
        {
            source -= actualMove;
            target += actualMove;

            if (source < TOLERANCE) source = 0f;
            if (maxTarget - target < TOLERANCE) target = maxTarget;

            return actualMove;
        }

        return 0f;
    }
}
