namespace Variable.Timer;

/// <summary>
///     A countdown cooldown timer that tracks remaining time until ready.
///     Useful for ability cooldowns, attack intervals, and rate limiting.
/// </summary>
/// <remarks>
///     <para>The cooldown starts at 0 (ready) or Duration (on cooldown) and counts down to 0.</para>
///     <para>Use <see cref="Tick" /> to reduce the cooldown each frame. Returns overflow time.</para>
///     <para>Use <see cref="Reset" /> to start the cooldown after using an ability.</para>
///     <para>Check <see cref="IsReady" /> to determine if the cooldown is complete.</para>
/// </remarks>
/// <example>
///     <code>
/// var dashCd = new Cooldown(3f); // 3 second cooldown, starts ready
/// 
/// void Update(float dt) {
///     float overflow = dashCd.Tick(dt);
///     
///     if (dashCd.IsReady() &amp;&amp; Input.Dash) {
///         Dash();
///         dashCd.Reset();
///     }
/// }
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Duration} ({(IsReady() ? \"Ready\" : \"Cooling\")})")]
public struct Cooldown :
    IBoundedInfo,
    IEquatable<Cooldown>,
    IComparable<Cooldown>,
    IComparable,
    IFormattable
{
    /// <summary>The current remaining cooldown time.</summary>
    public float Current;

    /// <summary>The total cooldown duration.</summary>
    public float Duration;

    /// <summary>
    ///     Creates a new cooldown with the specified duration.
    /// </summary>
    /// <param name="duration">The total cooldown duration in seconds.</param>
    /// <param name="current">The initial remaining time. Defaults to 0 (ready). Will be clamped to [0, duration].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Cooldown(float duration, float current = 0f)
    {
        Duration = duration;
        Current = current > duration ? duration : current < 0f ? 0f : current;
    }

    /// <summary>
    ///     Starts the cooldown by setting current to duration.
    ///     Call this after using an ability.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        Current = Duration;
    }

    /// <summary>
    ///     Starts the cooldown, reducing by any leftover overflow time.
    ///     Useful for high-precision timing when abilities are used in quick succession.
    /// </summary>
    /// <param name="overflow">Excess time from tick to subtract from the new cooldown.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset(float overflow)
    {
        Current = Duration - (overflow > 0f ? overflow : 0f);
        if (Current < 0f) Current = 0f;
    }

    /// <summary>
    ///     Finishes the cooldown immediately by setting current to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Finish()
    {
        Current = 0f;
    }

    /// <summary>
    ///     Reduces the cooldown by the specified delta time.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>The overflow time (how much past 0 we went). Use for precise timing in high-tick scenarios.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Tick(float deltaTime)
    {
        Current -= deltaTime;
        if (Current < 0f)
        {
            var overflow = -Current;
            Current = 0f;
            return overflow;
        }

        return 0f;
    }

    /// <summary>
    ///     Attempts to use the ability. If ready, triggers the cooldown and returns true.
    /// </summary>
    /// <returns>True if the ability was used (cooldown was ready); otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryUse()
    {
        if (!IsReady()) return false;
        Reset();
        return true;
    }

    /// <summary>
    ///     Attempts to use the ability with overflow compensation.
    ///     Returns true and starts cooldown (minus overflow) if ready.
    /// </summary>
    /// <param name="overflow">Overflow time to subtract from the new cooldown.</param>
    /// <returns>True if the ability was used; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryUse(float overflow)
    {
        if (!IsReady()) return false;
        Reset(overflow);
        return true;
    }

    /// <summary>
    ///     Returns true when the cooldown is ready (current &lt;= 0).
    ///     Alias for <see cref="IsFull" /> but more semantically appropriate for cooldowns.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsReady()
    {
        return Current <= 0f;
    }

    /// <summary>
    ///     Returns true when the cooldown is ready (current &lt;= 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFull()
    {
        return Current <= 0f;
    }

    /// <summary>
    ///     Returns true when the cooldown just started (current &gt;= duration).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsEmpty()
    {
        return Current >= Duration;
    }

    /// <summary>
    ///     Returns true when the cooldown is active (not ready).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsOnCooldown()
    {
        return Current > 0f;
    }

    /// <summary>
    ///     Gets the progress of the cooldown as a ratio from 0 (just started) to 1 (ready).
    ///     Useful for cooldown UI fills.
    /// </summary>
    public readonly float Progress
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Math.Abs(Duration) < float.Epsilon ? 1f : 1f - Current / Duration;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetRatio()
    {
        return Math.Abs(Duration) < float.Epsilon ? 0.0 : Current / Duration;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return IsReady() ? "Ready" : $"{Current:F2}s";
    }

    /// <inheritdoc />
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
    }

    /// <inheritdoc />
    public readonly override bool Equals(object obj)
    {
        return obj is Cooldown other && Equals(in other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Cooldown other)
    {
        return Equals(in other);
    }

    /// <summary>Compares equality with another cooldown using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Cooldown other)
    {
        return Current.Equals(other.Current) && Duration.Equals(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(Cooldown other)
    {
        return CompareTo(in other);
    }

    /// <summary>Compares with another cooldown using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(in Cooldown other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(object obj)
    {
        if (obj is Cooldown other) return CompareTo(in other);
        throw new ArgumentException($"Object must be of type {nameof(Cooldown)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Current, Duration);
    }

    /// <summary>Determines whether two cooldowns are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Cooldown left, in Cooldown right)
    {
        return left.Equals(in right);
    }

    /// <summary>Determines whether two cooldowns are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Cooldown left, in Cooldown right)
    {
        return !left.Equals(in right);
    }
}