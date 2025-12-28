using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Timer
{
    /// <summary>
    /// A countdown timer that tracks elapsed time from 0 to a target duration.
    /// Useful for casting times, loading bars, buffs, and count-up timers.
    /// </summary>
    /// <remarks>
    /// <para>The timer starts at 0 and counts up to <see cref="Duration"/>.</para>
    /// <para>Use <see cref="Tick"/> to advance the timer each frame.</para>
    /// <para>Check <see cref="IsFull"/> to determine if the timer has completed.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var castTime = new Timer(2.5f);
    /// castTime.Tick(deltaTime);
    /// if (castTime.IsFull()) CastSpell();
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
        /// Creates a new timer with the specified duration.
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
        /// Resets the timer to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Current = 0f;
        }

        /// <summary>
        /// Completes the timer immediately by setting current to duration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish()
        {
            Current = Duration;
        }

        /// <summary>
        /// Advances the timer by the specified delta time.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last tick (e.g., Time.deltaTime).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick(float deltaTime)
        {
            Current += deltaTime;
            if (Current > Duration) Current = Duration;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current >= Duration;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current <= 0f;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Math.Abs(Duration) < float.Epsilon ? 0.0 : Current / Duration;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Current}/{Duration}";
        }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Timer other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Timer other)
        {
            return Current.Equals(other.Current) && Duration.Equals(other.Duration);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Timer other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is Timer other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(Timer)}");
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Duration);
        }

        /// <summary>Determines whether two timers are equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Timer left, Timer right)
        {
            return left.Equals(right);
        }

        /// <summary>Determines whether two timers are not equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Timer left, Timer right)
        {
            return !left.Equals(right);
        }
    }
}