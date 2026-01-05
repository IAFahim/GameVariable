namespace Variable.Input;

/// <summary>
///     Runtime state for combo system traversal.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct ComboState : IEquatable<ComboState>
{
    /// <summary>
    ///     The index of the current active node in the graph.
    /// </summary>
    public int CurrentNodeIndex;

    /// <summary>
    ///     Indicates whether the current action is still executing, preventing transitions.
    /// </summary>
    public bool IsActionBusy;

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ComboState other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ComboState other)
    {
        return CurrentNodeIndex == other.CurrentNodeIndex && IsActionBusy == other.IsActionBusy;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(CurrentNodeIndex, IsActionBusy);
    }

    /// <summary>Determines whether two states are equal.</summary>
    /// <param name="left">The first state to compare.</param>
    /// <param name="right">The second state to compare.</param>
    /// <returns>True if the states are equal; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ComboState left, ComboState right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two states are not equal.</summary>
    /// <param name="left">The first state to compare.</param>
    /// <param name="right">The second state to compare.</param>
    /// <returns>True if the states are not equal; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ComboState left, ComboState right)
    {
        return !left.Equals(right);
    }
}