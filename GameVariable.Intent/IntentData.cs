using System;
using System.Runtime.InteropServices;

namespace GameVariable.Intent;

/// <summary>
///     Data structure for the Intent state machine.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct IntentData : IEquatable<IntentData>
{
    /// <summary>
    ///     The current state offset (StateId * 16).
    /// </summary>
    public byte StateOffset;

    /// <summary>
    ///     Gets the current State ID.
    /// </summary>
    public byte StateId
    {
        readonly get => IntentLogic.OffsetToStateId(StateOffset);
        set => StateOffset = IntentLogic.StateIdToOffset(value);
    }

    public IntentData(byte stateId)
    {
        StateOffset = IntentLogic.StateIdToOffset(stateId);
    }

    public override bool Equals(object? obj)
    {
        return obj is IntentData data && Equals(data);
    }

    public bool Equals(IntentData other)
    {
        return StateOffset == other.StateOffset;
    }

    public override int GetHashCode()
    {
        return StateOffset.GetHashCode();
    }

    public static bool operator ==(IntentData left, IntentData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(IntentData left, IntentData right)
    {
        return !left.Equals(right);
    }
}
