namespace Variable.Motion;

/// <summary>
///     A struct that interpolates a float value over time using an easing function.
///     Acts as a "Timer with a Value".
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Value} (T: {Time}/{Duration})")]
public struct TweenFloat :
    IBoundedInfo,
    ICompletable,
    IEquatable<TweenFloat>
{
    /// <summary>The current interpolated value. Updated by Tick().</summary>
    public float Value;

    /// <summary>The start value.</summary>
    public float Start;

    /// <summary>The target end value.</summary>
    public float End;

    /// <summary>The elapsed time since start.</summary>
    public float Time;

    /// <summary>The total duration of the tween.</summary>
    public float Duration;

    /// <summary>The easing function to use.</summary>
    public EasingType Easing;

    /// <summary>
    ///     Initializes a new TweenFloat.
    /// </summary>
    /// <param name="start">Start value.</param>
    /// <param name="end">End value.</param>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="easing">Easing function (default: Linear).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TweenFloat(float start, float end, float duration, EasingType easing = EasingType.Linear)
    {
        Start = start;
        End = end;
        Duration = duration;
        Easing = easing;
        Time = 0f;
        Value = start;
    }

    /// <summary>
    ///     Calculates the value at the current time without modifying state.
    ///     Useful if you manually modified Time.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float CalculateValue()
    {
        if (Duration <= 0f) return End;
        var t = Time / Duration;
        if (t <= 0f) return Start;
        if (t >= 1f) return End;
        return MotionLogic.Interpolate(Start, End, t, Easing);
    }

    /// <summary>
    ///     Resets the tween to start (Time = 0, Value = Start).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        Time = 0f;
        Value = Start;
    }

    /// <inheritdoc />
    float IBoundedInfo.Min => 0f;

    /// <inheritdoc />
    float IBoundedInfo.Max => Duration;

    /// <inheritdoc />
    float IBoundedInfo.Current => Time;

    /// <inheritdoc />
    bool ICompletable.IsComplete => Time >= Duration;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is TweenFloat other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(TweenFloat other)
    {
        return Value.Equals(other.Value) &&
               Start.Equals(other.Start) &&
               End.Equals(other.End) &&
               Time.Equals(other.Time) &&
               Duration.Equals(other.Duration) &&
               Easing == other.Easing;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Start, End, Time, Duration, Easing);
    }

    /// <summary>Determines whether two TweenFloats are equal.</summary>
    public static bool operator ==(TweenFloat left, TweenFloat right) => left.Equals(right);

    /// <summary>Determines whether two TweenFloats are not equal.</summary>
    public static bool operator !=(TweenFloat left, TweenFloat right) => !left.Equals(right);
}
