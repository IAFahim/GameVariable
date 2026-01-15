using System;
using System.Runtime.InteropServices;
using Variable.Regen;
using Variable.RPG;
using Variable.Timer;

namespace VoidClimber.Core
{
    /// <summary>
    /// Core entity struct representing both players and enemies.
    /// Demonstrates composition of multiple GameVariable types.
    ///
    /// Architecture: Data-Logic-Extension triad
    /// - Data: This struct holds all data
    /// - Logic: Static classes (CombatLogic, etc.) manipulate this data
    /// - Extension: Extension methods provide utility functionality
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Entity : IDisposable
    {
        // =====================
        // LIFECYCLE
        // =====================

        /// <summary>Whether this entity is alive</summary>
        public bool IsAlive;

        /// <summary>Current level (1-based)</summary>
        public int Level;

        /// <summary>Unique identifier for lookups</summary>
        public int EntityId;

        /// <summary>Display name</summary>
        public string Name;

        // =====================
        // COMPOSITION - Variable.Regen
        // =====================

        /// <summary>
        /// Health system using Variable.Regen.RegenFloat.
        /// Handles current/max health + regeneration per second.
        /// </summary>
        public RegenFloat Health;

        // =====================
        // COMPOSITION - Variable.Timer
        // =====================

        /// <summary>
        /// Attack cooldown using Variable.Timer.Cooldown.
        /// Time until next attack can be performed.
        /// </summary>
        public Cooldown AttackCooldown;

        /// <summary>
        /// Special ability cooldown.
        /// </summary>
        public Cooldown SpecialCooldown;

        // =====================
        // COMPOSITION - Variable.RPG
        // =====================

        /// <summary>
        /// RPG stats using Variable.RPG.RpgStatSheet.
        /// Contains all stat values with modifier support.
        ///
        /// IMPORTANT: This allocates unmanaged memory and must be disposed.
        /// </summary>
        public RpgStatSheet Stats;

        // =====================
        // TEMPORARY EFFECTS
        // =====================

        /// <summary>Turns remaining on status effects</summary>
        public int PoisonStacks;
        public int BurnStacks;
        public int StunTurns;

        // =====================
        // CONSTRUCTORS
        // =====================

        /// <summary>
        /// Create a new entity with default stats.
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="maxHealth">Starting max health</param>
        /// <param name="healthRegen">Health per second</param>
        /// <param name="statCount">Number of stats to allocate</param>
        public Entity(string name, float maxHealth, float healthRegen, int statCount = 10)
        {
            Name = name;
            IsAlive = true;
            Level = 1;
            EntityId = 0;
            PoisonStacks = 0;
            BurnStacks = 0;
            StunTurns = 0;

            // Initialize health with Regen system
            Health = new RegenFloat(maxHealth, maxHealth, healthRegen);

            // Initialize cooldowns (1 second base attack, 5 second special)
            AttackCooldown = new Cooldown(1.0f);
            SpecialCooldown = new Cooldown(5.0f);

            // Initialize RPG stat sheet
            Stats = new RpgStatSheet(statCount);

            // Set default stat values
            Stats.SetBase((int)StatType.HealthMax, maxHealth);
            Stats.SetBase((int)StatType.HealthRegen, healthRegen);
            Stats.SetBase((int)StatType.AttackDamage, 10f);
            Stats.SetBase((int)StatType.Defense, 0f);
            Stats.SetBase((int)StatType.AttackSpeed, 1.0f);
            Stats.SetBase((int)StatType.CritChance, 0.05f);
            Stats.SetBase((int)StatType.CritMultiplier, 1.5f);
            Stats.SetBase((int)StatType.DodgeChance, 0f);
            Stats.SetBase((int)StatType.Lifesteal, 0f);
            Stats.SetBase((int)StatType.Thorns, 0f);
        }

        // =====================
        // UTILITY METHODS
        // =====================

        /// <summary>
        /// Check if this entity can attack (cooldown ready and not stunned).
        /// </summary>
        public readonly bool CanAttack()
        {
            return IsAlive && AttackCooldown.IsReady() && StunTurns == 0;
        }

        /// <summary>
        /// Check if special ability is ready.
        /// </summary>
        public readonly bool CanUseSpecial()
        {
            return IsAlive && SpecialCooldown.IsReady() && StunTurns == 0;
        }

        /// <summary>
        /// Apply damage to this entity.
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            // Use BoundedExtensions to consume from health
            Variable.Bounded.BoundedExtensions.TryConsume(ref Health.Value, damage);

            // Check death
            if (Variable.Bounded.BoundedExtensions.IsEmpty(Health.Value))
            {
                IsAlive = false;
            }
        }

        /// <summary>
        /// Heal this entity.
        /// </summary>
        public void Heal(float amount)
        {
            if (!IsAlive) return;

            // Use InventoryLogic to add health (respects max)
            float current = Health.Value.Current;
            float max = Health.Value.Max;
            float added = Variable.Inventory.InventoryLogic.AddPartial(ref current, amount, max, out _);

            Health.Value.Current = current;
        }

        /// <summary>
        /// Get current health as percentage (0-1).
        /// </summary>
        public readonly float GetHealthPercent()
        {
            return Variable.Bounded.BoundedExtensions.GetRatio(Health.Value);
        }

        /// <summary>
        /// Get display string for health bar.
        /// </summary>
        public readonly string GetHealthBar(int width = 20)
        {
            float ratio = GetHealthPercent();
            int filled = (int)(ratio * width);
            int empty = width - filled;

            return new string('█', filled) + new string('░', empty);
        }

        // =====================
        // CLEANUP
        // =====================

        /// <summary>
        /// Dispose of unmanaged resources (RpgStatSheet).
        /// MUST be called when entity is no longer needed.
        /// </summary>
        public void Dispose()
        {
            Stats.Dispose();
        }
    }
}
