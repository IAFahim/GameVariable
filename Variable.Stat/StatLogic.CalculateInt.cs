namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Calculates the final value of a stat based on base value, flat modifiers, and percentage modifiers, rounded to the
    ///     nearest integer.
    ///     Formula: Round((baseValue + flat) * (1 + percentAdd) * percentMult)
    /// </summary>
    /// <param name="baseValue">The starting base value of the stat.</param>
    /// <param name="flat">The sum of all flat modifiers.</param>
    /// <param name="percentAdd">The sum of all additive percentage modifiers.</param>
    /// <param name="percentMult">The product of all multiplicative percentage modifiers.</param>
    /// <param name="result">The calculated final value rounded to the nearest integer.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateInt(float baseValue, float flat, float percentAdd, float percentMult, out int result)
    {
        var temp = (baseValue + flat) * (1f + percentAdd) * percentMult;
        result = (int)Math.Round(temp, MidpointRounding.AwayFromZero);
    }
}