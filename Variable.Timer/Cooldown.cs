namespace Variable.Timer;

/// <summary>
///     A countdown cooldown timer that tracks remaining time until ready.
///     Useful for ability cooldowns, attack intervals, and rate limiting.
/// </summary>
/// <remarks>
///     <para>The cooldown starts at 0 (ready) or Duration (on cooldown) and counts down to 0.</para>
///     <para>
///         Use <see cref="TimerExtensions.TickAndCheckReady(ref Cooldown, float)" /> to reduce time and check readiness
///         in one call.
///     </para>
///     <para>Use <see cref="TimerExtensions.Reset(ref Cooldown)" /> to start the cooldown after using an ability.</para>
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
        get => TimerLogic.IsEmpty(Current);
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        Span<char> buffer = stackalloc char[64];
        if (TryFormat(buffer, out var charsWritten))
        {
            return new string(buffer.Slice(0, charsWritten));
        }

        return TimerLogic.IsEmpty(Current) ? "Ready" : string.Format(CultureInfo.InvariantCulture, "{0:F2}s", Current);
    }

    /// <summary>
    ///     Formats the cooldown with custom format strings.
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
    ///     Formats the cooldown into a character span (zero allocation).
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

        if (TimerLogic.IsEmpty(Current))
        {
            const string ready = "Ready";
            if (ready.Length > destination.Length) return false;
            ready.AsSpan().CopyTo(destination);
            charsWritten = ready.Length;
            return true;
        }

        // Default format: "Current s" with F2 precision
        if (!Current.TryFormat(destination, out var currentWritten, "F2", provider))
            return false;
        charsWritten = currentWritten;

        if (charsWritten >= destination.Length)
            return false;
        destination[charsWritten++] = 's';

        return true;
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
    public static bool operator ==(Cooldown left, Cooldown right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two cooldowns are not equal.</summary>
    /// <param name="left">The first cooldown.</param>
    /// <param name="right">The second cooldown.</param>
    /// <returns>True if not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Cooldown left, Cooldown right)
    {
        return !left.Equals(right);
    }
}