using GameVariable.Synergy;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    private const int MaxHp = 0;
    private const int MaxMp = 1;
    private const int HpRegen = 2;
    private const int MpRegen = 3;

    [Fact]
    public void Integration_Flow_Works()
    {
        // 1. Setup
        var state = new SynergyState(10);
        try
        {
            state.Stats.SetBase(MaxHp, 100f);
            state.Stats.SetBase(MaxMp, 50f);
            state.Stats.SetBase(HpRegen, 1f);

            state.SyncStats(MaxHp, MaxMp, HpRegen, MpRegen);

            Assert.Equal(1, state.Experience.Level);
            Assert.Equal(100f, state.Health.Value.Max);
            Assert.Equal(50f, state.Mana.Value.Max);

            // 2. Add XP - Not enough to level up (Max is 1000)
            state.AddXp(500, MaxHp, MaxMp, HpRegen, MpRegen);
            Assert.Equal(1, state.Experience.Level);
            Assert.Equal(500, state.Experience.Current);

            // 3. Level Up
            // Add 600 -> Total 1100. Level up (req 1000). Overflow 100.
            // New Level 2. Next Max = 2000 (DefaultCurve: level * 1000).
            // Bonus: +10 HP (110), +5 MP (55).
            state.AddXp(600, MaxHp, MaxMp, HpRegen, MpRegen);

            Assert.Equal(2, state.Experience.Level);
            Assert.Equal(100, state.Experience.Current);
            Assert.Equal(2000, state.Experience.Max);

            Assert.Equal(110f, state.Stats.Get(MaxHp));
            Assert.Equal(110f, state.Health.Value.Max); // Synced

            // 4. Damage and Regen
            // Current Health logic:
            // Before Level Up: 100/100.
            // Level Up increases Max to 110. Current kept at 100.
            // Apply Damage 50 -> 100 - 50 = 50.
            state.ApplyDamage(50f);
            Assert.Equal(50f, state.Health.Value.Current);

            // Tick 10 seconds. Regen 1/sec.
            // 50 + 10 = 60.
            state.Tick(10f);
            Assert.Equal(60f, state.Health.Value.Current);
        }
        finally
        {
            state.Dispose();
        }
    }

    [Fact]
    public void Multi_Level_Up_Works()
    {
        var state = new SynergyState(10);
        try
        {
            state.Stats.SetBase(MaxHp, 100f);
            state.SyncStats(MaxHp, MaxMp, HpRegen, MpRegen);

            // Lvl 1 (0/1000).
            // Add 5000 XP.
            state.AddXp(5000, MaxHp, MaxMp, HpRegen, MpRegen);

            // Logic trace:
            // 1. 5000 >= 1000 -> Lvl 2 (Max 2000). Rem: 4000.
            // 2. 4000 >= 2000 -> Lvl 3 (Max 3000). Rem: 2000.
            // 3. 2000 < 3000 -> Stop.

            Assert.Equal(3, state.Experience.Level);
            Assert.Equal(2000, state.Experience.Current);
            Assert.Equal(3000, state.Experience.Max);

            // Stats increased twice (+10 * 2 = +20)
            Assert.Equal(120f, state.Stats.Get(MaxHp));
            Assert.Equal(120f, state.Health.Value.Max);
        }
        finally
        {
            state.Dispose();
        }
    }
}
