namespace Variable.Stat;

/// <summary>
///     Provides static logic for stat modification and management.
/// </summary>
public static partial class StatLogic
{
    /// <summary>
    ///     Adds an amount to the current value, clamping it to the maximum.
    /// </summary>
    /// <param name="current">The reference to the current value.</param>
    /// <param name="amount">The amount to add.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The actual amount added.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ref float current, float amount, float max, out float result)
    {
        if (amount <= MathConstants.Tolerance)
        {
            result = 0f;
            return;
        }

        var missing = max - current;
        var actual = amount > missing ? missing : amount;

        current += actual;
        if (max - current < MathConstants.Tolerance) current = max;

        result = actual;
    }
}