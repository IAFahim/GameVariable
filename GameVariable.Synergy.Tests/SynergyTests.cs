using Xunit;
using GameVariable.Synergy;
using Variable.Bounded;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void SynergyState_AllocatesAndDisposes()
    {
        var state = new SynergyState(5, 100, 100f, 50f);
        Assert.Equal(5, state.Stats.Count);
        Assert.Equal(100f, state.Health.Value.Max);
        Assert.Equal(50f, state.Mana.Value.Max);

        state.Dispose();

        // RpgStatSheet sets Count to 0 on dispose
        Assert.Equal(0, state.Stats.Count);
    }

    [Fact]
    public void Tick_UpdatesHealthAndMana()
    {
        var state = new SynergyState(5, 100, 100f, 100f);
        state.Health.Rate = 10f;
        state.Mana.Rate = -5f;
        // BoundedFloat clamps to bounds, so setting current manually is fine as long as it's within bounds.
        // BoundedFloat constructor sets Current to Max by default? Let's check constructor usage in SynergyState.
        // Health = new RegenFloat(healthMax, healthMax, 0f); -> Current is healthMax (100).

        // Let's set it lower to test regen up
        // We have to set via Value field because RegenFloat doesn't expose Current setter directly on struct,
        // but exposes Value field.
        state.Health.Value.Current = 50f;
        state.Mana.Value.Current = 50f;

        state.Tick(1f);

        Assert.Equal(60f, state.Health.Value.Current);
        Assert.Equal(45f, state.Mana.Value.Current);

        state.Dispose();
    }

    [Fact]
    public void SyncStats_UpdatesBoundsAndRates()
    {
        var state = new SynergyState(10, 100, 100f, 100f);

        // Define IDs
        int maxHpId = 0;
        int maxManaId = 1;
        int regenHpId = 2;
        int regenManaId = 3;

        // Set Stats
        state.Stats.SetBase(maxHpId, 200f);
        state.Stats.SetBase(maxManaId, 150f);
        state.Stats.SetBase(regenHpId, 5f);
        state.Stats.SetBase(regenManaId, 10f);

        // Sync
        state.SyncStats(maxHpId, maxManaId, regenHpId, regenManaId);

        // Verify
        Assert.Equal(200f, state.Health.Value.Max);
        Assert.Equal(150f, state.Mana.Value.Max);
        Assert.Equal(5f, state.Health.Rate);
        Assert.Equal(10f, state.Mana.Rate);

        state.Dispose();
    }
}
