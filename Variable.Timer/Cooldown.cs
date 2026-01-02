namespace Variable.Timer;

/// <summary>
///     A countdown cooldown timer that tracks remaining time until ready.
///     Useful for ability cooldowns, attack intervals, and rate limiting.
/// </summary>
/// <remarks>
///     <para>The cooldown starts at 0 (ready) or Duration (on cooldown) and counts down to 0.</para>
///     <para>Use <see>
///             <cref>TickAndCheckReady(float)</cref>
///         </see>
///         to reduce time and check readiness in one call.</para>
///     <para>Use <see>
///             <cref>Reset()</cref>
///         </see>
///         to start the cooldown after using an ability.</para>
/// </remarks>
/// <example>
///     <code>
/// var dashCd = new Cooldown(3f);
/// 
/// void Update(float dt) {
///     // TickAndCheckReady reduces time and returns true if ready
///     if (dashCd.TickAndCheckReady(dt) &amp;&amp; Input.Dash) {
///         Dash();
///         dashCd.Reset();
///     }
/// }
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Duration}")]
public struct Cooldown :
    IBoundedInfo,
    ICompletable,
    IEquatable<Cooldown>,
    IComparable<Cooldown>,
    IComparable
{
    /// <summary>The current remaining cooldown time.</summary>
    public float Current;

    /// <summary>The total cooldown duration.</summary>
    public float Duration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Cooldown" /> struct.
    /// </summary>
    /// <param name="duration">The total cooldown duration in seconds.</param>
    /// <param name="current">The initial remaining time. Defaults to 0 (ready). Clamped between 0 and duration.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Cooldown(float duration, float current = 0f)
    {
        Duration = duration;
        Current = current > duration ? duration : current < 0f ? 0f : current;
    }

    /// <inheritdoc />
    float IBoundedInfo.Min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 0;
    }

    /// <inheritdoc />
    float IBoundedInfo.Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Current;
    }

    /// <inheritdoc />
    float IBoundedInfo.Max
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Duration;
    }

    /// <inheritdoc />
    bool ICompletable.IsComplete
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Current <= 0.0001f;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return Current <= 0.0001f ? "Ready" : $"{Current:F2}s";
    }

    /// <inheritdoc />
    public readonly override bool Equals(object? obj)
    {
        return obj is Cooldown other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Cooldown other)
    {
        return Current.Equals(other.Current) && Duration.Equals(other.Duration);
    }

    /// <summary>
    ///     Compares equality with another cooldown using the in modifier for performance.
    /// </summary>
    /// <param name="other">The other cooldown to compare.</param>
    /// <returns>True if Current and Duration are equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Cooldown other)
    {
        return Current.Equals(other.Current) && Duration.Equals(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(Cooldown other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(object? obj)
    {
        if (obj is Cooldown other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(Cooldown)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Current, Duration);
    }

    /// <summary>Determines whether two cooldowns are equal.</summary>
    /// <param name="left">The first cooldown.</param>
    /// <param name="right">The second cooldown.</param>
    /// <returns>True if equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Cooldown left, in Cooldown right)
    {
        return left.Equals(in right);
    }

    /// <summary>Determines whether two cooldowns are not equal.</summary>
    /// <param name="left">The first cooldown.</param>
    /// <param name="right">The second cooldown.</param>
    /// <returns>True if not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Cooldown left, in Cooldown right)
    {
        return !left.Equals(in right);
    }
}