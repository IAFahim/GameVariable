using System;
using VoidClimber.Core;

namespace VoidClimber.Logic
{
    /// <summary>
    /// Procedural enemy generation system.
    /// Creates varied enemies with stat distributions based on type and floor.
    /// </summary>
    public static class EnemyGeneration
    {
        // =====================
        // ENEMY TEMPLATES
        // =====================

        /// <summary>
        /// Base stats for each enemy type.
        /// These are scaled based on floor and player level.
        /// </summary>
        private readonly struct EnemyTemplate
        {
            public readonly float Health;
            public readonly float Damage;
            public readonly float Defense;
            public readonly float AttackSpeed;
            public readonly float CritChance;
            public readonly float HealthRegen;

            public EnemyTemplate(float health, float damage, float defense, float attackSpeed, float critChance = 0f, float healthRegen = 0f)
            {
                Health = health;
                Damage = damage;
                Defense = defense;
                AttackSpeed = attackSpeed;
                CritChance = critChance;
                HealthRegen = healthRegen;
            }
        }

        private static readonly EnemyTemplate[] Templates = new EnemyTemplate[]
        {
            // Goblin: Fast, low damage, low health
            new EnemyTemplate(health: 40f, damage: 8f, defense: 2f, attackSpeed: 1.3f),

            // Slime: Very low stats, appears in groups
            new EnemyTemplate(health: 20f, damage: 5f, defense: 0f, attackSpeed: 0.8f, critChance: 0f, healthRegen: 1f),

            // Skeleton: Balanced stats
            new EnemyTemplate(health: 60f, damage: 12f, defense: 5f, attackSpeed: 1.0f),

            // Orc: Slow, high damage, medium health
            new EnemyTemplate(health: 80f, damage: 18f, defense: 8f, attackSpeed: 0.7f, critChance: 0.1f),

            // Lich: Magic user (high crit, low defense)
            new EnemyTemplate(health: 50f, damage: 15f, defense: 0f, attackSpeed: 1.1f, critChance: 0.2f),

            // Golem: High defense, very slow
            new EnemyTemplate(health: 120f, damage: 10f, defense: 15f, attackSpeed: 0.5f)
        };

        private static readonly EnemyTemplate BossTemplate = new EnemyTemplate(
            health: 200f, damage: 25f, defense: 10f, attackSpeed: 1.0f, critChance: 0.15f);

        private static readonly EnemyTemplate EliteTemplate = new EnemyTemplate(
            health: 100f, damage: 18f, defense: 8f, attackSpeed: 1.1f, critChance: 0.1f);

        // =====================
        // GENERATION METHODS
        // =====================

        /// <summary>
        /// Generate a random enemy based on floor and room type.
        /// </summary>
        public static Entity GenerateEnemy(in GameState state, RoomType roomType)
        {
            EnemyType enemyType = SelectEnemyType(state, roomType);
            return GenerateEnemy(state.Floor, state.Player.GetLevel(), enemyType, roomType);
        }

        /// <summary>
        /// Generate an enemy with specific type.
        /// </summary>
        public static Entity GenerateEnemy(int floor, int playerLevel, EnemyType enemyType, RoomType roomType = RoomType.Encounter)
        {
            // Get base template
            var template = GetTemplate(enemyType, roomType);

            // Calculate scaling
            float floorScaling = 1.0f + (floor - 1) * 0.15f;
            float levelScaling = 1.0f + (playerLevel - 1) * 0.05f;
            float combinedScaling = floorScaling * levelScaling;

            // Scale stats
            float health = template.Health * combinedScaling;
            float damage = template.Damage * combinedScaling;
            float defense = template.Defense * combinedScaling;

            // Create enemy entity
            var enemy = new Entity(
                name: GetEnemyName(enemyType),
                maxHealth: health,
                healthRegen: template.HealthRegen,
                statCount: 10);

            // Set stats
            enemy.Stats.SetBase((int)StatType.HealthMax, health);
            enemy.Stats.SetBase((int)StatType.HealthRegen, template.HealthRegen);
            enemy.Stats.SetBase((int)StatType.AttackDamage, damage);
            enemy.Stats.SetBase((int)StatType.Defense, defense);
            enemy.Stats.SetBase((int)StatType.AttackSpeed, template.AttackSpeed);
            enemy.Stats.SetBase((int)StatType.CritChance, template.CritChance);
            enemy.Stats.SetBase((int)StatType.CritMultiplier, 1.5f);

            // Set level to match floor
            enemy.Level = floor;

            // Adjust attack cooldown based on attack speed
            enemy.AttackCooldown = new Variable.Timer.Cooldown(1.0f / template.AttackSpeed);

            return enemy;
        }

        /// <summary>
        /// Get template for enemy type and room type.
        /// </summary>
        private static EnemyTemplate GetTemplate(EnemyType enemyType, RoomType roomType)
        {
            // Handle room-based modifications
            if (roomType == RoomType.Elite)
            {
                return EliteTemplate;
            }
            if (roomType == RoomType.Boss)
            {
                return enemyType == EnemyType.VoidTerror ? BossTemplate : EliteTemplate;
            }

            // Get base template for enemy type
            return enemyType switch
            {
                EnemyType.Goblin => Templates[0],
                EnemyType.Slime => Templates[1],
                EnemyType.Skeleton => Templates[2],
                EnemyType.Orc => Templates[3],
                EnemyType.Lich => Templates[4],
                EnemyType.Golem => Templates[5],

                // Bosses use boss template
                EnemyType.Dragon or EnemyType.VoidTerror => BossTemplate,

                _ => Templates[2]  // Default to Skeleton
            };
        }

