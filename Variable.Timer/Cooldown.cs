using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Timer
{
    /// <summary>
    /// A countdown cooldown timer that tracks remaining time until ready.
    /// Useful for ability cooldowns, attack intervals, and rate limiting.
    /// </summary>
    /// <remarks>
    /// <para>The cooldown starts at 0 (ready) or Duration (on cooldown) and counts down to 0.</para>
    /// <para>Use <see cref="Tick"/> to reduce the cooldown each frame.</para>
    /// <para>Use <see cref="Reset"/> to start the cooldown after using an ability.</para>
    /// <para>Check <see cref="IsFull"/> (which returns true when Current &lt;= 0) to determine if ready.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dashCd = new Cooldown(3f); // 3 second cooldown, starts ready
    /// if (dashCd.IsFull()) {
    ///     Dash();
    ///     dashCd.Reset(); // Start cooldown
    /// }
    /// dashCd.Tick(deltaTime);
    /// </code>
    /// </example>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Duration}")]
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
        /// Creates a new cooldown with the specified duration.
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
        /// Starts the cooldown by setting current to duration.
        /// Call this after using an ability.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Current = Duration;
        }

        /// <summary>
        /// Finishes the cooldown immediately by setting current to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish()
        {
            Current = 0f;
        }

        /// <summary>
        /// Reduces the cooldown by the specified delta time.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last tick.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick(float deltaTime)
        {
            Current -= deltaTime;
            if (Current < 0f) Current = 0f;
        }

        /// <summary>
        /// Returns true when the cooldown is ready (current &lt;= 0).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current <= 0f;

        /// <summary>
        /// Returns true when the cooldown just started (current &gt;= duration).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current >= Duration;

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
            return obj is Cooldown other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cooldown other)
        {
            return Current.Equals(other.Current) && Duration.Equals(other.Duration);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Cooldown other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is Cooldown other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(Cooldown)}");
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Duration);
        }

        /// <summary>Determines whether two cooldowns are equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cooldown left, Cooldown right)
        {
            return left.Equals(right);
        }

        /// <summary>Determines whether two cooldowns are not equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cooldown left, Cooldown right)
        {
            return !left.Equals(right);
        }
    }
}