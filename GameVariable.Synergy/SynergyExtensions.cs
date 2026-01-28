using System;
using System.Runtime.CompilerServices;
using Variable.Regen;
using Variable.RPG;

namespace GameVariable.Synergy;

/// <summary>
///     Extensions for the SynergyState struct.
/// </summary>
public static unsafe class SynergyExtensions
{
    /// <summary>
    ///     Updates the time-dependent state (Health/Mana regen).
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="dt">Delta time in seconds.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SynergyState state, float dt)
    {
        state.Health.Tick(dt);
        state.Mana.Tick(dt);
    }

    /// <summary>
    ///     Synchronizes derived stats (Max Health/Mana, Regen Rates) from the RpgStatSheet.
    ///     Call this after leveling up or equipping items.
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="maxHpId">Stat ID for Max Health.</param>
    /// <param name="maxManaId">Stat ID for Max Mana.</param>
    /// <param name="regenHpId">Stat ID for Health Regen.</param>
    /// <param name="regenManaId">Stat ID for Mana Regen.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SyncStats(ref this SynergyState state, int maxHpId, int maxManaId, int regenHpId, int regenManaId)
    {
        // Update Max Health
        float newMaxHp = state.Stats.Get(maxHpId);
        state.Health.Value.Max = newMaxHp;

        // Clamp current if it exceeds new max
        if (state.Health.Value.Current > newMaxHp)
            state.Health.Value.Current = newMaxHp;

        // Update Health Regen Rate
        state.Health.Rate = state.Stats.Get(regenHpId);

        // Update Max Mana
        float newMaxMana = state.Stats.Get(maxManaId);
        state.Mana.Value.Max = newMaxMana;

        if (state.Mana.Value.Current > newMaxMana)
            state.Mana.Value.Current = newMaxMana;

        // Update Mana Regen Rate
        state.Mana.Rate = state.Stats.Get(regenManaId);
    }
}
