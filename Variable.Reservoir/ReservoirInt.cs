namespace Variable.Reservoir;

/// <summary>
///     Represents a reservoir system with a limited volume and a reserve (integer version).
///     Ideal for ammo with magazines, fuel with tanks, batteries, and similar mechanics.
/// </summary>
/// <remarks>
///     <para>The <see cref="Volume" /> is the active/usable amount (e.g., ammo in clip).</para>
///     <para>The <see cref="Reserve" /> is the backup supply (e.g., extra ammo).</para>
///     <para>Use <see cref="ReservoirExtensions.Refill(ref ReservoirInt)" /> to transfer from reserve to volume.</para>
/// </remarks>
/// <example>
///     <code>
/// // Pistol: 12 shot magazine, 36 extra rounds
/// var ammo = new ReservoirInt(12, 12, 36);
/// ammo.Volume--; // Fire
/// ammo.Refill(); // Reload
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct ReservoirInt : IEquatable<ReservoirInt>
{
    /// <summary>The active volume (bounded capacity).</summary>
    public BoundedInt Volume;

    /// <summary>The reserve supply.</summary>
    public int Reserve;

    /// <summary>
    ///     Creates a new reservoir with specified capacity, current volume, and reserve.
    /// </summary>
    /// <param name="capacity">The maximum volume capacity.</param>
    /// <param name="currentVolume">The current volume.</param>
    /// <param name="reserve">The reserve amount.</param>
    public ReservoirInt(int capacity, int currentVolume, int reserve)
    {
        Volume = new BoundedInt(capacity, currentVolume);
        Reserve = reserve;
    }

    /// <summary>
    ///     Creates a new reservoir from an existing bounded int and reserve.
    /// </summary>
    /// <param name="volume">The bounded volume.</param>
    /// <param name="reserve">The reserve amount.</param>
    public ReservoirInt(BoundedInt volume, int reserve)
    {
        Volume = volume;
        Reserve = reserve;
    }

    /// <summary>Implicitly converts the reservoir to its current volume value.</summary>
    public static implicit operator int(ReservoirInt res)
    {
        return res.Volume.Current;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ReservoirInt other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ReservoirInt other)
    {
        return Volume.Equals(other.Volume) && Reserve == other.Reserve;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Volume, Reserve);
    }

    /// <summary>Determines whether two reservoirs are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ReservoirInt left, ReservoirInt right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two reservoirs are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ReservoirInt left, ReservoirInt right)
    {
        return !left.Equals(right);
    }
}