using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;
using Variable.Timer;

namespace Variable.Tween;

/// <summary>
///     A tweening value that interpolates from a start value to an end value over a duration.
///     Uses <see cref="Variable.Timer.Timer" /> for time tracking and <see cref="EasingType" /> for interpolation.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct TweenFloat : IEquatable<TweenFloat>, ICompletable
{
    /// <summary>The timer tracking the tween's progress.</summary>
    public Variable.Timer.Timer Timer;

    /// <summary>The starting value of the tween.</summary>
    public float Start;

    /// <summary>The ending value of the tween.</summary>
    public float End;

    /// <summary>The current interpolated value.</summary>
    public float Current;

    /// <summary>The easing function used for interpolation.</summary>
    public EasingType Easing;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TweenFloat" /> struct.
    /// </summary>
    /// <param name="start">The start value.</param>
    /// <param name="end">The end value.</param>
    /// <param name="duration">The duration of the tween in seconds.</param>
    /// <param name="easing">The easing function to use. Defaults to Linear.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TweenFloat(float start, float end, float duration, EasingType easing = EasingType.Linear)
    {
        Start = start;
        End = end;
        Timer = new Variable.Timer.Timer(duration);
        Easing = easing;
        Current = start;
    }

    /// <inheritdoc />
    bool ICompletable.IsComplete
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TimerExtensions.IsFull(Timer);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is TweenFloat other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TweenFloat other)
    {
        return Timer.Equals(other.Timer) &&
               Start.Equals(other.Start) &&
               End.Equals(other.End) &&
               Current.Equals(other.Current) &&
               Easing == other.Easing;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Timer, Start, End, Current, Easing);
    }

    /// <summary>Determines whether two tweens are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(TweenFloat left, TweenFloat right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two tweens are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(TweenFloat left, TweenFloat right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"TweenFloat: {Current} ({Start}->{End}) [{Timer.Current}/{Timer.Duration}]";
    }
}
