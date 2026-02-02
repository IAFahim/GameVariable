namespace Variable.Timer;

/// <summary>
///     A countdown timer that tracks elapsed time from 0 up to a target duration.
///     Useful for casting times, loading bars, buffs, and count-up timers.
/// </summary>
/// <remarks>
///     <para>The timer starts at 0 and counts up to <see cref="Duration" />.</para>
///     <para>
///         Use <see cref="TimerExtensions.TickAndCheckComplete(ref Timer, float)" /> to advance and check completion in
///         one call.
///     </para>
///     <para>Check <see cref="TimerExtensions.IsFull(Timer)" /> to determine if the timer has reached the duration.</para>
/// </remarks>
/// <example>
///     <code>
/// var castTime = new Timer(2.0f);
/// 
/// void Update(float dt) {
///     // TickAndCheckComplete returns true when Current >= Duration
///     if (castTime.TickAndCheckComplete(dt)) {
///         FinishCast();
///         castTime.Reset();
///     }
/// }
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Duration}")]
public struct Timer :
    IBoundedInfo,
    ICompletable,
    IEquatable<Timer>,
    IComparable<Timer>,
    IComparable
{
    /// <summary>The current elapsed time.</summary>
    public float Current;

    /// <summary>The target duration.</summary>
    public float Duration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Timer" /> struct.
    /// </summary>
    /// <param name="duration">The target duration in seconds.</param>
    /// <param name="current">The initial elapsed time. Defaults to 0. Clamped between 0 and duration.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Timer(float duration, float current = 0f)
    {
        Duration = duration;
        TimerLogic.Clamp(current, 0f, duration, out Current);
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
        get => TimerLogic.IsFull(Current, Duration);
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        Span<char> buffer = stackalloc char[64];
        if (TryFormat(buffer, out var charsWritten))
        {
            return new string(buffer.Slice(0, charsWritten));
        }

        return string.Format(CultureInfo.InvariantCulture, "{0:F2}/{1:F2}", Current, Duration);
    }

    /// <summary>
    ///     Formats the timer with custom format strings.
    /// </summary>
    /// <param name="format">
    ///     Format code: "R" = Ratio (percentage), null = default format.
    /// </param>
    /// <param name="formatProvider">The provider to use for formatting. Defaults to <see cref="CultureInfo.InvariantCulture" />.</param>
    /// <returns>Formatted string.</returns>
    public readonly string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        Span<char> buffer = stackalloc char[64];
        if (TryFormat(buffer, out var charsWritten, format, formatProvider))
        {
            return new string(buffer.Slice(0, charsWritten));
        }

        return ToString();
    }

    /// <summary>
    ///     Formats the timer into a character span (zero allocation).
    /// </summary>
    /// <param name="destination">The span to write the formatted value into.</param>
    /// <param name="charsWritten">The number of characters written.</param>
    /// <param name="format">Optional format string. "R" for ratio percentage.</param>
    /// <param name="provider">The provider to use for formatting. Defaults to <see cref="CultureInfo.InvariantCulture" />.</param>
    /// <returns>True if formatting succeeded; false if destination was too small.</returns>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        provider ??= CultureInfo.InvariantCulture;
        charsWritten = 0;

        // Handle ratio format
        if (format.Length > 0 && (format[0] == 'R' || format[0] == 'r'))
        {
            var ratio = this.GetRatio() * 100.0;
            if (!ratio.TryFormat(destination, out var written, "F1", provider))
                return false;
            charsWritten = written;

            if (charsWritten >= destination.Length)
                return false;
            destination[charsWritten++] = '%';
            return true;
        }

        // Default format: "Current/Duration" with F2 precision
        if (!Current.TryFormat(destination, out var currentWritten, "F2", provider))
            return false;
        charsWritten = currentWritten;

        if (charsWritten >= destination.Length)
            return false;
        destination[charsWritten++] = '/';

        if (!Duration.TryFormat(destination.Slice(charsWritten), out var durationWritten, "F2", provider))
            return false;
        charsWritten += durationWritten;

        return true;
    }

    /// <inheritdoc />
    public readonly override bool Equals(object? obj)
    {
        return obj is Timer other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Timer other)
    {
        return Current.Equals(other.Current) && Duration.Equals(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(Timer other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(object? obj)
    {
        if (obj is Timer other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(Timer)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Current, Duration);
    }

    /// <summary>Determines whether two timers are equal.</summary>
    /// <param name="left">The first timer.</param>
    /// <param name="right">The second timer.</param>
    /// <returns>True if equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Timer left, Timer right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two timers are not equal.</summary>
    /// <param name="left">The first timer.</param>
    /// <param name="right">The second timer.</param>
    /// <returns>True if not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Timer left, Timer right)
    {
        return !left.Equals(right);
    }
}