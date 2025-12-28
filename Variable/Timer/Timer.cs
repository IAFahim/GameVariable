using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Variable.Timer
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Duration}")]
    public struct Timer :
        IEquatable<Timer>,
        IComparable<Timer>,
        IComparable,
        IFormattable
    {
        public float Current;
        public float Duration;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Timer(float duration, float current)
        {
            Duration = duration;
            Current = current > duration ? duration : (current < 0f ? 0f : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Timer(float duration) : this(duration, 0f)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() => Current = 0f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish() => Current = Duration;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick(float deltaTime)
        {
            Current += deltaTime;
            if (Current > Duration) Current = Duration;
        }

        public bool IsFinished
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current >= Duration;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Math.Abs(Duration) < float.Epsilon ? 0.0 : Current / Duration;

        public override string ToString() => $"{Current}/{Duration}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            return $"{Current.ToString(format, formatProvider)}/{Duration.ToString(format, formatProvider)}";
        }

        public override bool Equals(object obj) => obj is Timer other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Timer other) => Current.Equals(other.Current) && Duration.Equals(other.Duration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Timer other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Duration.CompareTo(other.Duration);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is Timer other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(Timer)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Duration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Timer left, Timer right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Timer left, Timer right) => !left.Equals(right);
    }
}
