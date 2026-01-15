using System;
using System.Runtime.InteropServices;
using Variable.Experience;
using Variable.Reservoir;

namespace VoidClimber.Core
{
    /// <summary>
    /// Player-specific data extending Entity with progression systems.
    /// Demonstrates Variable.Experience and Variable.Reservoir usage.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerData : IDisposable
    {
        // =====================
        // BASE ENTITY
        // =====================

        /// <summary>
        /// Core entity data (health, stats, cooldowns).
        /// </summary>
        public Entity BaseEntity;

        // =====================
        // COMPOSITION - Variable.Experience
        // =====================

        /// <summary>
        /// Experience system using Variable.Experience.ExperienceInt.
        /// Tracks current XP and handles level-ups.
        /// </summary>
        public ExperienceInt Experience;

        // =====================
        // COMPOSITION - Variable.Reservoir
        // =====================

        /// <summary>
        /// Potion system using Variable.Reservoir.ReservoirInt.
        /// Volume = potions on belt (quick access)
        /// Reserve = potions in bag (storage)
        /// </summary>
        public ReservoirInt Potions;

        /// <summary>
        /// Gold currency (simple int for MVP).
        /// Could use ReservoirInt for inventory limits.
        /// </summary>
        public int Gold;

        /// <summary>
        /// Keys for treasure rooms.
        /// </summary>
        public int Keys;

        // =====================
        // PROGRESSION TRACKING
        // =====================

        /// <summary>Total enemies killed</summary>
        public int Kills;

        /// <summary>Total floors cleared</summary>
        public int FloorsCleared;

        /// <summary>Total damage dealt</summary>
        public float TotalDamageDealt;

        /// <summary>Total damage taken</summary>
        public float TotalDamageTaken;

        /// <summary>Highest floor reached</summary>
        public int HighestFloor;

        // =====================
        // CONSTRUCTORS
        // =====================

        /// <summary>
        /// Create a new player with default stats.
        /// </summary>
        /// <param name="name">Player name</param>
        /// <param name="maxHealth">Starting max health</param>
        /// <param name="xpToLevel">XP required for first level</param>
        public PlayerData(string name, float maxHealth, int xpToLevel = 100)
        {
            // Initialize base entity
            BaseEntity = new Entity(name, maxHealth, 1.0f, 10);

            // Initialize experience system
            Experience = new ExperienceInt(xpToLevel);

            // Initialize potions (3 on belt, 0 in reserve, max 3 belt, 10 reserve)
            Potions = new ReservoirInt(3, 3, 10);

            // Initialize currency
            Gold = 0;
            Keys = 0;

            // Initialize tracking
            Kills = 0;
            FloorsCleared = 0;
            TotalDamageDealt = 0;
            TotalDamageTaken = 0;
            HighestFloor = 1;
        }

        // =====================
        // POTION MANAGEMENT
        // =====================

        /// <summary>
        /// Use a health potion.
        /// Returns true if potion was consumed.
        /// </summary>
        public bool UsePotion()
        {
            // Check if potions available
            if (Potions.Volume > 0)
            {
                // Consume from belt
                Potions.Volume--;

                // Heal for 25 HP
                float healAmount = 25f;
                float currentHp = BaseEntity.Health.Value.Current;
                float maxHp = BaseEntity.Health.Value.Max;

                float added = Variable.Inventory.InventoryLogic.AddPartial(
                    ref currentHp, healAmount, maxHp, out _);

                BaseEntity.Health.Value.Current = currentHp;

                // Auto-refill belt from reserve if empty
                if (Potions.Volume == 0)
                {
                    Potions.Refill();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add potions to reserve.
        /// </summary>
        public void AddPotions(int count)
        {
            // Add to reserve (bag)
            int current = Potions.Reserve;
            int maxReserve = 10;

            Variable.Inventory.InventoryLogic.AddPartial(
                ref current, count, maxReserve, out int _);

            Potions.Reserve = current;
        }

        // =====================
        // CURRENCY MANAGEMENT
        // =====================

        /// <summary>
        /// Add gold.
        /// </summary>
        public void AddGold(int amount)
        {
            Gold += amount;
        }

        /// <summary>
        /// Spend gold if available.
        /// </summary>
        public bool TrySpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add key to inventory.
        /// </summary>
        public void AddKey()
        {
            Keys++;
        }

        /// <summary>
        /// Use a key if available.
        /// </summary>
        public bool TryUseKey()
        {
            if (Keys > 0)
            {
                Keys--;
                return true;
            }
            return false;
        }

        // =====================
        // UTILITY METHODS
        // =====================

        /// <summary>
        /// Get player level.
        /// </summary>
        public readonly int GetLevel()
        {
            return BaseEntity.Level;
        }

        /// <summary>
        /// Get XP progress to next level (0-1).
        /// </summary>
        public readonly float GetXpProgress()
        {
            return Variable.Bounded.BoundedExtensions.GetRatio(Experience);
        }

        /// <summary>
        /// Get XP bar display string.
        /// </summary>
        public readonly string GetXpBar(int width = 20)
        {
            float ratio = GetXpProgress();
            int filled = (int)(ratio * width);
            int empty = width - filled;

            return new string('█', filled) + new string('░', empty);
        }

        /// <summary>
        /// Check if player has potions available.
        /// </summary>
        public readonly bool HasPotions()
        {
            return Potions.Volume > 0 || Potions.Reserve > 0;
        }

        /// <summary>
        /// Get total potions (belt + reserve).
        /// </summary>
        public readonly int GetTotalPotions()
        {
            return Potions.Volume + Potions.Reserve;
        }

        // =====================
        // CLEANUP
        // =====================

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            BaseEntity.Dispose();
        }
    }
}
