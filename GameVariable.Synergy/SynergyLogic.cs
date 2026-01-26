using System.Runtime.CompilerServices;

namespace GameVariable.Synergy
{
    /// <summary>
    /// Core logic for the Synergy system.
    /// Pure, stateless, atomic calculations.
    /// </summary>
    public static class SynergyLogic
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckLevelUp(in int currentXp, in int maxXp, out bool isReady)
        {
            isReady = currentXp >= maxXp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateNextLevelXp(in int nextLevel, out int maxXp)
        {
            // Formula: Level * 1000 (e.g., Lvl 2 needs 2000 XP)
            maxXp = nextLevel * 1000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateBaseStats(in int level, out float vitality, out float spirit, out float strength, out float agility)
        {
             // Simple linear growth for demo
             vitality = 10f + (level - 1) * 2f;
             spirit = 5f + (level - 1) * 1f;
             strength = 10f + (level - 1) * 1.5f;
             agility = 5f + (level - 1) * 1f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateMaxHealth(in float vitality, out float maxHealth)
        {
            maxHealth = 100f + vitality * 10f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateMaxMana(in float spirit, out float maxMana)
        {
            maxMana = 50f + spirit * 10f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateHealthRegen(in float vitality, out float rate)
        {
            rate = 1f + vitality * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateManaRegen(in float spirit, out float rate)
        {
            rate = 2f + spirit * 1f;
        }
    }
}
