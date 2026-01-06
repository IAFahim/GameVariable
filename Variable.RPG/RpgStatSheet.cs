namespace Variable.RPG;

/// <summary>
///     A container for attributes using unmanaged memory.
///     Supports both managed wrapper (for Unity SerializeField) and pure unsafe usage.
/// </summary>
/// <remarks>
/// <para><b>CRITICAL - Memory Safety Warning:</b></para>
/// <para>This struct allocates unmanaged memory via Marshal.AllocHGlobal.</para>
/// <para>You MUST call Dispose() when finished, OR use a using statement.</para>
/// <para><b>DO NOT COPY THIS STRUCT</b> after construction - copying creates a shallow copy with the same pointer,
/// leading to use-after-free vulnerabilities if one copy is disposed.</para>
/// <para>Recommended usage: Create once, use by reference, dispose once.</para>
/// </remarks>
public unsafe struct RpgStatSheet
{
    private RpgStat* _attributes;
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
    public ref RpgStat this[int index]
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
    public Span<RpgStat> Attributes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => AsSpan();
    }

    /// <summary>
    ///     Creates a new RpgStatSheet with allocated memory.
    /// </summary>
    /// <param name="count">The number of attributes to allocate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RpgStatSheet(int count)
    {
        _count = count;
        _attributes = (RpgStat*)Marshal.AllocHGlobal(count * sizeof(RpgStat));
        _ownsMemory = true;

        // Initialize all attributes to default
        for (var i = 0; i < count; i++) _attributes[i] = new RpgStat(0f);
    }

    /// <summary>
    ///     Creates a new RpgStatSheet from externally allocated memory.
    /// </summary>
    /// <param name="attributes">Pointer to attribute array.</param>
    /// <param name="count">Number of attributes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RpgStatSheet(RpgStat* attributes, int count)
    {
        _attributes = attributes;
        _count = count;
        _ownsMemory = false;
    }

    /// <summary>
    ///     Creates a new RpgStatSheet from a Span.
    /// </summary>
    /// <param name="attributes">Span of attributes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RpgStatSheet(Span<RpgStat> attributes)
    {
        fixed (RpgStat* ptr = attributes)
        {
            _attributes = ptr;
            _count = attributes.Length;
            _ownsMemory = false;
        }
    }

    /// <summary>
    ///     Disposes the RpgStatSheet, freeing allocated memory if owned.
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
    public Span<RpgStat> AsSpan()
    {
        if (_attributes == null || _count == 0) return Span<RpgStat>.Empty;
        return new Span<RpgStat>(_attributes, _count);
    }

    /// <summary>
    ///     Gets a reference to an attribute by ID.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref RpgStat GetRef(int statId)
    {
        if (statId < 0 || statId >= _count || _attributes == null)
            throw new IndexOutOfRangeException($"Stat ID {statId} out of range [0, {_count})");
        return ref _attributes[statId];
    }
}