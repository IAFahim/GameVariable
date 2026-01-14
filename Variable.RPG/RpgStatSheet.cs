using System.Threading;

namespace Variable.RPG;

/// <summary>
///     A container for attributes using unmanaged memory.
///     Supports both managed wrapper (for Unity SerializeField) and pure unsafe usage.
/// </summary>
/// <remarks>
///     <para>
///         <b>CRITICAL - Memory Safety Warning:</b>
///     </para>
///     <para>This struct allocates unmanaged memory via Marshal.AllocHGlobal.</para>
///     <para>You MUST call Dispose() when finished, OR use a using statement.</para>
///     <para>
///         <b>DO NOT COPY THIS STRUCT</b> after construction - copying creates a shallow copy with the same pointer,
///         leading to use-after-free vulnerabilities if one copy is disposed.
///     </para>
///     <para>Recommended usage: Create once, pass by reference, dispose once.</para>
///     <para>
///         NOTE: This struct cannot be marked readonly due to mutable state requirements.
///         Extra caution is required when handling this type.
///     </para>
/// </remarks>
public unsafe struct RpgStatSheet
{
    private static int _globalAllocationCounter;
    private RpgStat* _attributes;
    private bool _ownsMemory;
    private int _allocationId; // Unique ID to detect copies

    /// <summary>
    ///     Gets the number of attributes.
    /// </summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        private set;
    }

    /// <summary>
    ///     Provides direct indexer access to attributes for test convenience.
    /// </summary>
    public ref RpgStat this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= Count || _attributes == null)
                throw new IndexOutOfRangeException($"Index {index} out of range [0, {Count})");
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
    public RpgStatSheet(int count)
    {
        if (count < 0)
        {
            Count = 0;
            _attributes = null;
            _ownsMemory = false;
            _allocationId = 0;
            return;
        }

        Count = count;
        _attributes = (RpgStat*)Marshal.AllocHGlobal(count * sizeof(RpgStat));
        _ownsMemory = true;
        _allocationId = Interlocked.Increment(ref _globalAllocationCounter);

        // Initialize all attributes to default
        for (var i = 0; i < count; i++) _attributes[i] = new RpgStat(0f);
    }

    /// <summary>
    ///     Creates a new RpgStatSheet from externally allocated memory.
    /// </summary>
    /// <param name="attributes">Pointer to attribute array.</param>
    /// <param name="count">Number of attributes.</param>
    public RpgStatSheet(RpgStat* attributes, int count)
    {
        if (count < 0)
        {
            _attributes = null;
            Count = 0;
            _ownsMemory = false;
            _allocationId = 0;
            return;
        }

        _attributes = attributes;
        Count = count;
        _ownsMemory = false;
        _allocationId = 0;
    }


    /// <summary>
    ///     Disposes the RpgStatSheet, freeing allocated memory if owned.
    ///     Safe to call multiple times. Detects if this is a copy and prevents double-free.
    /// </summary>
    public void Dispose()
    {
        if (!_ownsMemory || _attributes == null) return;

        // Double-free protection: mark as disposed before freeing
        var wasOwner = _ownsMemory;
        _ownsMemory = false;

        if (wasOwner)
        {
            Marshal.FreeHGlobal((IntPtr)_attributes);
            _attributes = null;
            Count = 0;
            _allocationId = 0;
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
        if (statId < 0 || statId >= Count || _attributes == null) return;
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
        if (statId < 0 || statId >= Count || _attributes == null) return 0f;
        return _attributes[statId].GetValue();
    }

    /// <summary>
    ///     Returns a Span for logic processing (Zero Alloc).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<RpgStat> AsSpan()
    {
        if (_attributes == null || Count == 0) return Span<RpgStat>.Empty;
        return new Span<RpgStat>(_attributes, Count);
    }

    /// <summary>
    ///     Gets a reference to an attribute by ID.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref RpgStat GetRef(int statId)
    {
        if (statId < 0 || statId >= Count || _attributes == null)
            throw new IndexOutOfRangeException($"Stat ID {statId} out of range [0, {Count})");
        return ref _attributes[statId];
    }
}