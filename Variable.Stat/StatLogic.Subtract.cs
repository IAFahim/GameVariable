namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Subtracts an amount from the current value, clamping it to the minimum.
    /// </summary>
    /// <param name="current">The reference to the current value.</param>
    /// <param name="amount">The amount to subtract.</param>
    /// <param name="min">The minimum value (default 0).</param>
    /// <param name="result">The actual amount subtracted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract(ref float current, float amount, out float result, float min = 0f)
    {
        if (amount <= MathConstants.Tolerance)
        {
            result = 0f;
            return;
        }

        var available = current - min;
        if (available < 0f) available = 0f;

        var toSubtract = amount < available ? amount : available;
        current -= toSubtract;

        // Ensure we hit min exactly if close enough
        if (current - min < MathConstants.Tolerance) current = min;

        result = toSubtract;
    }
}