namespace Variable.RPG;

/// <summary>
///     A container for attributes. Wraps an array for dense, fast access.
/// </summary>
public struct AttributeSheet
{
    public Attribute[] Attributes;

    public AttributeSheet(int count)
    {
        Attributes = new Attribute[count];
        for (int i = 0; i < count; i++)
            Attributes[i] = new Attribute(0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetBase(int statId, float value)
    {
        if (statId < 0 || statId >= Attributes.Length) return;
        Attributes[statId].Base = value;
        Attributes[statId].IsDirty = 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int statId)
    {
        if (statId < 0 || statId >= Attributes.Length) return 0f;
        return AttributeLogic.GetValue(ref Attributes[statId]);
    }

    /// <summary>
    ///     Returns a Span for logic processing (Zero Alloc).
    /// </summary>
    public Span<Attribute> AsSpan() => Attributes.AsSpan();
}
