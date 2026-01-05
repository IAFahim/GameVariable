namespace Variable.RPG;

/// <summary>
///     Extension methods for Attribute struct.
///     Adapts the struct to the primitive-only AttributeLogic.
/// </summary>
public static class AttributeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Recalculate(ref this Attribute attr)
    {
        AttributeLogic.Recalculate(
            attr.Base,
            attr.ModAdd,
            attr.ModMult,
            attr.Min,
            attr.Max,
            out attr.CachedValue,
            out attr.IsDirty
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddModifier(ref this Attribute attr, float flat, float percent)
    {
        AttributeLogic.AddModifier(
            ref attr.ModAdd,
            ref attr.ModMult,
            ref attr.IsDirty,
            flat,
            percent
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearModifiers(ref this Attribute attr)
    {
        AttributeLogic.ClearModifiers(
            out attr.ModAdd,
            out attr.ModMult,
            out attr.IsDirty
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValue(ref this Attribute attr)
    {
        return AttributeLogic.GetValue(
            ref attr.CachedValue,
            ref attr.IsDirty,
            attr.Base,
            attr.ModAdd,
            attr.ModMult,
            attr.Min,
            attr.Max
        );
    }
}