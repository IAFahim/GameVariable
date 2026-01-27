namespace Variable.Curve;

using System.Runtime.InteropServices;

/// <summary>
///     A struct representing a value that changes over time based on an easing curve.
///     Layer A: Pure Data.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct CurveFloat
{
    /// <summary>The current elapsed time of the curve.</summary>
    public float CurrentTime;

    /// <summary>The total duration of the curve.</summary>
    public float Duration;

    /// <summary>The starting value.</summary>
    public float StartValue;

    /// <summary>The target ending value.</summary>
    public float EndValue;

    /// <summary>The type of easing to apply.</summary>
    public EaseType Type;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CurveFloat"/> struct.
    /// </summary>
    /// <param name="duration">The total duration of the curve.</param>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The target value.</param>
    /// <param name="type">The easing type.</param>
    public CurveFloat(float duration, float start, float end, EaseType type = EaseType.Linear)
    {
        CurrentTime = 0f;
        Duration = duration;
        StartValue = start;
        EndValue = end;
        Type = type;
    }
}
