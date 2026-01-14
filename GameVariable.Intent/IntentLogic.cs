using System.Runtime.CompilerServices;

namespace GameVariable.Intent;

/// <summary>
///     Core logic for the Intent state machine.
///     Implements a lookup-table based state machine for maximum performance.
/// </summary>
public static class IntentLogic
{
    // States
    public const byte ROOT = 0;
    public const byte CANCELED = 1;
    public const byte CREATED = 2;
    public const byte FAULTED = 3;
    public const byte RAN_TO_COMPLETION = 4;
    public const byte RUNNING = 5;
    public const byte WAITING_FOR_ACTIVATION = 6;
    public const byte WAITING_FOR_CHILDREN_TO_COMPLETE = 7;
    public const byte WAITING_TO_RUN = 8;

    // Events
    public const byte ACTIVATED = 0;
    public const byte ALL_CHILDREN_COMPLETED = 1;
    public const byte CANCEL = 2;
    public const byte CANCELED_BEFORE_ACTIVATION = 3;
    public const byte CANCELED_BEFORE_RUN = 4;
    public const byte CHILD_TASK_CREATED = 5;
    public const byte COMPLETED_SUCCESSFULLY = 6;
    public const byte GET_READY = 7;
    public const byte RUN_AGAIN = 8;
    public const byte START_RUNNING = 9;
    public const byte UNABLE_TO_COMPLETE = 10;

    // Table: 9 states * 16 bytes stride = 144 bytes
    private static readonly byte[] TransitionTable = new byte[144];

    static IntentLogic()
    {
        // Initialize table where every state transitions to itself (no change)
        for (int i = 0; i < 9; i++)
        {
            byte offset = (byte)(i * 16);
            for (int j = 0; j < 16; j++)
            {
                TransitionTable[offset + j] = offset;
            }
        }

        // Row 2: CREATED
        Set(CREATED, GET_READY, WAITING_FOR_ACTIVATION);
        Set(CREATED, ACTIVATED, WAITING_TO_RUN);

        // Row 6: WAITING_FOR_ACTIVATION
        Set(WAITING_FOR_ACTIVATION, ACTIVATED, WAITING_TO_RUN);
        Set(WAITING_FOR_ACTIVATION, CANCELED_BEFORE_ACTIVATION, CANCELED);

        // Row 8: WAITING_TO_RUN
        Set(WAITING_TO_RUN, START_RUNNING, RUNNING);
        Set(WAITING_TO_RUN, CANCELED_BEFORE_RUN, CANCELED);

        // Row 5: RUNNING
        Set(RUNNING, CHILD_TASK_CREATED, WAITING_FOR_CHILDREN_TO_COMPLETE);
        Set(RUNNING, UNABLE_TO_COMPLETE, FAULTED);
        Set(RUNNING, CANCEL, CANCELED);
        Set(RUNNING, COMPLETED_SUCCESSFULLY, RAN_TO_COMPLETION);

        // Row 7: WAITING_FOR_CHILDREN_TO_COMPLETE
        Set(WAITING_FOR_CHILDREN_TO_COMPLETE, ALL_CHILDREN_COMPLETED, RUNNING);

        // Row 4: RAN_TO_COMPLETION
        Set(RAN_TO_COMPLETION, RUN_AGAIN, WAITING_TO_RUN);
    }

    private static void Set(byte fromState, byte evt, byte toState)
    {
        int index = (fromState * 16) + evt;
        TransitionTable[index] = (byte)(toState * 16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte OffsetToStateId(byte offset)
    {
        return (byte)(offset / 16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte StateIdToOffset(byte id)
    {
        return (byte)(id * 16);
    }

    /// <summary>
    /// Applies an event to the current state offset.
    /// </summary>
    /// <param name="currentOffset">The current state offset (stateId * 16).</param>
    /// <param name="eventId">The event ID to dispatch.</param>
    /// <param name="nextOffset">The resulting state offset.</param>
    /// <param name="changed">True if the state changed, false otherwise.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Apply(in byte currentOffset, byte eventId, out byte nextOffset, out bool changed)
    {
        // Mask eventId to ensure it doesn't overflow the 16-byte row
        // Although valid events are 0-10, masking protects against invalid input
        int index = currentOffset + (eventId & 0x0F);
        
        nextOffset = TransitionTable[index];
        changed = nextOffset != currentOffset;
    }
}
