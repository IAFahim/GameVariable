using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Reservoir;
using Variable.RPG;
using Variable.Timer;
using Xunit;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void Character_Initialization_SyncsStats()
    {
        // Allocate stats for basic attributes + regen
        var state = new CharacterState(4);
        try
        {
            // Set base stats
            state.Stats.SetBase(CharacterLogic.STAT_VITALITY, 10f); // +100 HP -> 200 Max
            state.Stats.SetBase(CharacterLogic.STAT_INTELLIGENCE, 10f); // +50 Mana -> 100 Max

            // Sync
            CharacterLogic.SynchronizeStats(ref state.Stats, ref state.Health, ref state.Mana);

            Assert.Equal(200f, state.Health.Max);
            Assert.Equal(100f, state.Mana.Volume.Max);
        }
        finally
        {
            state.Dispose();
        }
    }

    [Fact]
    public void Character_CastSpell_ConsumesMana_And_StartsCooldown()
    {
        var state = new CharacterState(4);
        try
        {
            // Setup Mana
            state.Stats.SetBase(CharacterLogic.STAT_INTELLIGENCE, 10f); // 100 Max
            CharacterLogic.SynchronizeStats(ref state.Stats, ref state.Health, ref state.Mana);
            state.Mana.Volume.Current = 100f;

            // Cast Spell (Cost 20, CD 5s)
            bool success = state.CastSpell(20f, 5f);

            Assert.True(success);
            Assert.Equal(80f, state.Mana.Volume.Current);
            Assert.Equal(5f, state.Cooldown.Current); // Cooldown started
            Assert.Equal(5f, state.Cooldown.Duration);

            // Try casting again immediately -> Fail due to cooldown
            success = state.CastSpell(20f, 5f);
            Assert.False(success);
            Assert.Equal(80f, state.Mana.Volume.Current); // No mana consumed
        }
        finally
        {
            state.Dispose();
        }
    }

    [Fact]
    public void Character_Update_TicksCooldown_And_Regens()
    {
        var state = new CharacterState(4);
        try
        {
            // Setup Regen
            state.Stats.SetBase(CharacterLogic.STAT_REGEN_HEALTH, 10f); // 10 HP/sec
            state.Stats.SetBase(CharacterLogic.STAT_REGEN_MANA, 5f); // 5 Mana/sec
            CharacterLogic.SynchronizeStats(ref state.Stats, ref state.Health, ref state.Mana);

            // Set initial values
            state.Health.Current = 50f;
            state.Mana.Volume.Current = 10f;

            // Set Cooldown
            state.Cooldown.Duration = 5f;
            state.Cooldown.Current = 5f;

            // Update 1 sec
            state.Update(1f);

            // Check Regen
            Assert.Equal(60f, state.Health.Current);
            Assert.Equal(15f, state.Mana.Volume.Current);

            // Check Cooldown Tick
            Assert.Equal(4f, state.Cooldown.Current);

            // Update 4 more seconds -> Cooldown ready
            state.Update(4f);
            Assert.Equal(0f, state.Cooldown.Current);

            // Now can cast
            state.Mana.Volume.Current = 100f; // Ensure mana
            bool success = state.CastSpell(10f, 1f);
            Assert.True(success);
        }
        finally
        {
            state.Dispose();
        }
    }

    [Fact]
    public void Character_GainXp_LevelUp_IncreasesStats()
    {
        var state = new CharacterState(4);
        try
        {
            // Level 1, 0 XP
            // Next level at 1000 XP (implicitly set in Logic if 0)

            state.Stats.SetBase(CharacterLogic.STAT_VITALITY, 10f);
            CharacterLogic.SynchronizeStats(ref state.Stats, ref state.Health, ref state.Mana);
            float initialMaxHp = state.Health.Max;

            // Gain 500 XP -> No Level Up
            state.GainXp(500);
            Assert.Equal(1, state.Experience.Level);
            Assert.Equal(500, state.Experience.Current);
            Assert.Equal(initialMaxHp, state.Health.Max);

            // Gain 600 XP -> Level Up (Total 1100, Level 2, Current 100)
            state.GainXp(600);
            Assert.Equal(2, state.Experience.Level);
            Assert.Equal(100, state.Experience.Current);

            // Check Stat Increase (Vit +1 -> +10 HP)
            Assert.Equal(initialMaxHp + 10f, state.Health.Max);

            // Check Full Heal on Level Up
            Assert.Equal(state.Health.Max, state.Health.Current);
        }
        finally
        {
            state.Dispose();
        }
    }
}
