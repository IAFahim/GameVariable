using Xunit;
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;
using Variable.RPG;
using GameVariable.Intent;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void SynergyState_Integration_Test()
    {
        // 1. Setup (Layer A)
        // using var state = ... is not allowed because we need to pass 'state' by ref.
        var state = new SynergyState();

        try
        {
            // Initialize components
            state.Health = new BoundedFloat(100f);
            state.Mana = new RegenFloat(100f, 50f, 5f); // 50/100, +5/sec
            state.SkillCooldown = new Cooldown(3f);     // 3s duration
            state.Experience = new ExperienceInt(1000); // Level 1, 1000 XP to next

            // Initialize Stats (Unmanaged)
            state.Stats = new RpgStatSheet(10);
            state.Stats.SetBase(SynergyExtensions.STAT_STR, 10f); // 10 Strength

            // Initialize Inventory Logic Data
            state.MaxGold = 1000f; // Base capacity
            state.Gold = 0f;

            // Initialize AI
            state.StartAi(); // Enters CREATED state

            // Move AI to WAITING_TO_RUN so Cast() can trigger RUNNING
            state.AiState.DispatchEvent(IntentState.EventId.ACTIVATED);
            // Now state should be WAITING_TO_RUN

            // 2. Tick (Layer C)
            state.Tick(1f); // 1 second passes

            // Mana should regen: 50 + 5 = 55
            Assert.Equal(55f, state.Mana.Value.Current);

            // Cooldown should tick (was ready 0/3, stays ready 0/3)
            Assert.True(state.SkillCooldown.IsReady());

            // 3. Cast Skill (Cost 20)
            // Requires Mana >= 20 and Cooldown Ready
            bool casted = state.Cast(20f);

            Assert.True(casted, "Skill should be castable");
            Assert.Equal(35f, state.Mana.Value.Current); // 55 - 20 = 35
            Assert.False(state.SkillCooldown.IsReady(), "Skill should be on cooldown");

            // Cooldown reset to Duration (3.0)
            Assert.Equal(3f, state.SkillCooldown.Current);

            // 4. Try Cast Again (Should fail due to Cooldown)
            bool castedAgain = state.Cast(20f);
            Assert.False(castedAgain, "Skill should fail due to cooldown");
            Assert.Equal(35f, state.Mana.Value.Current); // No change

            // 5. Loot Gold
            // Capacity Logic in Extensions: Capacity = MaxGold (1000) + (Str (10) * 10) = 1100.
            // Current Gold = 0.
            state.Loot(500f);
            Assert.Equal(500f, state.Gold);

            state.Loot(700f); // 500 + 700 = 1200. Should cap at 1100.
            Assert.Equal(1100f, state.Gold);

            // 6. Gain XP and Level Up
            // Current XP 0/1000.
            // Add 1500 XP -> Level up to 2.
            // Level up should trigger Full Restore via SynergyExtensions logic.

            // Damage Health first to test restore
            state.Health.Current = 10f;

            state.GainXp(1500);

            Assert.Equal(2, state.Experience.Level);
            Assert.Equal(100f, state.Health.Current);       // Restored
            Assert.Equal(100f, state.Mana.Value.Current);   // Restored

            // Verify XP overflow
            // Added 1500 to 0/1000.
            // Level 1 -> 2 costs 1000. Remaining 500.
            // Level 2 -> 3 costs 2000 (Linear Formula: Level * 1000 -> 2 * 1000).
            // Current XP should be 500/2000.
            Assert.Equal(500, state.Experience.Current);
            Assert.Equal(2000, state.Experience.Max);
        }
        finally
        {
            state.Dispose();
        }
    }
}
