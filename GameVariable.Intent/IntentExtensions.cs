using System.Runtime.CompilerServices;

namespace GameVariable.Intent
{
    /// <summary>
    /// Branchless extension methods for Intent state machine.
    /// Provides zero-branch event dispatch using CPU store buffers.
    /// </summary>
    public static class IntentExtensions
    {
        /// <summary>
        /// Intent events matching the original IntentState.EventId enum.
        /// </summary>
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
            UNABLE_TO_COMPLETE = IntentLogic.UNABLE_TO_COMPLETE,
        }

        /// <summary>
        /// Intent states matching the original IntentState.StateId enum.
        /// </summary>
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
            WAITING_TO_RUN = IntentLogic.WAITING_TO_RUN,
        }

        /// <summary>
        /// Branchless event dispatch.
        /// Uses CPU store buffers to handle redundant writes faster than branch prediction.
        /// </summary>
        /// <param name="data">Reference to the intent data.</param>
        /// <param name="evt">The event to dispatch.</param>
        /// <returns>True if the state changed, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryDispatch(ref this IntentData data, IntentEvent evt)
        {
            // 1. Calculate next state using table lookup
            IntentLogic.Apply(in data.StateOffset, (byte)evt, out byte next, out bool changed);

            // 2. Branchless Write
            // CPU Store Buffers handle redundant writes faster than Branch Prediction handles 'if(changed)'
            data.StateOffset = next;

            return changed;
        }

        /// <summary>
        /// Gets the current state as an IntentState enum.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState GetState(in this IntentData data)
        {
            return (IntentState)data.StateId;
        }

        /// <summary>
        /// Starts the intent state machine in the CREATED state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Start(ref this IntentData data)
        {
            data.StateOffset = IntentLogic.StateIdToOffset(IntentLogic.CREATED);
        }

        /// <summary>
        /// Converts state ID to string for debugging.
        /// </summary>
        public static string StateIdToString(IntentState state)
        {
            return state switch
            {
                IntentState.ROOT => "ROOT",
                IntentState.CANCELED => "CANCELED",
                IntentState.CREATED => "CREATED",
                IntentState.FAULTED => "FAULTED",
                IntentState.RAN_TO_COMPLETION => "RAN_TO_COMPLETION",
                IntentState.RUNNING => "RUNNING",
                IntentState.WAITING_FOR_ACTIVATION => "WAITING_FOR_ACTIVATION",
                IntentState.WAITING_FOR_CHILDREN_TO_COMPLETE => "WAITING_FOR_CHILDREN_TO_COMPLETE",
                IntentState.WAITING_TO_RUN => "WAITING_TO_RUN",
                _ => "?",
            };
        }

        /// <summary>
        /// Converts event ID to string for debugging.
        /// </summary>
        public static string EventIdToString(IntentEvent evt)
        {
            return evt switch
            {
                IntentEvent.ACTIVATED => "ACTIVATED",
                IntentEvent.ALL_CHILDREN_COMPLETED => "ALL_CHILDREN_COMPLETED",
                IntentEvent.CANCEL => "CANCEL",
                IntentEvent.CANCELED_BEFORE_ACTIVATION => "CANCELED_BEFORE_ACTIVATION",
                IntentEvent.CANCELED_BEFORE_RUN => "CANCELED_BEFORE_RUN",
                IntentEvent.CHILD_TASK_CREATED => "CHILD_TASK_CREATED",
                IntentEvent.COMPLETED_SUCCESSFULLY => "COMPLETED_SUCCESSFULLY",
                IntentEvent.GET_READY => "GET_READY",
                IntentEvent.RUN_AGAIN => "RUN_AGAIN",
                IntentEvent.START_RUNNING => "START_RUNNING",
                IntentEvent.UNABLE_TO_COMPLETE => "UNABLE_TO_COMPLETE",
                _ => "?",
            };
        }
    }
}
