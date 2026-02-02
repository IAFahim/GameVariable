namespace Variable.Motion;

/// <summary>
///     A physics-based value that moves towards a target using a spring-damper system.
///     Ideal for "juicy" UI, camera follow, and organic movement.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current} -> {Target} (V: {Velocity})")]
public struct SpringFloat : IEquatable<SpringFloat>
{
    /// <summary>The current value.</summary>
    public float Current;

    /// <summary>The current velocity.</summary>
    public float Velocity;

    /// <summary>The target value to move towards.</summary>
    public float Target;

    /// <summary>Spring stiffness (k). Higher values mean faster oscillation.</summary>
    public float Stiffness;

    /// <summary>Spring damping (c). Higher values mean less oscillation (more friction).</summary>
    public float Damping;

    /// <summary>
    ///     Initializes a new SpringFloat with raw stiffness and damping.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SpringFloat(float start, float target, float stiffness, float damping)
    {
        Current = start;
        Target = target;
        Velocity = 0f;
        Stiffness = stiffness;
        Damping = damping;
    }

    /// <summary>
    ///     Initializes a new SpringFloat using frequency and damping ratio.
    /// </summary>
    /// <param name="start">Initial value.</param>
    /// <param name="target">Target value.</param>
    /// <param name="frequency">Frequency in Hz (oscillations per second). Default 10.</param>
    /// <param name="dampingRatio">Damping ratio (0 = no damping, 1 = critical damping, >1 = overdamping). Default 0.8.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SpringFloat FromFrequency(float start, float target, float frequency = 10f, float dampingRatio = 0.8f)
    {
        // k = (2 * pi * f)^2
        // c = 4 * pi * f * z
        var f = frequency * 2f * MathF.PI;
        var k = f * f;
        var c = 2f * f * dampingRatio;

        return new SpringFloat(start, target, k, c);
    }

    /// <summary>
    ///     Checks if the spring has settled at the target (low velocity and low distance).
    /// </summary>
    /// <param name="epsilon">The threshold for distance and velocity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsSettled(float epsilon = 0.01f)
    {
        return MathF.Abs(Current - Target) < epsilon && MathF.Abs(Velocity) < epsilon;
    }

    /// <summary>
    ///     Teleports the spring to the target and stops velocity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Snap()
    {
        Current = Target;
        Velocity = 0f;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is SpringFloat other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(SpringFloat other)
    {
        return Current.Equals(other.Current) &&
               Velocity.Equals(other.Velocity) &&
               Target.Equals(other.Target) &&
               Stiffness.Equals(other.Stiffness) &&
               Damping.Equals(other.Damping);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Current, Velocity, Target, Stiffness, Damping);
    }

    /// <summary>Determines whether two SpringFloats are equal.</summary>
    public static bool operator ==(SpringFloat left, SpringFloat right) => left.Equals(right);

    /// <summary>Determines whether two SpringFloats are not equal.</summary>
    public static bool operator !=(SpringFloat left, SpringFloat right) => !left.Equals(right);
}
