namespace Variable.RPG;

/// <summary>
///     A container for attributes using unmanaged memory.
///     Supports both managed wrapper (for Unity SerializeField) and pure unsafe usage.
/// </summary>
public unsafe struct AttributeSheet
{
    private Attribute* _attributes;
    private int _count;
    private bool _ownsMemory;

    /// <summary>
    ///     Gets the number of attributes.
    /// </summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _count;
    }

    /// <summary>
    ///     Provides direct indexer access to attributes for test convenience.
    /// </summary>
    public ref Attribute this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= _count || _attributes == null)
                throw new IndexOutOfRangeException($"Index {index} out of range [0, {_count})");
            return ref _attributes[index];
        }
    }

    /// <summary>
    ///     Gets a Span view of all attributes for compatibility.
    /// </summary>
    public Span<Attribute> Attributes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => AsSpan();
    }

    /// <summary>
    ///     Creates a new AttributeSheet with allocated memory.
    /// </summary>
    /// <param name="count">The number of attributes to allocate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AttributeSheet(int count)
    {
        _count = count;
        _attributes = (Attribute*)Marshal.AllocHGlobal(count * sizeof(Attribute));
        _ownsMemory = true;

        // Initialize all attributes to default
        for (var i = 0; i < count; i++) _attributes[i] = new Attribute(0f);
    }

    /// <summary>
    ///     Creates a new AttributeSheet from externally allocated memory.
    /// </summary>
    /// <param name="attributes">Pointer to attribute array.</param>
    /// <param name="count">Number of attributes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AttributeSheet(Attribute* attributes, int count)
    {
        _attributes = attributes;
        _count = count;
        _ownsMemory = false;
    }

    /// <summary>
    ///     Creates a new AttributeSheet from a Span.
    /// </summary>
    /// <param name="attributes">Span of attributes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AttributeSheet(Span<Attribute> attributes)
    {
        fixed (Attribute* ptr = attributes)
        {
            _attributes = ptr;
            _count = attributes.Length;
            _ownsMemory = false;
        }
    }

    /// <summary>
    ///     Disposes the AttributeSheet, freeing allocated memory if owned.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (_ownsMemory && _attributes != null)
        {
            Marshal.FreeHGlobal((IntPtr)_attributes);
            _attributes = null;
            _count = 0;
            _ownsMemory = false;
        }
    }

    /// <summary>
    ///     Sets the base value of a specific attribute.
    /// </summary>
    /// <param name="statId">The ID of the attribute.</param>
    /// <param name="value">The base value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetBase(int statId, float value)
    {
        if (statId < 0 || statId >= _count || _attributes == null) return;
        _attributes[statId].Base = value;
    }

    /// <summary>
    ///     Gets the calculated value of a specific attribute.
    /// </summary>
    /// <param name="statId">The ID of the attribute.</param>
    /// <returns>The calculated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int statId)
    {
        if (statId < 0 || statId >= _count || _attributes == null) return 0f;
        return _attributes[statId].GetValue();
    }

    /// <summary>
    ///     Returns a Span for logic processing (Zero Alloc).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<Attribute> AsSpan()
    {
        if (_attributes == null || _count == 0) return Span<Attribute>.Empty;
        return new Span<Attribute>(_attributes, _count);
    }

    /// <summary>
    ///     Gets a reference to an attribute by ID.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Attribute GetRef(int statId)
    {
        if (statId < 0 || statId >= _count || _attributes == null)
            throw new IndexOutOfRangeException($"Stat ID {statId} out of range [0, {_count})");
        return ref _attributes[statId];
    }
}