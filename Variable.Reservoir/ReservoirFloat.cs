namespace Variable.Reservoir;

/// <summary>
///     Represents a reservoir system with a limited volume and a reserve.
///     Ideal for ammo with magazines, fuel with tanks, batteries, and similar mechanics.
/// </summary>
/// <remarks>
///     <para>The <see cref="Volume" /> is the active/usable amount (e.g., ammo in clip).</para>
///     <para>The <see cref="Reserve" /> is the backup supply (e.g., extra ammo).</para>
///     <para>Use <see cref="ReservoirExtensions.Refill(ref ReservoirFloat)" /> to transfer from reserve to volume.</para>
/// </remarks>
/// <example>
///     <code>
/// // Pistol: 12 shot magazine, 36 extra rounds
/// var ammo = new ReservoirFloat(12f, 12f, 36f);
/// ammo.Volume -= 1f; // Fire
/// ammo.Refill(); // Reload
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct ReservoirFloat : IEquatable<ReservoirFloat>
{
    /// <summary>The active volume (bounded capacity).</summary>
    public BoundedFloat Volume;

    /// <summary>The reserve supply.</summary>
    public float Reserve;

    /// <summary>
    ///     Creates a new reservoir with specified capacity, current volume, and reserve.
    /// </summary>
    /// <param name="capacity">The maximum volume capacity.</param>
    /// <param name="currentVolume">The current volume.</param>
    /// <param name="reserve">The reserve amount.</param>
    public ReservoirFloat(float capacity, float currentVolume, float reserve)
    {
        Volume = new BoundedFloat(capacity, currentVolume);
        Reserve = reserve;
    }

    /// <summary>
    ///     Creates a new reservoir from an existing bounded float and reserve.
    /// </summary>
    /// <param name="volume">The bounded volume.</param>
    /// <param name="reserve">The reserve amount.</param>
    public ReservoirFloat(BoundedFloat volume, float reserve)
    {
        Volume = volume;
        Reserve = reserve;
    }

    /// <summary>Implicitly converts the reservoir to its current volume value.</summary>
    public static implicit operator float(ReservoirFloat res)
    {
        return res.Volume.Current;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ReservoirFloat other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ReservoirFloat other)
    {
        return Volume.Equals(other.Volume) && Reserve.Equals(other.Reserve);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Volume, Reserve);
    }

    /// <summary>Determines whether two reservoirs are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ReservoirFloat left, ReservoirFloat right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two reservoirs are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ReservoirFloat left, ReservoirFloat right)
    {
        return !left.Equals(right);
    }
}