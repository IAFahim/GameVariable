using Xunit;
using GameVariable.Intent;
using System;

namespace GameVariable.Intent.Tests
{
    public class IntentTests
    {
        [Fact]
        public void IntentData_InitialState_IsCorrect()
        {
            var data = new IntentData(IntentLogic.CREATED);
            Assert.Equal(IntentLogic.CREATED, data.StateId);
            Assert.Equal(32, data.StateOffset); // 2 * 16
        }

        [Fact]
        public void IntentData_StateIdFromOffset_IsCorrect()
        {
            var data = new IntentData(IntentLogic.RUNNING);
            Assert.Equal(IntentLogic.RUNNING, data.StateId);
            Assert.Equal(80, data.StateOffset); // 5 * 16
        }

        [Fact]
        public void IntentLogic_OffsetToStateId_IsCorrect()
        {
            Assert.Equal(0, IntentLogic.OffsetToStateId(0));
            Assert.Equal(1, IntentLogic.OffsetToStateId(16));
            Assert.Equal(2, IntentLogic.OffsetToStateId(32));
            Assert.Equal(5, IntentLogic.OffsetToStateId(80));
            Assert.Equal(8, IntentLogic.OffsetToStateId(128));
        }

        [Fact]
        public void IntentLogic_StateIdToOffset_IsCorrect()
        {
            Assert.Equal(0, IntentLogic.StateIdToOffset(0));
            Assert.Equal(16, IntentLogic.StateIdToOffset(1));
            Assert.Equal(32, IntentLogic.StateIdToOffset(2));
            Assert.Equal(80, IntentLogic.StateIdToOffset(5));
            Assert.Equal(128, IntentLogic.StateIdToOffset(8));
        }

        // Test: CREATED -> GET_READY -> WAITING_FOR_ACTIVATION
        [Fact]
        public void Apply_Created_GetReady_GoesToWaitingForActivation()
        {
            var data = new IntentData(IntentLogic.CREATED);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.GET_READY, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(96, nextOffset); // WAITING_FOR_ACTIVATION offset
        }

        // Test: CREATED -> ACTIVATED -> WAITING_TO_RUN
        [Fact]
        public void Apply_Created_Activated_GoesToWaitingToRun()
        {
            var data = new IntentData(IntentLogic.CREATED);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.ACTIVATED, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(128, nextOffset); // WAITING_TO_RUN offset
        }

        // Test: WAITING_TO_RUN -> START_RUNNING -> RUNNING
        [Fact]
        public void Apply_WaitingToRun_StartRunning_GoesToRunning()
        {
            var data = new IntentData(IntentLogic.WAITING_TO_RUN);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.START_RUNNING, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(80, nextOffset); // RUNNING offset
        }

        // Test: WAITING_TO_RUN -> CANCELED_BEFORE_RUN -> CANCELED
        [Fact]
        public void Apply_WaitingToRun_CanceledBeforeRun_GoesToCanceled()
        {
            var data = new IntentData(IntentLogic.WAITING_TO_RUN);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.CANCELED_BEFORE_RUN, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(16, nextOffset); // CANCELED offset
        }

        // Test: WAITING_FOR_ACTIVATION -> ACTIVATED -> WAITING_TO_RUN
        [Fact]
        public void Apply_WaitingForActivation_Activated_GoesToWaitingToRun()
        {
            var data = new IntentData(IntentLogic.WAITING_FOR_ACTIVATION);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.ACTIVATED, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(128, nextOffset); // WAITING_TO_RUN offset
        }

        // Test: WAITING_FOR_ACTIVATION -> CANCELED_BEFORE_ACTIVATION -> CANCELED
        [Fact]
        public void Apply_WaitingForActivation_CanceledBeforeActivation_GoesToCanceled()
        {
            var data = new IntentData(IntentLogic.WAITING_FOR_ACTIVATION);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.CANCELED_BEFORE_ACTIVATION, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(16, nextOffset); // CANCELED offset
        }

