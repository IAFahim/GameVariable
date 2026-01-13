using System.Runtime.CompilerServices;

namespace GameVariable.Intent
{
    /// <summary>
    /// Ultra-fast pure functional state machine.
    /// Eliminates ALL overhead: no enter/exit methods, no redundant assignments.
    /// Just pure state transitions.
    /// </summary>
    public static class IntentFast
    {
        // Pure function: (currentState, event) -> nextState
        // No side effects, no allocations, no method call overhead
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState.StateId Transition(IntentState.StateId current, IntentState.EventId evt)
        {
            // Optimized switch expression - compiles to jump table
            return (current, evt) switch
            {
                // FROM CREATED
                (IntentState.StateId.CREATED, IntentState.EventId.GET_READY) => IntentState.StateId.WAITING_FOR_ACTIVATION,
                (IntentState.StateId.CREATED, IntentState.EventId.ACTIVATED) => IntentState.StateId.WAITING_TO_RUN,

                // FROM WAITING_TO_RUN
                (IntentState.StateId.WAITING_TO_RUN, IntentState.EventId.START_RUNNING) => IntentState.StateId.RUNNING,
                (IntentState.StateId.WAITING_TO_RUN, IntentState.EventId.CANCELED_BEFORE_RUN) => IntentState.StateId.CANCELED,

                // FROM WAITING_FOR_ACTIVATION
                (IntentState.StateId.WAITING_FOR_ACTIVATION, IntentState.EventId.ACTIVATED) => IntentState.StateId.WAITING_TO_RUN,
                (IntentState.StateId.WAITING_FOR_ACTIVATION, IntentState.EventId.CANCELED_BEFORE_ACTIVATION) => IntentState.StateId.CANCELED,

                // FROM RUNNING
                (IntentState.StateId.RUNNING, IntentState.EventId.CHILD_TASK_CREATED) => IntentState.StateId.WAITING_FOR_CHILDREN_TO_COMPLETE,
                (IntentState.StateId.RUNNING, IntentState.EventId.UNABLE_TO_COMPLETE) => IntentState.StateId.FAULTED,
                (IntentState.StateId.RUNNING, IntentState.EventId.CANCEL) => IntentState.StateId.CANCELED,
                (IntentState.StateId.RUNNING, IntentState.EventId.COMPLETED_SUCCESSFULLY) => IntentState.StateId.RAN_TO_COMPLETION,

                // FROM WAITING_FOR_CHILDREN_TO_COMPLETE
                (IntentState.StateId.WAITING_FOR_CHILDREN_TO_COMPLETE, IntentState.EventId.ALL_CHILDREN_COMPLETED) => IntentState.StateId.RUNNING,

                // FROM RAN_TO_COMPLETION
                (IntentState.StateId.RAN_TO_COMPLETION, IntentState.EventId.RUN_AGAIN) => IntentState.StateId.WAITING_TO_RUN,

                // Terminal states and invalid events stay in current state
                _ => current
            };
        }

        // Even faster: Pass by reference, update in place
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transition(ref IntentState.StateId state, IntentState.EventId evt)
        {
            IntentState.StateId next = Transition(state, evt);
            bool changed = next != state;
            state = next;
            return changed;
        }
    }

    /// <summary>
    /// Lightweight struct for ultra-fast state machine usage.
    /// No methods, no overhead - just data.
    /// </summary>
    public struct IntentStateFast
    {
        public IntentState.StateId state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dispatch(IntentState.EventId evt)
        {
            return IntentFast.Transition(ref state, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            state = IntentState.StateId.CREATED;
        }
    }
}
