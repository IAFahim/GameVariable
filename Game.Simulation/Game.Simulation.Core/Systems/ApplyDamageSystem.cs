using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Game.Simulation.Core.Components;
using Game.Simulation.Core.Logic;

namespace Game.Simulation.Core.Systems
{
    [BurstCompile]
    public partial struct ApplyDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DamageEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Iterate over entities with DamageEvent buffer, Health, ProvisionalHealth
            foreach (var (damageBuffer, health, provisional, armored, entity) in 
                     SystemAPI.QueryWithEntity<DynamicBuffer<DamageEvent>, RefRW<Health>, RefRW<ProvisionalHealth>>())
            {
                if (damageBuffer.IsEmpty) continue;

                // Check Armored component presence/enabled state
                bool isArmored = SystemAPI.IsComponentEnabled<Armored>(entity);

                foreach (var dmg in damageBuffer)
                {
                    float incomingDamage = dmg.Amount;
                    
                    DamageLogic.CalculateDamage(
                        incomingDamage,
                        isArmored,
                        provisional.ValueRW.Value,
                        out float damageToHealth,
                        out float newProvisional
                    );

                    health.ValueRW.Current -= damageToHealth;
                    provisional.ValueRW.Value = newProvisional;
                }
                
                // Buffer clearing is handled by ClearDamageBufferSystem to allow other systems to read events.
            }
        }
    }
}
