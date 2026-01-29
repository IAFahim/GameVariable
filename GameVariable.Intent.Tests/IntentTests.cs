using Xunit;
using GameVariable.Intent;
using System;

namespace GameVariable.Intent.Tests
{
    /// <summary>
    /// Comprehensive tests for the Intent State Machine.
    /// Verifies all state transitions and event flows.
    /// </summary>
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

        // === Happy Path Tests (Green Flow) ===

        [Fact]
        public void HappyPath_Created_To_Inactive_Via_Prepare()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.PREPARE);

            Assert.Equal(IntentState.StateId.INACTIVE, state.stateId);
        }

        [Fact]
        public void HappyPath_Inactive_To_Pending_Via_Activate()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.PREPARE);
            state.DispatchEvent(IntentState.EventId.ACTIVATE);

            Assert.Equal(IntentState.StateId.PENDING, state.stateId);
        }

        [Fact]
        public void HappyPath_Created_To_Pending_Via_Activate()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);

            Assert.Equal(IntentState.StateId.PENDING, state.stateId);
        }

        [Fact]
        public void HappyPath_Pending_To_Running_Via_Start()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);

            Assert.Equal(IntentState.StateId.RUNNING, state.stateId);
        }

        [Fact]
        public void HappyPath_Running_To_Completed_Via_Complete()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.COMPLETE);

            Assert.Equal(IntentState.StateId.COMPLETED, state.stateId);
        }

        // === Child Process Flow Tests (Blue Flow) ===

        [Fact]
        public void ChildProcess_Running_To_Blocked_Via_SpawnChild()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.SPAWN_CHILD);

            Assert.Equal(IntentState.StateId.BLOCKED, state.stateId);
        }

        [Fact]
        public void ChildProcess_Blocked_To_Running_Via_Resume()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.SPAWN_CHILD);
            state.DispatchEvent(IntentState.EventId.RESUME);

            Assert.Equal(IntentState.StateId.RUNNING, state.stateId);
        }

        // === Recovery Flow Tests (Goldenrod Flow) ===

        [Fact]
        public void Recovery_Completed_To_Pending_Via_Restart()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.COMPLETE);
            state.DispatchEvent(IntentState.EventId.RESTART);

            Assert.Equal(IntentState.StateId.PENDING, state.stateId);
        }

        [Fact]
        public void Recovery_Faulted_To_Pending_Via_Recover()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.FAIL);
            state.DispatchEvent(IntentState.EventId.RECOVER);

            Assert.Equal(IntentState.StateId.PENDING, state.stateId);
        }

        // === Error Flow Tests (Red Flow) ===

        [Fact]
        public void Error_Running_To_Faulted_Via_Fail()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.FAIL);

            Assert.Equal(IntentState.StateId.FAULTED, state.stateId);
        }

        [Fact]
        public void Error_Blocked_To_Faulted_Via_Abort()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.SPAWN_CHILD);
            state.DispatchEvent(IntentState.EventId.ABORT);

            Assert.Equal(IntentState.StateId.FAULTED, state.stateId);
        }

        // === Cancellation Flow Tests (Gray Flow) ===

        [Fact]
        public void Cancellation_Created_To_Cancelled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELLED, state.stateId);
        }

        [Fact]
        public void Cancellation_Inactive_To_Cancelled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.PREPARE);
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELLED, state.stateId);
        }

        [Fact]
        public void Cancellation_Pending_To_Cancelled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELLED, state.stateId);
        }

        [Fact]
        public void Cancellation_Running_To_Cancelled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELLED, state.stateId);
        }

        [Fact]
        public void Cancellation_Blocked_To_Cancelled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.SPAWN_CHILD);
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELLED, state.stateId);
        }

        [Fact]
        public void Cancellation_Faulted_To_Cancelled()
        {
            var state = new IntentState();
            state.Start();
            state.DispatchEvent(IntentState.EventId.ACTIVATE);
            state.DispatchEvent(IntentState.EventId.START);
            state.DispatchEvent(IntentState.EventId.FAIL);
            state.DispatchEvent(IntentState.EventId.CANCEL);

            Assert.Equal(IntentState.StateId.CANCELLED, state.stateId);
        }

        // === String Conversion Tests ===

        [Fact]
        public void StringConversion_StateId_ReturnsCorrectStrings()
        {
            Assert.Equal("CREATED", IntentState.StateIdToString(IntentState.StateId.CREATED));
            Assert.Equal("INACTIVE", IntentState.StateIdToString(IntentState.StateId.INACTIVE));
            Assert.Equal("PENDING", IntentState.StateIdToString(IntentState.StateId.PENDING));
            Assert.Equal("RUNNING", IntentState.StateIdToString(IntentState.StateId.RUNNING));
            Assert.Equal("BLOCKED", IntentState.StateIdToString(IntentState.StateId.BLOCKED));
            Assert.Equal("COMPLETED", IntentState.StateIdToString(IntentState.StateId.COMPLETED));
            Assert.Equal("FAULTED", IntentState.StateIdToString(IntentState.StateId.FAULTED));
            Assert.Equal("CANCELLED", IntentState.StateIdToString(IntentState.StateId.CANCELLED));
        }

        [Fact]
        public void StringConversion_EventId_ReturnsCorrectStrings()
        {
            Assert.Equal("PREPARE", IntentState.EventIdToString(IntentState.EventId.PREPARE));
            Assert.Equal("ACTIVATE", IntentState.EventIdToString(IntentState.EventId.ACTIVATE));
            Assert.Equal("START", IntentState.EventIdToString(IntentState.EventId.START));
            Assert.Equal("COMPLETE", IntentState.EventIdToString(IntentState.EventId.COMPLETE));
            Assert.Equal("SPAWN_CHILD", IntentState.EventIdToString(IntentState.EventId.SPAWN_CHILD));
            Assert.Equal("RESUME", IntentState.EventIdToString(IntentState.EventId.RESUME));
            Assert.Equal("RESTART", IntentState.EventIdToString(IntentState.EventId.RESTART));
            Assert.Equal("RECOVER", IntentState.EventIdToString(IntentState.EventId.RECOVER));
            Assert.Equal("FAIL", IntentState.EventIdToString(IntentState.EventId.FAIL));
            Assert.Equal("ABORT", IntentState.EventIdToString(IntentState.EventId.ABORT));
            Assert.Equal("CANCEL", IntentState.EventIdToString(IntentState.EventId.CANCEL));
        }
    }
}
