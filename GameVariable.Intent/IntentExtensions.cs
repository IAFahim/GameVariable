using System.Runtime.CompilerServices;

namespace GameVariable.Intent;

/// <summary>
///     Extension methods for IntentData.
/// </summary>
public static class IntentExtensions
{
    public enum IntentState : byte
    {
        ROOT = IntentLogic.ROOT,
        CANCELED = IntentLogic.CANCELED,
        CREATED = IntentLogic.CREATED,
        FAULTED = IntentLogic.FAULTED,
        RAN_TO_COMPLETION = IntentLogic.RAN_TO_COMPLETION,
        RUNNING = IntentLogic.RUNNING,
        WAITING_FOR_ACTIVATION = IntentLogic.WAITING_FOR_ACTIVATION,
        WAITING_FOR_CHILDREN_TO_COMPLETE = IntentLogic.WAITING_FOR_CHILDREN_TO_COMPLETE,
        WAITING_TO_RUN = IntentLogic.WAITING_TO_RUN
    }

    public enum IntentEvent : byte
    {
        ACTIVATED = IntentLogic.ACTIVATED,
        ALL_CHILDREN_COMPLETED = IntentLogic.ALL_CHILDREN_COMPLETED,
        CANCEL = IntentLogic.CANCEL,
        CANCELED_BEFORE_ACTIVATION = IntentLogic.CANCELED_BEFORE_ACTIVATION,
        CANCELED_BEFORE_RUN = IntentLogic.CANCELED_BEFORE_RUN,
        CHILD_TASK_CREATED = IntentLogic.CHILD_TASK_CREATED,
        COMPLETED_SUCCESSFULLY = IntentLogic.COMPLETED_SUCCESSFULLY,
        GET_READY = IntentLogic.GET_READY,
        RUN_AGAIN = IntentLogic.RUN_AGAIN,
        START_RUNNING = IntentLogic.START_RUNNING,
        UNABLE_TO_COMPLETE = IntentLogic.UNABLE_TO_COMPLETE
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDispatch(ref this IntentData data, IntentEvent eventId)
    {
        IntentLogic.Apply(in data.StateOffset, (byte)eventId, out var nextOffset, out var changed);
        if (changed)
        {
            data.StateOffset = nextOffset;
        }
        return changed;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Start(ref this IntentData data)
    {
        data.StateOffset = IntentLogic.StateIdToOffset(IntentLogic.CREATED);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntentState GetState(this IntentData data)
    {
        return (IntentState)data.StateId;
    }

    public static string StateIdToString(IntentState id)
    {
        return id.ToString();
    }

    public static string EventIdToString(IntentEvent id)
    {
        return id.ToString();
    }
}