        /// <summary>
        /// Select enemy type based on floor and random seed.
        /// </summary>
        private static EnemyType SelectEnemyType(in GameState state, RoomType roomType)
        {
            var rng = state.GetRng();
            int floor = state.Floor;

            // Boss floors
            if (roomType == RoomType.Boss)
            {
                return floor % 5 == 0 ? EnemyType.VoidTerror : EnemyType.Dragon;
            }

            // Elite rooms have different pool
            if (roomType == RoomType.Elite)
            {
                return rng.Next(0, 100) switch
                {
                    < 30 => EnemyType.Orc,
                    < 60 => EnemyType.Golem,
                    < 90 => EnemyType.Lich,
                    _ => EnemyType.Dragon
                };
            }

            // Normal enemies - pool changes by floor
            int roll = rng.Next(0, 100);

            if (floor <= 2)
            {
                // Early game: easy enemies
                return roll switch
                {
                    < 40 => EnemyType.Slime,
                    < 80 => EnemyType.Goblin,
                    _ => EnemyType.Skeleton
                };
            }
            else if (floor <= 5)
            {
                // Mid game: more variety
                return roll switch
                {
                    < 20 => EnemyType.Slime,
                    < 40 => EnemyType.Goblin,
                    < 70 => EnemyType.Skeleton,
                    < 90 => EnemyType.Orc,
                    _ => EnemyType.Lich
                };
            }
            else
            {
                // Late game: all enemies
                return roll switch
                {
                    < 15 => EnemyType.Slime,
                    < 25 => EnemyType.Goblin,
                    < 45 => EnemyType.Skeleton,
                    < 65 => EnemyType.Orc,
                    < 85 => EnemyType.Lich,
                    _ => EnemyType.Golem
                };
            }
        }

        // =====================
        // UTILITY METHODS
        // =====================

        /// <summary>
        /// Get display name for enemy type.
        /// </summary>
        public static string GetEnemyName(EnemyType enemyType)
        {
            return enemyType switch
            {
                EnemyType.Goblin => "Goblin",
                EnemyType.Slime => "Slime",
                EnemyType.Skeleton => "Skeleton",
                EnemyType.Orc => "Orc Warrior",
                EnemyType.Lich => "Dark Lich",
                EnemyType.Golem => "Stone Golem",
                EnemyType.Dragon => "Ancient Dragon",
                EnemyType.VoidTerror => "Void Terror",
                _ => "Unknown Enemy"
            };
        }

        /// <summary>
        /// Get enemy description with stat summary.
        /// </summary>
        public static string GetEnemyDescription(in Entity enemy)
        {
            float hp = enemy.Stats.Get((int)StatType.HealthMax);
            float damage = enemy.Stats.Get((int)StatType.AttackDamage);
            float defense = enemy.Stats.Get((int)StatType.Defense);
            float speed = enemy.Stats.Get((int)StatType.AttackSpeed);

            return $"HP: {hp:F0} | DMG: {damage:F0} | DEF: {defense:F0} | SPD: {speed:F1}";
        }

        /// <summary>
        /// Get threat level (1-5) based on stats.
        /// </summary>
        public static int GetThreatLevel(in Entity enemy)
        {
            float hp = enemy.Stats.Get((int)StatType.HealthMax);
            float damage = enemy.Stats.Get((int)StatType.AttackDamage);
            float score = hp + damage * 5;

            return score switch
            {
                < 100 => 1,
                < 200 => 2,
                < 300 => 3,
                < 400 => 4,
                _ => 5
            };
        }

        /// <summary>
        /// Generate a room type based on floor progress.
        /// </summary>
        public static RoomType GenerateRoomType(in GameState state)
        {
            var rng = state.GetRng();
            int room = state.Room;
            int floor = state.Floor;
            int roomsPerFloor = state.RoomsPerFloor;

            // Last room is always boss
            if (room == roomsPerFloor)
            {
                return RoomType.Boss;
            }

            // Middle room is elite
            if (room == roomsPerFloor / 2)
            {
                return RoomType.Elite;
            }

            // Random room type
            int roll = rng.Next(0, 100);

            // Adjust probabilities based on floor
            int shopChance = 10 + floor * 2;
            int shrineChance = 8 + floor;
            int treasureChance = 10;
            int restChance = 5;

            int normalChance = 100 - shopChance - shrineChance - treasureChance - restChance;

            if (roll < normalChance)
                return RoomType.Encounter;
            else if (roll < normalChance + shopChance)
                return RoomType.Shop;
            else if (roll < normalChance + shopChance + shrineChance)
                return RoomType.Shrine;
            else if (roll < normalChance + shopChance + shrineChance + treasureChance)
                return RoomType.Treasure;
            else
                return RoomType.Rest;
        }
    }
}