        // Test: RUNNING -> CHILD_TASK_CREATED -> WAITING_FOR_CHILDREN_TO_COMPLETE
        [Fact]
        public void Apply_Running_ChildTaskCreated_GoesToWaitingForChildren()
        {
            var data = new IntentData(IntentLogic.RUNNING);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.CHILD_TASK_CREATED, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(112, nextOffset); // WAITING_FOR_CHILDREN_TO_COMPLETE offset
        }

        // Test: RUNNING -> UNABLE_TO_COMPLETE -> FAULTED
        [Fact]
        public void Apply_Running_UnableToComplete_GoesToFaulted()
        {
            var data = new IntentData(IntentLogic.RUNNING);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.UNABLE_TO_COMPLETE, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(48, nextOffset); // FAULTED offset
        }

        // Test: RUNNING -> CANCEL -> CANCELED
        [Fact]
        public void Apply_Running_Cancel_GoesToCanceled()
        {
            var data = new IntentData(IntentLogic.RUNNING);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.CANCEL, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(16, nextOffset); // CANCELED offset
        }

        // Test: RUNNING -> COMPLETED_SUCCESSFULLY -> RAN_TO_COMPLETION
        [Fact]
        public void Apply_Running_CompletedSuccessfully_GoesToRanToCompletion()
        {
            var data = new IntentData(IntentLogic.RUNNING);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.COMPLETED_SUCCESSFULLY, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(64, nextOffset); // RAN_TO_COMPLETION offset
        }

        // Test: WAITING_FOR_CHILDREN_TO_COMPLETE -> ALL_CHILDREN_COMPLETED -> RUNNING
        [Fact]
        public void Apply_WaitingForChildren_AllChildrenCompleted_GoesToRunning()
        {
            var data = new IntentData(IntentLogic.WAITING_FOR_CHILDREN_TO_COMPLETE);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.ALL_CHILDREN_COMPLETED, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(80, nextOffset); // RUNNING offset
        }

        // Test: RAN_TO_COMPLETION -> RUN_AGAIN -> WAITING_TO_RUN
        [Fact]
        public void Apply_RanToCompletion_RunAgain_GoesToWaitingToRun()
        {
            var data = new IntentData(IntentLogic.RAN_TO_COMPLETION);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.RUN_AGAIN, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(128, nextOffset); // WAITING_TO_RUN offset
        }

        // Test: Terminal states don't change on invalid events
        [Theory]
        [InlineData(IntentLogic.CANCELED)]
        [InlineData(IntentLogic.FAULTED)]
        [InlineData(IntentLogic.ROOT)]
        public void Apply_TerminalStates_NoChange(byte terminalState)
        {
            var data = new IntentData(terminalState);
            IntentLogic.Apply(in data.StateOffset, IntentLogic.ACTIVATED, out byte nextOffset, out bool changed);

            Assert.False(changed);
            Assert.Equal(data.StateOffset, nextOffset);
        }

        // Test: Extension method TryDispatch
        [Fact]
        public void TryDispatch_CompleteWorkflow_ReturnsCorrectChanges()
        {
            var data = new IntentData(IntentLogic.CREATED);

            // CREATED -> GET_READY -> WAITING_FOR_ACTIVATION
            bool changed1 = data.TryDispatch(IntentExtensions.IntentEvent.GET_READY);
            Assert.True(changed1);
            Assert.Equal(IntentExtensions.IntentState.WAITING_FOR_ACTIVATION, data.GetState());

            // WAITING_FOR_ACTIVATION -> ACTIVATED -> WAITING_TO_RUN
            bool changed2 = data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
            Assert.True(changed2);
            Assert.Equal(IntentExtensions.IntentState.WAITING_TO_RUN, data.GetState());

            // WAITING_TO_RUN -> START_RUNNING -> RUNNING
            bool changed3 = data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            Assert.True(changed3);
            Assert.Equal(IntentExtensions.IntentState.RUNNING, data.GetState());
        }

