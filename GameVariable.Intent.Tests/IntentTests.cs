using Xunit;
using GameVariable.Intent;
using System;

namespace GameVariable.Intent.Tests
{
    public class IntentTests
    {
        [Fact]
        public void InitialState_IsRoot_BeforeStart()
        {
            var state = new IntentState();
            Assert.Equal(IntentState.StateId.ROOT, state.stateId);
        }

        [Fact]
        public void Start_GoesToCreated()
        {
            var state = new IntentState();
            state.Start();
            Assert.Equal(IntentState.StateId.CREATED, state.stateId);
        }

        // Test: CREATED -> GET_READY -> WAITING_FOR_ACTIVATION
        [Fact]
        public void Dispatch_Created_GetReady_GoesToWaitingForActivation()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.GET_READY);

            Assert.Equal(IntentState.StateId.WAITING_FOR_ACTIVATION, state.stateId);
        }

        // Test: CREATED -> ACTIVATED -> WAITING_TO_RUN
        [Fact]
        public void Dispatch_Created_Activated_GoesToWaitingToRun()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);

            Assert.Equal(IntentState.StateId.WAITING_TO_RUN, state.stateId);
        }

        // Test: WAITING_TO_RUN -> START_RUNNING -> RUNNING
        [Fact]
        public void Dispatch_WaitingToRun_StartRunning_GoesToRunning()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED); // To WaitingToRun
            state.DispatchEvent(IntentState.EventId.START_RUNNING);

            Assert.Equal(IntentState.StateId.RUNNING, state.stateId);
        }

        // Test: WAITING_TO_RUN -> CANCEL -> CANCELED
        [Fact]
        public void Dispatch_WaitingToRun_Cancel_GoesToCanceled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED); // To WaitingToRun
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELED, state.stateId);
        }

        // Test: WAITING_FOR_ACTIVATION -> ACTIVATED -> WAITING_TO_RUN
        [Fact]
        public void Dispatch_WaitingForActivation_Activated_GoesToWaitingToRun()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.GET_READY); // To WaitingForActivation
            state.DispatchEvent(IntentState.EventId.ACTIVATED);

            Assert.Equal(IntentState.StateId.WAITING_TO_RUN, state.stateId);
        }

        // Test: RUNNING -> CHILD_TASK_CREATED -> WAITING_FOR_CHILDREN_TO_COMPLETE
        [Fact]
        public void Dispatch_Running_ChildTaskCreated_GoesToWaitingForChildren()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
            state.DispatchEvent(IntentState.EventId.START_RUNNING);
            state.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);

            Assert.Equal(IntentState.StateId.WAITING_FOR_CHILDREN_TO_COMPLETE, state.stateId);
        }

        // Test: RUNNING -> UNABLE_TO_COMPLETE -> FAULTED
        [Fact]
        public void Dispatch_Running_UnableToComplete_GoesToFaulted()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
            state.DispatchEvent(IntentState.EventId.START_RUNNING);
            state.DispatchEvent(IntentState.EventId.UNABLE_TO_COMPLETE);

            Assert.Equal(IntentState.StateId.FAULTED, state.stateId);
        }

        // Test: RUNNING -> COMPLETED_SUCCESSFULLY -> RAN_TO_COMPLETION
        [Fact]
        public void Dispatch_Running_CompletedSuccessfully_GoesToRanToCompletion()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
            state.DispatchEvent(IntentState.EventId.START_RUNNING);
            state.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);

            Assert.Equal(IntentState.StateId.RAN_TO_COMPLETION, state.stateId);
        }

        // Test: WAITING_FOR_CHILDREN_TO_COMPLETE -> ALL_CHILDREN_COMPLETED -> RUNNING
        [Fact]
        public void Dispatch_WaitingForChildren_AllChildrenCompleted_GoesToRunning()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
            state.DispatchEvent(IntentState.EventId.START_RUNNING);
            state.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
            state.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);

            Assert.Equal(IntentState.StateId.RUNNING, state.stateId);
        }

        // Test: RAN_TO_COMPLETION -> RUN_AGAIN -> WAITING_TO_RUN
        [Fact]
        public void Dispatch_RanToCompletion_RunAgain_GoesToWaitingToRun()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATED);
            state.DispatchEvent(IntentState.EventId.START_RUNNING);
            state.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            state.DispatchEvent(IntentState.EventId.RUN_AGAIN);

            Assert.Equal(IntentState.StateId.WAITING_TO_RUN, state.stateId);
        }

        // Test: String conversion methods
        [Fact]
        public void StringConversion_StateAndEvent_ReturnsCorrectStrings()
        {
            var state = new IntentState();
            Assert.Equal("RUNNING", state.StateIdToString(IntentState.StateId.RUNNING));
            Assert.Equal("CANCELED", state.StateIdToString(IntentState.StateId.CANCELED));
            Assert.Equal("COMPLETED_SUCCESSFULLY", state.EventIdToString(IntentState.EventId.COMPLETED_SUCCESSFULLY));
            Assert.Equal("START_RUNNING", state.EventIdToString(IntentState.EventId.START_RUNNING));
        }
    }
}
