using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Variable.Timer;

/// <summary>
///     A countdown timer that tracks elapsed time from 0 up to a target duration.
///     Useful for casting times, loading bars, buffs, and count-up timers.
/// </summary>
/// <remarks>
///     <para>The timer starts at 0 and counts up to <see cref="Duration" />.</para>
///     <para>Use <see cref="TryTick(float)" /> to advance the timer and check for completion in one call.</para>
///     <para>Check <see cref="IsFull" /> to determine if the timer has reached the duration.</para>
/// </remarks>
/// <example>
///     <code>
/// var castTimer = new Timer(2.0f);
/// 
/// void Update(float dt) {
///     // TryTick returns true when Current >= Duration
///     if (castTimer.TryTick(dt)) {
///         FinishCast();
///         castTimer.Reset();
///     }
/// }
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Duration} ({(IsFull() ? \"Done\" : \"Running\")})")]
public struct Timer :
    IBoundedInfo,
    IEquatable<Timer>,
    IComparable<Timer>,
    IComparable,
    IFormattable
{
    private const float Tolerance = 0.0001f;

    /// <summary>The current elapsed time.</summary>
    public float Current;

    /// <summary>The target duration.</summary>
    public float Duration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Timer"/> struct.
    /// </summary>
    /// <param name="duration">The target duration in seconds.</param>
    /// <param name="current">The initial elapsed time. Defaults to 0. Clamped between 0 and duration.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Timer(float duration, float current = 0f)
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
    ///     Advances the timer by the specified delta time.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the timer is full (completed); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryTick(float deltaTime)
    {
        if (Current >= Duration - Tolerance) return true;

        Current += deltaTime;
        if (Current >= Duration - Tolerance)
        {
            Current = Duration;
            return true;
        }
        return false;
    }

    /// <summary>
    ///     Advances the timer and outputs the overflow time if it completes.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <param name="overflow">The amount of time that exceeded the duration.</param>
    /// <returns>True if the timer is full (completed); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryTick(float deltaTime, out float overflow)
    {
        if (Current >= Duration - Tolerance)
        {
            overflow = deltaTime;
            return true;
        }

        Current += deltaTime;
        if (Current >= Duration - Tolerance)
        {
            overflow = Current - Duration;
            Current = Duration;
            return true;
        }

        overflow = 0f;
        return false;
    }

    /// <summary>
    ///     Resets the timer elapsed time to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        Current = 0f;
    }

    /// <summary>
    ///     Resets the timer and applies an overflow offset.
    /// </summary>
    /// <param name="overflow">The amount of time to start the timer with (clamped to duration).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset(float overflow)
    {
        Current = overflow > 0f ? (overflow > Duration ? Duration : overflow) : 0f;
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
    ///     Returns true when the timer has reached or exceeded its duration.
    /// </summary>
    /// <returns>True if complete.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFull() => Current >= Duration - Tolerance;

    /// <summary>
    ///     Returns true when the timer is at zero (not yet started).
    /// </summary>
    /// <returns>True if current is near zero.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsEmpty() => Current <= Tolerance;

    /// <summary>
    ///     Gets the remaining time until the timer reaches its duration.
    /// </summary>
    public readonly float Remaining
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Max(0, Duration - Current);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetRatio()
    {
        return Math.Abs(Duration) < Tolerance ? 1.0 : (double)Current / Duration;
    }

    /// <inheritdoc />
    public readonly override string ToString() => $"{Current:F2}/{Duration:F2}";

    /// <inheritdoc />
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
    }

    /// <inheritdoc />
    public readonly override bool Equals(object? obj) => obj is Timer other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Timer other) => Current.Equals(other.Current) && Duration.Equals(other.Duration);

    /// <summary>
    ///     Compares equality with another timer using the in modifier.
    /// </summary>
    /// <param name="other">The other timer.</param>
    /// <returns>True if equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in Timer other) => Current.Equals(other.Current) && Duration.Equals(other.Duration);

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
    public readonly override int GetHashCode() => HashCode.Combine(Current, Duration);

    /// <summary>Determines whether two timers are equal.</summary>
    /// <param name="left">The first timer.</param>
    /// <param name="right">The second timer.</param>
    /// <returns>True if equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Timer left, in Timer right) => left.Equals(in right);

    /// <summary>Determines whether two timers are not equal.</summary>
    /// <param name="left">The first timer.</param>
    /// <param name="right">The second timer.</param>
    /// <returns>True if not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Timer left, in Timer right) => !left.Equals(in right);
}