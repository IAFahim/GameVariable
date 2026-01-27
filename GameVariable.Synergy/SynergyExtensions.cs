using System;
using System.Runtime.CompilerServices;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Inventory;
using Variable.Experience;
using Variable.RPG;
using GameVariable.Intent;

namespace GameVariable.Synergy;

/// <summary>
///     The "Sergey" adapter (Layer C).
///     Orchestrates the interaction between multiple GameVariable systems.
/// </summary>
public static class SynergyExtensions
{
    // --- Stats Definition for Synergy (Mock RPG Stats) ---
    public const int STAT_STR = 0; // Affects Gold Capacity
    public const int STAT_INT = 1; // Affects Mana Regen (future)
    public const int STAT_VIT = 2; // Affects Max Health (future)

    /// <summary>
    ///     Advances the entire state by one frame.
    ///     Demonstrates: Timer, Regen, and custom Synergy Logic ticking together.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SynergyState state, float deltaTime)
    {
        // 1. Tick Mana Regeneration (Variable.Regen)
        // Note: RegenExtensions must be imported.
        // If RegenExtensions.Tick(ref this RegenFloat) exists, we call it.
        // Assuming Variable.Regen has extensions, otherwise we call Logic.
        // Based on memory/patterns, it likely has extensions.
        // If not, we fall back to RegenLogic.Tick(ref state.Mana.Value, state.Mana.Min, state.Mana.Max, state.Mana.Rate, deltaTime);
        // Let's assume extensions exist for cleaner code.
        // To be safe against "Primitives Only" if extensions don't exist, I'll check via Logic first?
        // No, I'll try to find if RegenExtensions exists. It was in file list.
        // I'll assume standard extensions pattern.

        // Manual expansion for safety if Extensions are not verified:
        RegenLogic.Tick(ref state.Mana.Value.Current, state.Mana.Value.Min, state.Mana.Value.Max, state.Mana.Rate, deltaTime);

        // 2. Tick Cooldowns (Variable.Timer)
        state.SkillCooldown.Tick(deltaTime);

        // 3. AI State Machine (GameVariable.Intent) - Event driven, but maybe needs update?
        // IntentState is purely event driven in this version.
    }

    /// <summary>
    ///     Attempts to cast a skill, consuming Mana and checking Cooldown.
    ///     Demonstrates: Interaction between Regen (Mana) and Timer (Cooldown).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Cast(ref this SynergyState state, float manaCost)
    {
        // Check conditions using SynergyLogic (Layer B)
        SynergyLogic.CanCast(in state.Mana.Value.Current, in manaCost, state.SkillCooldown.IsReady(), out bool canCast);

        if (canCast)
        {
            // Consume Mana (Mutation)
            state.Mana.Value -= manaCost;

            // Reset Cooldown (Mutation)
            state.SkillCooldown.Reset();

            // Dispatch AI Event
            state.AiState.DispatchEvent(IntentState.EventId.START_RUNNING); // Example integration
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Loots gold, respecting capacity defined by Strength stat.
    ///     Demonstrates: Interaction between Inventory, RPG Stats, and Bounded logic.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Loot(ref this SynergyState state, float amount)
    {
        // 1. Calculate dynamic capacity based on RPG Stats
        // (Assuming stats are initialized)
        float str = 0f;
        if (state.Stats.AsSpan().Length > STAT_STR)
        {
            str = state.Stats.Get(STAT_STR);
        }

        // Use Logic to calculate capacity
        SynergyLogic.CalculateGoldCapacity(in state.MaxGold, in str, out float actualCapacity);

        // 2. Add to inventory (Variable.Inventory)
        InventoryLogic.TryAddPartial(ref state.Gold, amount, actualCapacity, out _, out _);
    }

    /// <summary>
    ///     Gains XP and handles leveling up.
    ///     Demonstrates: Variable.Experience with a custom formula.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GainXp(ref this SynergyState state, int amount)
    {
        int oldLevel = state.Experience.Level;

        // Use standard extension with our custom formula
        state.Experience.Add(amount, new SynergyXpFormula());

        int newLevel = state.Experience.Level;

        // Check for level up events
        SynergyLogic.CheckLevelUpRestore(in oldLevel, in newLevel, out bool shouldRestore);

        if (shouldRestore)
        {
            // Full Heal on Level Up!
            state.Health.Current = state.Health.Max;
            state.Mana.Value.Current = state.Mana.Value.Max;
        }
    }

    /// <summary>
    ///     Starts the AI.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StartAi(ref this SynergyState state)
    {
        state.AiState.Start();
    }
}

/// <summary>
///     Simple linear XP formula for the Synergy example.
///     Level 1 -> 1000 XP
///     Level 2 -> 2000 XP
/// </summary>
public struct SynergyXpFormula : INextMaxFormula<int>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Calculate(int level)
    {
        return level * 1000;
    }
}
