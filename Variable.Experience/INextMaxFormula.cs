namespace Variable.Experience;

/// <summary>
///     Struct-based function interface for level progression formulas.
///     Replaces Func delegates for Burst compatibility.
/// </summary>
/// <typeparam name="T">The numeric type (int or long).</typeparam>
public interface INextMaxFormula<T> where T : unmanaged
{
    /// <summary>
    ///     Calculates the max XP required for a given level.
    /// </summary>
    /// <param name="level">The level to calculate for.</param>
    /// <returns>The max XP required.</returns>
    T Calculate(int level);
}
