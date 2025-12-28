using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Timer
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Duration}")]
    public struct Cooldown :
        IVariable,
        IEquatable<Cooldown>,
        IComparable<Cooldown>,
        IComparable,
        IFormattable
    {
        public float Current;
        public float Duration;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cooldown(float duration, float current = 0f)
        {
            Duration = duration;
            Current = current > duration ? duration : current < 0f ? 0f : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Current = Duration;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish()
        {
            Current = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick(float deltaTime)
        {
            Current -= deltaTime;
            if (Current < 0f) Current = 0f;
        }

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current <= 0f;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current >= Duration;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Math.Abs(Duration) < float.Epsilon ? 0.0 : Current / Duration;
        }

        public override string ToString()
        {
            return $"{Current}/{Duration}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
        }

        public override bool Equals(object obj)
        {
            return obj is Cooldown other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Cooldown other)
        {
            return Current.Equals(other.Current) && Duration.Equals(other.Duration);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Cooldown other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is Cooldown other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(Cooldown)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Duration);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cooldown left, Cooldown right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cooldown left, Cooldown right)
        {
            return !left.Equals(right);
        }
    }
}