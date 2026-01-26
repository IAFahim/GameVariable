using System;
using System.Runtime.InteropServices;
using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.RPG;

namespace GameVariable.Synergy
{
    /// <summary>
    /// Represents the composite state of a game entity, including stats, experience, health, and mana.
    /// Acts as the integration layer ("Sergey") between different Variable systems.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SynergyState : IDisposable
    {
        // Stat IDs
        public const int STAT_VITALITY = 0;
        public const int STAT_SPIRIT = 1;
        public const int STAT_STRENGTH = 2;
        public const int STAT_AGILITY = 3;
        public const int STAT_COUNT = 4;

        /// <summary>The RPG stats sheet (Unmanaged).</summary>
        public RpgStatSheet Stats;

        /// <summary>The experience tracker.</summary>
        public ExperienceInt Experience;

        /// <summary>The regenerating health value.</summary>
        public RegenFloat Health;

        /// <summary>The regenerating mana value.</summary>
        public RegenFloat Mana;

        /// <summary>
        /// Initializes a new instance of the SynergyState struct.
        /// </summary>
        /// <param name="maxHealth">Initial max health.</param>
        /// <param name="maxMana">Initial max mana.</param>
        /// <param name="maxXp">Initial max XP for level 1.</param>
        public SynergyState(int maxHealth, int maxMana, int maxXp)
        {
            Stats = new RpgStatSheet(STAT_COUNT);
            Experience = new ExperienceInt(maxXp);
            Health = new RegenFloat(maxHealth, maxHealth, 0f); // Default rate 0, updated by logic
            Mana = new RegenFloat(maxMana, maxMana, 0f);     // Default rate 0, updated by logic
        }

        /// <summary>
        /// Disposes the unmanaged resources (Stats).
        /// </summary>
        public void Dispose()
        {
            Stats.Dispose();
        }
    }
}
