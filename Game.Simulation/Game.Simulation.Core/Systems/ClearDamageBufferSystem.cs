using Unity.Entities;
using Unity.Burst;
using Game.Simulation.Core.Components;

namespace Game.Simulation.Core.Systems
{
    [BurstCompile]
    public partial struct ClearDamageBufferSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {}
        public void OnDestroy(ref SystemState state) {}

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var buffer in SystemAPI.Query<DamageEvent>())
            {
                buffer.Clear();
            }
        }
    }
}