        // Test: Extension method Start
        [Fact]
        public void Start_SetsStateToCreated()
        {
            var data = new IntentData();
            data.Start();

            Assert.Equal(IntentLogic.CREATED, data.StateId);
            Assert.Equal(IntentExtensions.IntentState.CREATED, data.GetState());
        }

        // Test: Full lifecycle
        [Fact]
        public void FullLifecycle_CompleteExecution_EndsInRanToCompletion()
        {
            var data = new IntentData(IntentLogic.CREATED);

            // Created -> WaitingForActivation
            data.TryDispatch(IntentExtensions.IntentEvent.GET_READY);
            Assert.Equal(IntentExtensions.IntentState.WAITING_FOR_ACTIVATION, data.GetState());

            // WaitingForActivation -> WaitingToRun
            data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
            Assert.Equal(IntentExtensions.IntentState.WAITING_TO_RUN, data.GetState());

            // WaitingToRun -> Running
            data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            Assert.Equal(IntentExtensions.IntentState.RUNNING, data.GetState());

            // Running -> WaitingForChildren
            data.TryDispatch(IntentExtensions.IntentEvent.CHILD_TASK_CREATED);
            Assert.Equal(IntentExtensions.IntentState.WAITING_FOR_CHILDREN_TO_COMPLETE, data.GetState());

            // WaitingForChildren -> Running
            data.TryDispatch(IntentExtensions.IntentEvent.ALL_CHILDREN_COMPLETED);
            Assert.Equal(IntentExtensions.IntentState.RUNNING, data.GetState());

            // Running -> RanToCompletion
            data.TryDispatch(IntentExtensions.IntentEvent.COMPLETED_SUCCESSFULLY);
            Assert.Equal(IntentExtensions.IntentState.RAN_TO_COMPLETION, data.GetState());
        }

        // Test: Cancellation workflow
        [Fact]
        public void CancellationWorkflow_FromRunning_GoesToCanceled()
        {
            var data = new IntentData(IntentLogic.RUNNING);

            // Running -> CANCELED
            bool changed = data.TryDispatch(IntentExtensions.IntentEvent.CANCEL);
            Assert.True(changed);
            Assert.Equal(IntentExtensions.IntentState.CANCELED, data.GetState());

            // CANCELED is terminal - further events don't change state
            bool changed2 = data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            Assert.False(changed2);
            Assert.Equal(IntentExtensions.IntentState.CANCELED, data.GetState());
        }

        // Test: Run again workflow
        [Fact]
        public void RunAgainWorkflow_RanToCompletion_CanRunAgain()
        {
            var data = new IntentData(IntentLogic.RAN_TO_COMPLETION);

            // RanToCompletion -> WaitingToRun
            bool changed = data.TryDispatch(IntentExtensions.IntentEvent.RUN_AGAIN);
            Assert.True(changed);
            Assert.Equal(IntentExtensions.IntentState.WAITING_TO_RUN, data.GetState());

            // WaitingToRun -> Running
            data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            Assert.Equal(IntentExtensions.IntentState.RUNNING, data.GetState());

            // Running -> RanToCompletion
            data.TryDispatch(IntentExtensions.IntentEvent.COMPLETED_SUCCESSFULLY);
            Assert.Equal(IntentExtensions.IntentState.RAN_TO_COMPLETION, data.GetState());
        }

        // Test: Faulted workflow
        [Fact]
        public void FaultedWorkflow_FromRunning_GoesToFaulted()
        {
            var data = new IntentData(IntentLogic.RUNNING);

            // Running -> FAULTED
            bool changed = data.TryDispatch(IntentExtensions.IntentEvent.UNABLE_TO_COMPLETE);
            Assert.True(changed);
            Assert.Equal(IntentExtensions.IntentState.FAULTED, data.GetState());

            // FAULTED is terminal
            bool changed2 = data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            Assert.False(changed2);
            Assert.Equal(IntentExtensions.IntentState.FAULTED, data.GetState());
        }

