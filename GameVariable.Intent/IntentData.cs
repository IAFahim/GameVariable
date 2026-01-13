using System;
using System.Runtime.InteropServices;

namespace GameVariable.Intent
{
    /// <summary>
    /// Optimized data structure for Intent state machine.
    /// Uses pre-multiplied state offsets to eliminate shift operations during runtime.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct IntentData
    {
        /// <summary>
        /// The raw byte offset in the lookup table (StateIndex * 16).
        /// Storing it this way removes the bitshift (<< 4) instruction during the Tick.
        /// Example: State 2 (CREATED) is stored as 32 (2 * 16)
        /// </summary>
        public byte StateOffset;

        /// <summary>
        /// Initializes the IntentData with a specific state ID.
        /// </summary>
        /// <param name="stateId">The initial state ID (0-8).</param>
        public IntentData(byte stateId)
        {
            // Pre-multiply: state 0 -> offset 0, state 1 -> offset 16, etc.
            StateOffset = (byte)(stateId * 16);
        }

        /// <summary>
        /// Gets the current state ID from the offset.
        /// </summary>
        public readonly byte StateId => (byte)(StateOffset / 16);
    }
}
