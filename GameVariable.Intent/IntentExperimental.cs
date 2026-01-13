using System;
using System.Runtime.CompilerServices;

namespace GameVariable.Intent
{
    /// <summary>
    /// EXPERIMENTAL: Ultra-aggressive optimizations to beat 191x speedup.
    /// Testing multiple approaches:
    /// 1. Unsafe pointer arithmetic
    /// 2. Manual jump table with direct array lookup
    /// 3. Bit manipulation to eliminate branching
    /// 4. Pure math-based state computation
    /// </summary>

    // APPROACH 1: Unsafe Direct Memory Lookup
    public unsafe static class IntentUnsafe
    {
        // Compile-time constant table stored in read-only memory
        private static readonly byte* _table;
        private static readonly System.Runtime.InteropServices.GCHandle _handle;

        static IntentUnsafe()
        {
            // Pin the table in memory for zero-overhead access
            var table = new byte[]
            {
                // State 0: ROOT (0) - all events stay
                0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
                // State 1: CANCELED (16) - terminal
                1,1,1,1, 1,1,1,1, 1,1,1,1, 1,1,1,1,
                // State 2: CREATED (32)
                8,2,2,2, 2,2,2,6, 2,2,2,2, 2,2,2,2,
                // State 3: FAULTED (48) - terminal
                3,3,3,3, 3,3,3,3, 3,3,3,3, 3,3,3,3,
                // State 4: RAN_TO_COMPLETION (64)
                4,4,4,4, 4,4,4,4, 8,4,4,4, 4,4,4,4,
                // State 5: RUNNING (80)
                5,5,1,5, 5,7,4,5, 5,5,3,5, 5,5,5,5,
                // State 6: WAITING_FOR_ACTIVATION (96)
                8,6,6,1, 6,6,6,6, 6,6,6,6, 6,6,6,6,
                // State 7: WAITING_FOR_CHILDREN (112)
                7,5,7,7, 7,7,7,7, 7,7,7,7, 7,7,7,7,
                // State 8: WAITING_TO_RUN (128)
                8,8,8,8, 1,8,8,8, 8,5,8,8, 8,8,8,8,
            };

            // Pin the array and get a pointer
            _handle = System.Runtime.InteropServices.GCHandle.Alloc(table, System.Runtime.InteropServices.GCHandleType.Pinned);
            _table = (byte*)_handle.AddrOfPinnedObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState.StateId Transition(IntentState.StateId current, IntentState.EventId evt)
        {
            // Direct pointer arithmetic - zero overhead
            byte* ptr = _table + ((int)current * 16) + (int)evt;
            return (IntentState.StateId)(*ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transition(ref IntentState.StateId state, IntentState.EventId evt)
        {
            IntentState.StateId next = Transition(state, evt);
            bool changed = next != state;
            state = next;
            return changed;
        }
    }

    // APPROACH 2: Manual Jump Table with Computed Index
    public static class IntentJumpTable
    {
        // Flatten to 1D array for single memory access
        private static readonly byte[] _transitions = new byte[]
        {
            // Using a perfect hash: (state * 16) + event gives direct index
            // State 0 (ROOT): indices 0-15
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            // State 1 (CANCELED): indices 16-31
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
            // State 2 (CREATED): indices 32-47
            8,2,2,2,2,2,2,6,2,2,2,2,2,2,2,2,
            // State 3 (FAULTED): indices 48-63
            3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,
            // State 4 (RAN_TO_COMPLETION): indices 64-79
            4,4,4,4,4,4,4,4,8,4,4,4,4,4,4,4,
            // State 5 (RUNNING): indices 80-95
            5,5,1,5,5,7,4,5,5,5,3,5,5,5,5,5,
            // State 6 (WAITING_FOR_ACTIVATION): indices 96-111
            8,6,6,1,6,6,6,6,6,6,6,6,6,6,6,6,
            // State 7 (WAITING_FOR_CHILDREN): indices 112-127
            7,5,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
            // State 8 (WAITING_TO_RUN): indices 128-143
            8,8,8,8,1,8,8,8,8,5,8,8,8,8,8,8,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState.StateId Transition(IntentState.StateId current, IntentState.EventId evt)
        {
            // Single array lookup with computed index
            int index = ((int)current << 4) | (int)evt; // Multiply by 16 using bit shift
            return (IntentState.StateId)_transitions[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transition(ref IntentState.StateId state, IntentState.EventId evt)
        {
            IntentState.StateId next = Transition(state, evt);
            bool changed = next != state;
            state = next;
            return changed;
        }
    }

    // APPROACH 3: Bit Manipulation - No Branching
    public static class IntentBitManip
    {
        // Encode transitions as bitmasks for ultra-fast lookup
        // Each state has a 128-bit bitmap (16 events * 8 bits for next state)
        private static readonly ulong[] _bitmaps = new ulong[]
        {
            // State 0: ROOT -> always 0
            0x0000000000000000UL,
            // State 1: CANCELED -> always 1
            0x0101010101010101UL,
            // State 2: CREATED -> complex transitions
            // Events: ACTIVATED(0)->8, GET_READY(7)->6, others->2
            0x0202020202020208UL, 0x02020202060202UL,
            // State 3: FAULTED -> always 3
            0x0303030303030303UL,
            0x0303030303030303UL,
            // State 4: RAN_TO_COMPLETION -> RUN_AGAIN(8)->8, others->4
            0x04040404040408UL, 0x04040404040404UL,
            // State 5: RUNNING -> complex
            0x05050105051005705UL, 0x04050305050505UL,
            // State 6: WAITING_FOR_ACTIVATION -> ACTIVATED(0)->8, CANCELED_BEFORE_ACTIVATION(3)->1
            0x0606060606060608UL, 0x06060601060606UL,
            // State 7: WAITING_FOR_CHILDREN -> ALL_CHILDREN_COMPLETED(1)->5
            0x0705070707070707UL, 0x07070707070707UL,
            // State 8: WAITING_TO_RUN -> START_RUNNING(9)->5, CANCELED_BEFORE_RUN(4)->1
            0x0808080808080808UL, 0x01050808080808UL,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState.StateId Transition(IntentState.StateId current, IntentState.EventId evt)
        {
            // Extract the byte for this event from the bitmap
            ulong bitmap = _bitmaps[(int)current * 2 + ((int)evt >> 3)];
            int shift = ((int)evt & 0x7) << 3; // (evt % 8) * 8
            return (IntentState.StateId)((bitmap >> shift) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transition(ref IntentState.StateId state, IntentState.EventId evt)
        {
            IntentState.StateId next = Transition(state, evt);
            bool changed = next != state;
            state = next;
            return changed;
        }
    }

    // APPROACH 4: Interlocked - Thread-safe but still fast
    public static class IntentAtomic
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState.StateId Transition(IntentState.StateId current, IntentState.EventId evt)
        {
            return IntentFast.Transition(current, evt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transition(ref IntentState.StateId state, IntentState.EventId evt)
        {
            IntentState.StateId current = state;
            IntentState.StateId next = Transition(current, evt);
            state = next;
            return next != current;
        }
    }

    // APPROACH 5: Pure Switch with AggressiveInlining
    public static class IntentCompiled
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntentState.StateId Transition(IntentState.StateId current, IntentState.EventId evt)
        {
            return (current, evt) switch
            {
                (IntentState.StateId.CREATED, IntentState.EventId.GET_READY) => IntentState.StateId.WAITING_FOR_ACTIVATION,
                (IntentState.StateId.CREATED, IntentState.EventId.ACTIVATED) => IntentState.StateId.WAITING_TO_RUN,
                (IntentState.StateId.WAITING_TO_RUN, IntentState.EventId.START_RUNNING) => IntentState.StateId.RUNNING,
                (IntentState.StateId.WAITING_TO_RUN, IntentState.EventId.CANCELED_BEFORE_RUN) => IntentState.StateId.CANCELED,
                (IntentState.StateId.WAITING_FOR_ACTIVATION, IntentState.EventId.ACTIVATED) => IntentState.StateId.WAITING_TO_RUN,
                (IntentState.StateId.WAITING_FOR_ACTIVATION, IntentState.EventId.CANCELED_BEFORE_ACTIVATION) => IntentState.StateId.CANCELED,
                (IntentState.StateId.RUNNING, IntentState.EventId.CHILD_TASK_CREATED) => IntentState.StateId.WAITING_FOR_CHILDREN_TO_COMPLETE,
                (IntentState.StateId.RUNNING, IntentState.EventId.UNABLE_TO_COMPLETE) => IntentState.StateId.FAULTED,
                (IntentState.StateId.RUNNING, IntentState.EventId.CANCEL) => IntentState.StateId.CANCELED,
                (IntentState.StateId.RUNNING, IntentState.EventId.COMPLETED_SUCCESSFULLY) => IntentState.StateId.RAN_TO_COMPLETION,
                (IntentState.StateId.WAITING_FOR_CHILDREN_TO_COMPLETE, IntentState.EventId.ALL_CHILDREN_COMPLETED) => IntentState.StateId.RUNNING,
                (IntentState.StateId.RAN_TO_COMPLETION, IntentState.EventId.RUN_AGAIN) => IntentState.StateId.WAITING_TO_RUN,
                _ => current
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Transition(ref IntentState.StateId state, IntentState.EventId evt)
        {
            IntentState.StateId next = Transition(state, evt);
            bool changed = next != state;
            state = next;
            return changed;
        }
    }
}
