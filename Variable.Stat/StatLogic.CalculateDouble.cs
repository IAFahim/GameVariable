using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Calculates the final value of a stat based on base value, flat modifiers, and percentage modifiers.
    ///     Formula: (baseValue + flat) * (1 + percentAdd) * percentMult
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Calculate(double baseValue, double flat, double percentAdd, double percentMult)
    {
        return (baseValue + flat) * (1d + percentAdd) * percentMult;
    }
}