using Unity.Entities;

namespace Game.Simulation.Core.Components
{
    public struct Stagger : IComponentData
    {
        public float Current;
        public float Max;
        
        /// <summary>
        /// Time in seconds since the last hit.
        /// </summary>
        public float TimeSinceLastHit;
        
        /// <summary>
        /// If true, stagger is actively decaying.
        /// </summary>
        public bool IsDecaying;
    }
}
