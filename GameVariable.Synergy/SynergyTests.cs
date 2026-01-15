using Xunit;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using GameVariable.Intent;
using Variable.RPG;
using Variable.Input;
using Variable.Core;

namespace GameVariable.Synergy;

public class SynergyTests
{
    [Fact]
    public void Verify_Projects_Working_Perfectly_Together()
    {
        // 1. Setup State (Bounded, Regen, Timer)
        var health = new BoundedFloat(100f, 0f, 100f);
        // RegenFloat(max, current, rate) - Min is implied 0 for RegenFloat usually, or it uses BoundedFloat internals
        // The constructor is public RegenFloat(float max, float current, float rate)
        var mana = new RegenFloat(100f, 50f, 10f); // Max 100, Current 50, Rate 10/sec
        var attackCooldown = new Cooldown(1.5f);

        // 2. Setup Intent (The Brain)
        var intent = new IntentState();
        intent.Start(); // Enters ROOT -> CREATED

        Assert.Equal(IntentState.StateId.CREATED, intent.stateId);

        // 3. Simulate Game Loop (Input -> Intent -> Action -> State Change)
        float dt = 0.1f;

        // --- Tick 1: Idle ---
        mana.Tick(dt);
        attackCooldown.Tick(dt);

        // Input: "Activate" (Get Ready for combat)
        intent.DispatchEvent(IntentState.EventId.GET_READY);
        Assert.Equal(IntentState.StateId.WAITING_FOR_ACTIVATION, intent.stateId);

        // Input: "Activate" (Combat Start)
        intent.DispatchEvent(IntentState.EventId.ACTIVATED);
        Assert.Equal(IntentState.StateId.WAITING_TO_RUN, intent.stateId);

        // Input: "Start Running" (Attack Command)
        // Check preconditions first (Sergey/Synergy check)
        // Access mana.Value.Current because RegenFloat wraps BoundedFloat
        bool canAttack = attackCooldown.IsFull() && mana.Value.Current >= 20f;

        if (canAttack)
        {
            intent.DispatchEvent(IntentState.EventId.START_RUNNING);
        }

        Assert.Equal(IntentState.StateId.RUNNING, intent.stateId);

        // --- Tick 2: Attack Execution ---
        if (intent.stateId == IntentState.StateId.RUNNING)
        {
            // Consumption: RegenFloat does not have -= operator, modify inner Value
            // But we can't modify 'Value' directly if it's a property return?
            // RegenFloat field 'Value' is public BoundedFloat.
            mana.Value -= 20f;
            attackCooldown.Reset();

            // RPG Calculation (Damage)
            float baseDamage = 10f;
            float strengthBonus = 5f;
            float critMult = 2.0f; // Critical hit!

            // Note: RPG StatLogic might be needed here, or just direct math
            // Let's assume we use a simple calculation for now as a "synergy" proof
            float finalDamage = (baseDamage + strengthBonus) * critMult;

            // Apply to Target (Simulated)
            var enemyHealth = new BoundedFloat(50f, 50f);
            enemyHealth -= finalDamage;

            // Verify Enemy Taken Damage
            Assert.Equal(20f, enemyHealth.Current);

            // Signal Completion to Intent
            intent.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
        }

        Assert.Equal(IntentState.StateId.RAN_TO_COMPLETION, intent.stateId);

        // --- Tick 3: Cleanup ---
        mana.Tick(dt); // Regenerate some mana
        attackCooldown.Tick(dt); // Cooldown ticking

        // Mana was 50, +1 (tick 1) = 51.
        // -20 (attack) = 31.
        // +1 (tick 3) = 32.
        Assert.True(mana.Value.Current > 30f);
        Assert.False(attackCooldown.IsFull());

        // Intent -> Run Again?
        intent.DispatchEvent(IntentState.EventId.RUN_AGAIN);
        Assert.Equal(IntentState.StateId.WAITING_TO_RUN, intent.stateId);
    }
}
