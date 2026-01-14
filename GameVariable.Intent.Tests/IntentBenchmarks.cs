using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GameVariable.Intent;

namespace GameVariable.Intent.Tests
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    public class IntentBenchmarks
    {
        private IntentData _optimizedData;
        private IntentState _originalData;
        // private IntentStateFast _fastData;
        // private IntentStateUnsafe _unsafeData;
        // private IntentStateJumpTable _jumpTableData;
        // private IntentStateBitManip _bitManipData;
        // private IntentStateCompiled _compiledData;

        [GlobalSetup]
        public void Setup()
        {
            _optimizedData = new IntentData(IntentLogic.CREATED);
            _originalData = new IntentState();
            _originalData.Start();
            // _fastData = new IntentStateFast();
            // _fastData.Start();
            // _unsafeData = new IntentStateUnsafe();
            // _unsafeData.Start();
            // _jumpTableData = new IntentStateJumpTable();
            // _jumpTableData.Start();
            // _bitManipData = new IntentStateBitManip();
            // _bitManipData.Start();
            // _compiledData = new IntentStateCompiled();
            // _compiledData.Start();
        }

        // BENCHMARK: Original Implementation (Switch-Case)
        [Benchmark(Baseline = true)]
        public void Original_Implementation_SingleDispatch()
        {
            var data = _originalData;
            data.DispatchEvent(IntentState.EventId.GET_READY);
            data.DispatchEvent(IntentState.EventId.ACTIVATED);
            data.DispatchEvent(IntentState.EventId.START_RUNNING);
            data.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _originalData = data;
        }

        // BENCHMARK: Optimized Implementation (Table-Based)
        [Benchmark]
        public void Optimized_Implementation_SingleDispatch()
        {
            var data = _optimizedData;
            data.TryDispatch(IntentExtensions.IntentEvent.GET_READY);
            data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
            data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            data.TryDispatch(IntentExtensions.IntentEvent.COMPLETED_SUCCESSFULLY);
            _optimizedData = data;
        }

        // BENCHMARK: Original - Complex Workflow (CANCELED path)
        [Benchmark]
        public void Original_CanceledWorkflow()
        {
            var data = _originalData;
            data.DispatchEvent(IntentState.EventId.ACTIVATED);
            data.DispatchEvent(IntentState.EventId.START_RUNNING);
            data.DispatchEvent(IntentState.EventId.CANCEL);
            _originalData = data;
        }

        // BENCHMARK: Optimized - Complex Workflow (CANCELED path)
        [Benchmark]
        public void Optimized_CanceledWorkflow()
        {
            var data = _optimizedData;
            data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
            data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            data.TryDispatch(IntentExtensions.IntentEvent.CANCEL);
            _optimizedData = data;
        }

        /*
        // BENCHMARK: Fast - Complex Workflow (CANCELED path)
        [Benchmark]
        public void Fast_CanceledWorkflow()
        {
            var data = _fastData;
            data.Dispatch(IntentState.EventId.ACTIVATED);
            data.Dispatch(IntentState.EventId.START_RUNNING);
            data.Dispatch(IntentState.EventId.CANCEL);
            _fastData = data;
        }

        // BENCHMARK: Fast - Single Dispatch
        [Benchmark]
        public void Fast_Implementation_SingleDispatch()
        {
            var data = _fastData;
            data.Dispatch(IntentState.EventId.GET_READY);
            data.Dispatch(IntentState.EventId.ACTIVATED);
            data.Dispatch(IntentState.EventId.START_RUNNING);
            data.Dispatch(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _fastData = data;
        }

        // EXPERIMENTAL: Unsafe pointers
        [Benchmark]
        public void Unsafe_Implementation_SingleDispatch()
        {
            var data = _unsafeData;
            data.Dispatch(IntentState.EventId.GET_READY);
            data.Dispatch(IntentState.EventId.ACTIVATED);
            data.Dispatch(IntentState.EventId.START_RUNNING);
            data.Dispatch(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _unsafeData = data;
        }

        // EXPERIMENTAL: Jump table
        [Benchmark]
        public void JumpTable_Implementation_SingleDispatch()
        {
            var data = _jumpTableData;
            data.Dispatch(IntentState.EventId.GET_READY);
            data.Dispatch(IntentState.EventId.ACTIVATED);
            data.Dispatch(IntentState.EventId.START_RUNNING);
            data.Dispatch(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _jumpTableData = data;
        }

        // EXPERIMENTAL: Bit manipulation
        [Benchmark]
        public void BitManip_Implementation_SingleDispatch()
        {
            var data = _bitManipData;
            data.Dispatch(IntentState.EventId.GET_READY);
            data.Dispatch(IntentState.EventId.ACTIVATED);
            data.Dispatch(IntentState.EventId.START_RUNNING);
            data.Dispatch(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _bitManipData = data;
        }

        // EXPERIMENTAL: AggressiveOptimization
        [Benchmark]
        public void Compiled_Implementation_SingleDispatch()
        {
            var data = _compiledData;
            data.Dispatch(IntentState.EventId.GET_READY);
            data.Dispatch(IntentState.EventId.ACTIVATED);
            data.Dispatch(IntentState.EventId.START_RUNNING);
            data.Dispatch(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _compiledData = data;
        }
        */

        // BENCHMARK: Original - Complete Lifecycle
        [Benchmark]
        public void Original_CompleteLifecycle()
        {
            var data = _originalData;
            // Created -> WaitingForActivation
            data.DispatchEvent(IntentState.EventId.GET_READY);
            // WaitingForActivation -> WaitingToRun
            data.DispatchEvent(IntentState.EventId.ACTIVATED);
            // WaitingToRun -> Running
            data.DispatchEvent(IntentState.EventId.START_RUNNING);
            // Running -> WaitingForChildren
            data.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
            // WaitingForChildren -> Running
            data.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
            // Running -> RanToCompletion
            data.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            _originalData = data;
        }

        // BENCHMARK: Optimized - Complete Lifecycle
        [Benchmark]
        public void Optimized_CompleteLifecycle()
        {
            var data = _optimizedData;
            // Created -> WaitingForActivation
            data.TryDispatch(IntentExtensions.IntentEvent.GET_READY);
            // WaitingForActivation -> WaitingToRun
            data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
            // WaitingToRun -> Running
            data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            // Running -> WaitingForChildren
            data.TryDispatch(IntentExtensions.IntentEvent.CHILD_TASK_CREATED);
            // WaitingForChildren -> Running
            data.TryDispatch(IntentExtensions.IntentEvent.ALL_CHILDREN_COMPLETED);
            // Running -> RanToCompletion
            data.TryDispatch(IntentExtensions.IntentEvent.COMPLETED_SUCCESSFULLY);
            _optimizedData = data;
        }

        // BENCHMARK: Original - No State Change (Terminal State)
        [Benchmark]
        public void Original_TerminalStateNoChange()
        {
            var data = _originalData;
            data.DispatchEvent(IntentState.EventId.ACTIVATED);
            data.DispatchEvent(IntentState.EventId.START_RUNNING);
            data.DispatchEvent(IntentState.EventId.UNABLE_TO_COMPLETE); // Goes to FAULTED
            // These events should not change state from FAULTED
            for (int i = 0; i < 10; i++)
            {
                data.DispatchEvent(IntentState.EventId.ACTIVATED);
                data.DispatchEvent(IntentState.EventId.START_RUNNING);
            }
            _originalData = data;
        }

        // BENCHMARK: Optimized - No State Change (Terminal State)
        [Benchmark]
        public void Optimized_TerminalStateNoChange()
        {
            var data = _optimizedData;
            data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
            data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            data.TryDispatch(IntentExtensions.IntentEvent.UNABLE_TO_COMPLETE); // Goes to FAULTED
            // These events should not change state from FAULTED
            for (int i = 0; i < 10; i++)
            {
                data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
                data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
            }
            _optimizedData = data;
        }

        // STRESS TEST: 1000 Dispatches - Original
        [Benchmark]
        public void Original_1000Dispatches()
        {
            var data = _originalData;
            for (int i = 0; i < 1000; i++)
            {
                data.DispatchEvent(IntentState.EventId.ACTIVATED);
                data.DispatchEvent(IntentState.EventId.START_RUNNING);
                data.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
                data.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
                data.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
                data.DispatchEvent(IntentState.EventId.RUN_AGAIN);
            }
            _originalData = data;
        }

        // STRESS TEST: 1000 Dispatches - Optimized
        [Benchmark]
        public void Optimized_1000Dispatches()
        {
            var data = _optimizedData;
            for (int i = 0; i < 1000; i++)
            {
                data.TryDispatch(IntentExtensions.IntentEvent.ACTIVATED);
                data.TryDispatch(IntentExtensions.IntentEvent.START_RUNNING);
                data.TryDispatch(IntentExtensions.IntentEvent.CHILD_TASK_CREATED);
                data.TryDispatch(IntentExtensions.IntentEvent.ALL_CHILDREN_COMPLETED);
                data.TryDispatch(IntentExtensions.IntentEvent.COMPLETED_SUCCESSFULLY);
                data.TryDispatch(IntentExtensions.IntentEvent.RUN_AGAIN);
            }
            _optimizedData = data;
        }
    }
}
