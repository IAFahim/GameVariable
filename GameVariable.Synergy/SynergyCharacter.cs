using System;
using System.Runtime.InteropServices;
using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.Timer;

namespace GameVariable.Synergy
{
    /// <summary>
    /// A composite structure representing a game character with Health, Mana, Cooldowns, and XP.
    /// Acts as Layer A (Pure Data) for the Synergy system.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SynergyCharacter
    {
        /// <summary>
        /// Health points of the character.
        /// </summary>
        public BoundedFloat Health;

        /// <summary>
        /// Mana points that regenerate over time.
        /// </summary>
        public RegenFloat Mana;

        /// <summary>
        /// Cooldown timer for the character's primary ability.
        /// </summary>
        public Cooldown AbilityCooldown;

        /// <summary>
        /// Experience points and leveling progress.
        /// </summary>
        public ExperienceInt XP;
    }
}
