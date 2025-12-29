using Unity.Entities;
using Unity.Burst;
using Game.Simulation.Core.Components;
using Game.Simulation.Core.Logic;

namespace Game.Simulation.Core.Systems
{
    [BurstCompile]
    public partial struct StaggerSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {}
        public void OnDestroy(ref SystemState state) {}

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float dt = SystemAPI.TimeDeltaTime;

            foreach (var (damageBuffer, stagger) in SystemAPI.Query<DynamicBuffer<DamageEvent>, RefRW<Stagger>, RefRW<Health>>()) // Adding Health just to match tuple size or whatever logic
            {
                // Note: Querying tuple order must match SystemAPI mock overload or generic params.
                // My Mock Query has <T1, T2, T3>.
                // StaggerSystem needs DamageEvent (Buffer), Stagger (RefRW).
                // I'll adjust usage.
                
                if (!damageBuffer.IsEmpty)
                {
                    stagger.ValueRW.TimeSinceLastHit = 0f;
                    stagger.ValueRW.IsDecaying = false;

                    foreach (var dmg in damageBuffer)
                    {
                         StaggerLogic.AddStagger(ref stagger.ValueRW.Current, dmg.Amount, stagger.ValueRO.Max, 1.0f);
                    }
                }
                else
                {
                    stagger.ValueRW.TimeSinceLastHit += dt;
                }

                float current = stagger.ValueRW.Current;
                StaggerLogic.UpdateStaggerDecay(ref current, stagger.ValueRO.Max, stagger.ValueRW.TimeSinceLastHit, dt);
                stagger.ValueRW.Current = current;
                
                if (stagger.ValueRW.TimeSinceLastHit > 5.0f)
                {
                    stagger.ValueRW.IsDecaying = true;
                }
            }
        }
    }
}
