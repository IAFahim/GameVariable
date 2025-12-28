namespace Variable.Timer;

/// <summary>
///     A countdown timer that tracks elapsed time from 0 to a target duration.
///     Useful for casting times, loading bars, buffs, and count-up timers.
/// </summary>
/// <remarks>
///     <para>The timer starts at 0 and counts up to <see cref="Duration" />.</para>
///     <para>Use <see cref="Tick" /> to advance the timer each frame.</para>
///     <para>Check <see cref="IsFull" /> to determine if the timer has completed.</para>
///     <para>The <see cref="Tick" /> method returns overflow time for precise timing.</para>
/// </remarks>
/// <example>
///     <code>
/// var castTime = new Timer(2.5f);
/// float overflow = castTime.Tick(deltaTime);
/// if (castTime.IsFull()) {
///     CastSpell();
///     castTime.Reset();
///     // Apply overflow to next action if needed
/// }
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Duration}")]
public struct Timer :
    IBoundedInfo,
    IEquatable<Timer>,
    IComparable<Timer>,
    IComparable,
    IFormattable
{
    /// <summary>The current elapsed time.</summary>
    public float Current;

    /// <summary>The target duration.</summary>
    public float Duration;

    /// <summary>
    ///     Creates a new timer with the specified duration.
    /// </summary>
    /// <param name="duration">The target duration in seconds.</param>
    /// <param name="current">The initial elapsed time. Defaults to 0. Will be clamped to [0, duration].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Timer(float duration, float current = 0f)
    {
        Duration = duration;
        Current = current > duration ? duration : current < 0f ? 0f : current;
    }

    /// <summary>
    ///     Resets the timer to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        Current = 0f;
    }

    /// <summary>
    ///     Resets the timer to 0 and applies the specified overflow time.
    ///     Useful for precise timing when chaining timers.
    /// </summary>
    /// <param name="overflow">Excess time from a previous tick to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset(float overflow)
    {
        Current = overflow > 0f ? overflow > Duration ? Duration : overflow : 0f;
    }

    /// <summary>
    ///     Completes the timer immediately by setting current to duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Finish()
    {
        Current = Duration;
    }

    /// <summary>
    ///     Advances the timer by the specified delta time.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick (e.g., Time.deltaTime).</param>
    /// <returns>The overflow time if the timer completed this tick; otherwise 0.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Tick(float deltaTime)
    {
        Current += deltaTime;
        if (Current > Duration)
        {
            var overflow = Current - Duration;
            Current = Duration;
            return overflow;
        }

        return 0f;
    }

    /// <summary>
    ///     Advances the timer and automatically resets if completed, applying overflow.
    ///     Useful for repeating timers.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the timer completed this tick; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TickAndReset(float deltaTime)
    {
        var overflow = Tick(deltaTime);
        if (overflow > 0f || IsFull())
        {
            Reset(overflow);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Gets the remaining time until completion.
    /// </summary>
    public readonly float Remaining
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Duration - Current;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFull()
    {
        return Current >= Duration - MathConstants.Tolerance;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsEmpty()
    {
        return Current <= MathConstants.Tolerance;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetRatio()
    {
        return Math.Abs(Duration) < MathConstants.Tolerance ? 0.0 : Current / Duration;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return $"{Current:F2}/{Duration:F2}";
    }

    /// <inheritdoc />
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
    }

    /// <inheritdoc />
    public readonly override bool Equals(object obj)
    {
        return obj is Timer other && Equals(in other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Timer other)
    {
        return Equals(in other);
    }

    /// <summary>Compares equality with another timer using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Timer other)
    {
        return Current.Equals(other.Current) && Duration.Equals(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(Timer other)
    {
        return CompareTo(in other);
    }

    /// <summary>Compares with another timer using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(in Timer other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(object obj)
    {
        if (obj is Timer other) return CompareTo(in other);
        throw new ArgumentException($"Object must be of type {nameof(Timer)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Current, Duration);
    }

    /// <summary>Determines whether two timers are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Timer left, in Timer right)
    {
        return left.Equals(in right);
    }

    /// <summary>Determines whether two timers are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Timer left, in Timer right)
    {
        return !left.Equals(in right);
    }
}