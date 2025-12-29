using Unity.Entities;

namespace Game.Simulation.Core.Components
{
    public struct Health : IComponentData
    {
        public float Current;
        public float Max;
    }
}
