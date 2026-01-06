namespace Variable.RPG;

/// <summary>
///     Represents a complex RPG statistic (Health, Strength, Fire Resist).
///     Supports base values, flat modifiers, percentage multipliers, and min/max bounds.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Attribute
{
    /// <summary>The base value (e.g., base Strength = 10).</summary>
    public float Base;

    /// <summary>Sum of flat modifiers (e.g., +5 from Ring).</summary>
    public float ModAdd;

    /// <summary>Product of multipliers (e.g., x1.1 from Buff). Starts at 1.0.</summary>
    public float ModMult;

    /// <summary>The hard minimum bound (e.g., 0 for Health, -1.0 for Resistance).</summary>
    public float Min;

    /// <summary>The hard maximum bound.</summary>
    public float Max;

    /// <summary>The cached result of the calculation.</summary>
    public float Value;

    /// <summary>
    ///     Creates a new Attribute with default bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Attribute(float baseVal, float min = 0f, float max = float.MaxValue)
    {
        Base = baseVal;
        ModAdd = 0f;
        ModMult = 1f;
        Min = min;
        Max = max;
        var val = baseVal;
        if (val > max) val = max;
        else if (val < min) val = min;
        Value = val;
    }
}