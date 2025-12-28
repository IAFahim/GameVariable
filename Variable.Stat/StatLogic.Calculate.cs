namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Calculates the final value of a stat based on base value, flat modifiers, and percentage modifiers.
    ///     Formula: (baseValue + flat) * (1 + percentAdd) * percentMult
    /// </summary>
    /// <param name="baseValue">The starting base value of the stat.</param>
    /// <param name="flat">The sum of all flat modifiers (e.g., +10 Strength).</param>
    /// <param name="percentAdd">The sum of all additive percentage modifiers (e.g., +0.10 for 10% increase). 0.5 means +50%.</param>
    /// <param name="percentMult">The product of all multiplicative percentage modifiers. 1.0 is neutral. 1.5 means x1.5.</param>
    /// <returns>The calculated final value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Calculate(float baseValue, float flat, float percentAdd, float percentMult)
    {
        return (baseValue + flat) * (1f + percentAdd) * percentMult;
    }
}