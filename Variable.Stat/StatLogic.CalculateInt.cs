using System.Runtime.CompilerServices;

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
    /// <returns>The calculated final value rounded to the nearest integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalculateInt(float baseValue, float flat, float percentAdd, float percentMult)
    {
        var result = (baseValue + flat) * (1f + percentAdd) * percentMult;
        return (int)(result + 0.5f);
    }
}