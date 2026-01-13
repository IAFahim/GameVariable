using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameVariable.Intent
{
    /// <summary>
    /// Ultra-optimized table-based state machine logic.
    /// Uses pre-multiplied offsets and eliminates static constructor overhead.
    /// Note: [BurstCompile] attribute can be added when using Unity Burst compiler.
    /// </summary>
    public static class IntentLogic
    {
        // State IDs for reference
        public const byte ROOT = 0;
        public const byte CANCELED = 1;
        public const byte CREATED = 2;
        public const byte FAULTED = 3;
        public const byte RAN_TO_COMPLETION = 4;
        public const byte RUNNING = 5;
        public const byte WAITING_FOR_ACTIVATION = 6;
        public const byte WAITING_FOR_CHILDREN_TO_COMPLETE = 7;
        public const byte WAITING_TO_RUN = 8;

        // Pre-multiplied offsets (StateID * 16)
        private const byte OFFSET_ROOT = 0;
        private const byte OFFSET_CANCELED = 16;
        private const byte OFFSET_CREATED = 32;
        private const byte OFFSET_FAULTED = 48;
        private const byte OFFSET_RAN_TO_COMPLETION = 64;
        private const byte OFFSET_RUNNING = 80;
        private const byte OFFSET_WAITING_FOR_ACTIVATION = 96;
        private const byte OFFSET_WAITING_FOR_CHILDREN = 112;
        private const byte OFFSET_WAITING_TO_RUN = 128;

        // Event IDs
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

        // Initialized inline to allow 'beforefieldinit' optimization (No JIT Checks).
        // The table stores OFFSETS, not state IDs.
        // Table layout:[stateOffset + eventId] = nextOffset
        // Each state has 16 entries (events 0-15, though only 0-10 are used)
        private static readonly byte[] _table = new byte[]
        {
            // State 0: ROOT (Offset 0) - All events stay in ROOT
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

            // State 1: CANCELED (Offset 16) - Terminal state, all events stay in CANCELED
            16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16,

            // State 2: CREATED (Offset 32)
            // Events: ACTIVATED(0)->WAITING_TO_RUN(128), GET_READY(7)->WAITING_FOR_ACTIVATION(96)
            128, 32, 32, 32, 32, 32, 32, 96, 32, 32, 32, 32, 32, 32, 32, 32,

            // State 3: FAULTED (Offset 48) - Terminal state
            48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48,

            // State 4: RAN_TO_COMPLETION (Offset 64)
            // Events: RUN_AGAIN(8)->WAITING_TO_RUN(128)
            64, 64, 64, 64, 64, 64, 64, 64, 128, 64, 64, 64, 64, 64, 64, 64,

            // State 5: RUNNING (Offset 80)
            // Events: CHILD_TASK_CREATED(5)->WAITING_FOR_CHILDREN(112), UNABLE_TO_COMPLETE(10)->FAULTED(48)
            //         CANCEL(2)->CANCELED(16), COMPLETED_SUCCESSFULLY(6)->RAN_TO_COMPLETION(64)
            80, 80, 16, 80, 80, 112, 64, 80, 80, 80, 48, 80, 80, 80, 80, 80,

            // State 6: WAITING_FOR_ACTIVATION (Offset 96)
            // Events: ACTIVATED(0)->WAITING_TO_RUN(128), CANCELED_BEFORE_ACTIVATION(3)->CANCELED(16)
            128, 96, 96, 16, 96, 96, 96, 96, 96, 96, 96, 96, 96, 96, 96, 96,

            // State 7: WAITING_FOR_CHILDREN_TO_COMPLETE (Offset 112)
            // Events: ALL_CHILDREN_COMPLETED(1)->RUNNING(80)
            112, 80, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112,

            // State 8: WAITING_TO_RUN (Offset 128)
            // Events: START_RUNNING(9)->RUNNING(80), CANCELED_BEFORE_RUN(4)->CANCELED(16)
            128, 128, 128, 128, 16, 128, 128, 128, 128, 80, 128, 128, 128, 128, 128, 128,
        };

        /// <summary>
        /// Resolves the next state offset.
        /// Uses direct array indexing with pre-multiplied offsets.
        /// Compatible with netstandard2.1 while maintaining high performance.
        /// </summary>
        /// <param name="currentOffset">The current pre-multiplied state offset.</param>
        /// <param name="eventId">The event ID (0-15).</param>
        /// <param name="nextOffset">The result offset (pre-multiplied).</param>
        /// <param name="changed">Did the state change?</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Apply(in byte currentOffset, in byte eventId, out byte nextOffset, out bool changed)
        {
            // OPTIMIZATION 1: Direct array indexing with bounds check elision.
            // The JIT compiler will often elide bounds checks when it can prove safety.
            // We use 'currentOffset' directly. No (state << 4) shift required.
            // (eventId & 0x0F) ensures safety mask.
            // Logic: table[StateOffset + (EventID masked)]
            int index = currentOffset + (eventId & 0x0F);
            byte next = _table[index];

            nextOffset = next;

            // OPTIMIZATION 2: Integer comparison is preferred over bool branching.
            changed = currentOffset != next;
        }

        /// <summary>
        /// Gets the state ID from a pre-multiplied offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte OffsetToStateId(byte offset)
        {
            return (byte)(offset / 16);
        }

        /// <summary>
        /// Converts a state ID to a pre-multiplied offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte StateIdToOffset(byte stateId)
        {
            return (byte)(stateId * 16);
        }
    }
}
