using System.Runtime.CompilerServices;

namespace GameVariable.Intent
{
    /// <summary>
    /// Experimental struct wrappers for different optimization approaches
    /// </summary>

    // Unsafe approach
    public unsafe struct IntentStateUnsafe
    {
        public IntentState.StateId state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dispatch(IntentState.EventId evt)
        {
            return IntentUnsafe.Transition(ref state, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            state = IntentState.StateId.CREATED;
        }
    }

    // Jump table approach
    public struct IntentStateJumpTable
    {
        public IntentState.StateId state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dispatch(IntentState.EventId evt)
        {
            return IntentJumpTable.Transition(ref state, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            state = IntentState.StateId.CREATED;
        }
    }

    // Bit manipulation approach
    public struct IntentStateBitManip
    {
        public IntentState.StateId state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dispatch(IntentState.EventId evt)
        {
            return IntentBitManip.Transition(ref state, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            state = IntentState.StateId.CREATED;
        }
    }

    // Atomic approach
    public struct IntentStateAtomic
    {
        public IntentState.StateId state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dispatch(IntentState.EventId evt)
        {
            return IntentAtomic.Transition(ref state, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            state = IntentState.StateId.CREATED;
        }
    }

    // Compiled with AggressiveInlining
    public struct IntentStateCompiled
    {
        public IntentState.StateId state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Dispatch(IntentState.EventId evt)
        {
            return IntentCompiled.Transition(ref state, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            state = IntentState.StateId.CREATED;
        }
    }
}
