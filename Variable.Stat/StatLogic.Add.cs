using System.Runtime.CompilerServices;

namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    /// Adds an amount to the current value, clamping it to the maximum.
    /// </summary>
    /// <param name="current">The reference to the current value.</param>
    /// <param name="amount">The amount to add.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The actual amount added.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Add(ref float current, float amount, float max)
    {
        if (amount <= TOLERANCE) return 0f;
        
        float space = max - current;
        if (space < 0f) space = 0f;
        
        float toAdd = amount < space ? amount : space;
        current += toAdd;
        
        // Ensure we hit max exactly if close enough
        if (max - current < TOLERANCE) current = max;
        
        return toAdd;
    }
}