        // Test: String conversion methods
        [Fact]
        public void StringConversion_StateAndEvent_ReturnsCorrectStrings()
        {
            Assert.Equal("RUNNING", IntentExtensions.StateIdToString(IntentExtensions.IntentState.RUNNING));
            Assert.Equal("CANCELED", IntentExtensions.StateIdToString(IntentExtensions.IntentState.CANCELED));
            Assert.Equal("COMPLETED_SUCCESSFULLY", IntentExtensions.EventIdToString(IntentExtensions.IntentEvent.COMPLETED_SUCCESSFULLY));
            Assert.Equal("START_RUNNING", IntentExtensions.EventIdToString(IntentExtensions.IntentEvent.START_RUNNING));
        }

        // Test: All state transitions match original implementation
        [Fact]
        public void OptimizedMatchesOriginal_AllTransitions()
        {
            // Test each state transition from original implementation
            var testData = new[]
            {
                (IntentLogic.CREATED, IntentLogic.GET_READY, IntentLogic.WAITING_FOR_ACTIVATION),
                (IntentLogic.CREATED, IntentLogic.ACTIVATED, IntentLogic.WAITING_TO_RUN),
                (IntentLogic.WAITING_TO_RUN, IntentLogic.START_RUNNING, IntentLogic.RUNNING),
                (IntentLogic.WAITING_TO_RUN, IntentLogic.CANCELED_BEFORE_RUN, IntentLogic.CANCELED),
                (IntentLogic.WAITING_FOR_ACTIVATION, IntentLogic.ACTIVATED, IntentLogic.WAITING_TO_RUN),
                (IntentLogic.WAITING_FOR_ACTIVATION, IntentLogic.CANCELED_BEFORE_ACTIVATION, IntentLogic.CANCELED),
                (IntentLogic.RUNNING, IntentLogic.CHILD_TASK_CREATED, IntentLogic.WAITING_FOR_CHILDREN_TO_COMPLETE),
                (IntentLogic.RUNNING, IntentLogic.UNABLE_TO_COMPLETE, IntentLogic.FAULTED),
                (IntentLogic.RUNNING, IntentLogic.CANCEL, IntentLogic.CANCELED),
                (IntentLogic.RUNNING, IntentLogic.COMPLETED_SUCCESSFULLY, IntentLogic.RAN_TO_COMPLETION),
                (IntentLogic.WAITING_FOR_CHILDREN_TO_COMPLETE, IntentLogic.ALL_CHILDREN_COMPLETED, IntentLogic.RUNNING),
                (IntentLogic.RAN_TO_COMPLETION, IntentLogic.RUN_AGAIN, IntentLogic.WAITING_TO_RUN),
            };

            foreach (var (fromState, eventId, expectedState) in testData)
            {
                var data = new IntentData(fromState);
                IntentLogic.Apply(in data.StateOffset, eventId, out byte nextOffset, out bool changed);

                Assert.True(changed, $"Transition from {fromState} on event {eventId} should change state");
                Assert.Equal(expectedState, IntentLogic.OffsetToStateId(nextOffset));
            }
        }

        // Test: Event ID masking (events > 15 should be masked)
        [Fact]
        public void Apply_EventIdMasking_HandlesLargeEventIds()
        {
            var data = new IntentData(IntentLogic.CREATED);
            // Event ID 16 + 7 (GET_READY) = 23, should be masked to 7
            IntentLogic.Apply(in data.StateOffset, 23, out byte nextOffset, out bool changed);

            Assert.True(changed);
            Assert.Equal(96, nextOffset); // WAITING_FOR_ACTIVATION offset
        }
    }
}
