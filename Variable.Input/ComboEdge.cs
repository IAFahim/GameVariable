namespace Variable.Input;

/// <summary>
///     Represents an edge in the combo graph.
///     Defines input trigger and target node.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct ComboEdge : IEquatable<ComboEdge>
{
    /// <summary>
    ///     The input ID that triggers the transition.
    /// </summary>
    public int InputTrigger;

    /// <summary>
    ///     The index of the target node in the graph's node array.
    /// </summary>
    public int TargetNodeIndex;

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ComboEdge other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ComboEdge other)
    {
        return InputTrigger == other.InputTrigger && TargetNodeIndex == other.TargetNodeIndex;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(InputTrigger, TargetNodeIndex);
    }

    /// <summary>Determines whether two edges are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ComboEdge left, ComboEdge right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two edges are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ComboEdge left, ComboEdge right)
    {
        return !left.Equals(right);
    }
}