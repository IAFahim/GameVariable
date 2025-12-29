using Unity.Entities;

namespace Game.Simulation.Core.Components
{
    public struct DamageEvent : IBufferElementData
    {
        public float Amount;
        public Entity Source;
    }
}
