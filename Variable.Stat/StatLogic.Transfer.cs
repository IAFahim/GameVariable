namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Transfers as much of the requested amount as possible from a source to a destination.
    /// </summary>
    /// <param name="source">The reference to the source value.</param>
    /// <param name="target">The reference to the target value.</param>
    /// <param name="maxTarget">The maximum value of the target.</param>
    /// <param name="amount">The amount requested to transfer.</param>
    /// <param name="result">The actual amount transferred.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transfer(ref float source, ref float target, float maxTarget, float amount, out float result)
    {
        if (source <= MathConstants.Tolerance || amount <= MathConstants.Tolerance)
        {
            result = 0f;
            return;
        }

        var availableSpace = maxTarget - target;
        var actualMove = amount < source ? amount : source;
        if (actualMove > availableSpace) actualMove = availableSpace;

        if (actualMove < 0f) actualMove = 0f;

        if (actualMove > MathConstants.Tolerance)
        {
            source -= actualMove;
            target += actualMove;

            if (source < MathConstants.Tolerance) source = 0f;
            if (maxTarget - target < MathConstants.Tolerance) target = maxTarget;
        }

        result = actualMove;
    }
}