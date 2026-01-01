namespace Variable.Timer;

/// <summary>
///     A countdown cooldown timer that tracks remaining time until ready.
///     Useful for ability cooldowns, attack intervals, and rate limiting.
/// </summary>
/// <remarks>
///     <para>The cooldown starts at 0 (ready) or Duration (on cooldown) and counts down to 0.</para>
///     <para>Use <see cref="TryTick(float)" /> to reduce time and check readiness in one call.</para>
///     <para>Use <see cref="Reset()" /> to start the cooldown after using an ability.</para>
/// </remarks>
/// <example>
///     <code>
/// var dashCd = new Cooldown(3f);
/// 
/// void Update(float dt) {
///     // TryTick reduces time and returns true if ready
///     if (dashCd.TryTick(dt) &amp;&amp; Input.Dash) {
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
    private const float Tolerance = 0.0001f;

    /// <summary>The current remaining cooldown time.</summary>
    public float Current;

    /// <summary>The total cooldown duration.</summary>
    public float Duration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Cooldown"/> struct.
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float GetMin() => 0;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float GetCurrent() => Current;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float GetMax() => Duration;

    /// <summary>
    ///     Attempts to progress the timer by the specified delta time.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the cooldown is ready (Current is 0); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryTick(float deltaTime)
    {
        if (Current <= Tolerance) return true;

        Current -= deltaTime;
        if (Current <= Tolerance)
        {
            Current = 0f;
            return true;
        }
        return false;
    }

    /// <summary>
    ///     Attempts to progress the timer and outputs the overflow time.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <param name="overflow">The amount of time that exceeded the zero mark.</param>
    /// <returns>True if the cooldown is ready (Current is 0); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryTick(float deltaTime, out float overflow)
    {
        if (Current <= Tolerance)
        {
            overflow = deltaTime;
            return true;
        }

        Current -= deltaTime;
        if (Current <= Tolerance)
        {
            overflow = -Current;
            Current = 0f;
            return true;
        }

        overflow = 0f;
        return false;
    }

    /// <summary>
    ///     Starts the cooldown by setting <see cref="Current"/> to <see cref="Duration"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        Current = Duration;
    }

    /// <summary>
    ///     Starts the cooldown, reducing the start time by the provided overflow.
    /// </summary>
    /// <param name="overflow">Excess time to subtract from the new duration.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset(float overflow)
    {
        Current = Duration - (overflow > 0f ? overflow : 0f);
        if (Current < 0f) Current = 0f;
    }

    /// <summary>
    ///     Finishes the cooldown immediately by setting <see cref="Current"/> to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Finish()
    {
        Current = 0f;
    }

    /// <summary>
    ///     Returns true when the cooldown is ready (Current &lt;= 0).
    /// </summary>
    /// <returns>True if ready; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsReady() => Current <= Tolerance;

    /// <summary>
    ///     Returns true when the cooldown is ready. Semantic alias for <see cref="IsReady"/>.
    /// </summary>
    /// <returns>True if the remaining time is zero.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFull() => Current <= Tolerance;

    /// <summary>
    ///     Returns true when the cooldown is at its maximum duration (just started).
    /// </summary>
    /// <returns>True if current time matches or exceeds duration.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsEmpty() => Current >= Duration - Tolerance;

    /// <summary>
    ///     Returns true when the cooldown is active (not yet ready).
    /// </summary>
    /// <returns>True if on cooldown; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsOnCooldown() => Current > Tolerance;

    /// <summary>
    ///     Gets the progress of the cooldown as a ratio from 0 (just started) to 1 (ready).
    /// </summary>
    public readonly float Progress
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Abs(Duration) < Tolerance ? 1f : 1f - MathF.Max(0, Current / Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetRatio()
    {
        return Math.Abs(Duration) < Tolerance ? 0.0 : (double)Current / Duration;
    }

    /// <inheritdoc />
    public readonly override string ToString() => IsReady() ? "Ready" : $"{Current:F2}s";

    /// <inheritdoc />
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
    }

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Cooldown other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Cooldown other) => Current.Equals(other.Current) && Duration.Equals(other.Duration);

    /// <summary>
    ///     Compares equality with another cooldown using the in modifier for performance.
    /// </summary>
    /// <param name="other">The other cooldown to compare.</param>
    /// <returns>True if Current and Duration are equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Cooldown other) => Current.Equals(other.Current) && Duration.Equals(other.Duration);

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
    public readonly override int GetHashCode() => HashCode.Combine(Current, Duration);

    /// <summary>Determines whether two cooldowns are equal.</summary>
    /// <param name="left">The first cooldown.</param>
    /// <param name="right">The second cooldown.</param>
    /// <returns>True if equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Cooldown left, in Cooldown right) => left.Equals(in right);

    /// <summary>Determines whether two cooldowns are not equal.</summary>
    /// <param name="left">The first cooldown.</param>
    /// <param name="right">The second cooldown.</param>
    /// <returns>True if not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Cooldown left, in Cooldown right) => !left.Equals(in right);
}
