using System;
using Variable.Experience;
using Variable.RPG;
using VoidClimber.Core;

namespace VoidClimber.Logic
{
    /// <summary>
    /// Progression system using Variable.Experience.
    /// Handles XP gain, level-ups, and powerup selection.
    /// </summary>
    public static class ProgressionLogic
    {
        // =====================
        // XP FORMULAS
        // =====================

        /// <summary>
        /// Linear XP curve: 100 * level
        /// Simple and predictable for the demo.
        /// </summary>
        public readonly struct LinearXpFormula : INextMaxFormula<int>
        {
            public int Calculate(int level) => level * 100;
        }

        /// <summary>
        /// Exponential XP curve: 100 * level^1.5
        /// Creates steeper requirements at higher levels.
        /// </summary>
        public readonly struct ExponentialXpFormula : INextMaxFormula<int>
        {
            public int Calculate(int level) => (int)(100 * Math.Pow(level, 1.5));
        }

        // =====================
        // XP MANAGEMENT
        // =====================

        /// <summary>
        /// Add XP to player and check for level-ups.
        /// </summary>
        /// <returns>Number of levels gained</returns>
        public static int AddXp(ref PlayerData player, int amount)
        {
            // Use ExperienceExtensions to add XP
            int levelsGained = Variable.Experience.ExperienceExtensions.Add(
                ref player.Experience,
                amount,
                new LinearXpFormula());

            // Update player level
            if (levelsGained > 0)
            {
                player.BaseEntity.Level += levelsGained;

                // Heal player on level up
                player.BaseEntity.Health.Value.Current = player.BaseEntity.Health.Value.Max;
            }

            return levelsGained;
        }

        /// <summary>
        /// Get total XP required for a specific level.
        /// </summary>
        public static int GetXpForLevel(int level)
        {
            return new LinearXpFormula().Calculate(level);
        }

        /// <summary>
        /// Get current XP progress (0-1).
        /// </summary>
        public static float GetXpProgress(in PlayerData player)
        {
            return Variable.Bounded.BoundedExtensions.GetRatio(player.Experience);
        }

        // =====================
        // POWERUP GENERATION
        // =====================

        /// <summary>
        /// Generate random powerup options for level-up.
        /// Uses procedural generation with seed.
        /// </summary>
        public static void GeneratePowerups(in GameState state, Span<PowerupType> options)
        {
            var rng = state.GetRng();
            int playerLevel = state.Player.GetLevel();

            // Generate 3 random options
            for (int i = 0; i < options.Length && i < 3; i++)
            {
                // Weight towards different stats based on level
                options[i] = rng.Next(0, 100) switch
                {
                    < 15 when playerLevel < 3 => PowerupType.FlatHealth,       // Early game: prioritize HP
                    < 25 => PowerupType.FlatDamage,                           // Always good
                    < 35 => PowerupType.FlatDefense,
                    < 45 when playerLevel > 5 => PowerupType.FlatCritChance,   // Mid game: crit
                    < 55 when playerLevel > 3 => PowerupType.FlatRegen,
                    < 65 when playerLevel > 5 => PowerupType.FlatSpeed,
                    < 75 => PowerupType.Lifesteal,
                    < 85 when playerLevel > 7 => PowerupType.DodgeChance,
                    < 95 => PowerupType.Thorns,
                    _ => PowerupType.FlatHealth
                };
            }
        }

        // =====================
        // POWERUP APPLICATION
        // =====================

        /// <summary>
        /// Apply a powerup to the player's stats.
        /// Uses Variable.RPG modifier system.
        /// </summary>
        public static void ApplyPowerup(ref PlayerData player, PowerupType powerup)
        {
            int level = player.GetLevel();

            // Create modifier based on powerup type
            RpgStatModifier modifier = powerup switch
            {
                // Flat increases
                PowerupType.FlatHealth => RpgStatModifier.AddFlat(RpgStatField.Base, 10f + level * 2f),
                PowerupType.FlatDamage => RpgStatModifier.AddFlat(RpgStatField.Base, 3f + level * 0.5f),
                PowerupType.FlatDefense => RpgStatModifier.AddFlat(RpgStatField.Base, 2f + level * 0.3f),
                PowerupType.FlatRegen => RpgStatModifier.AddFlat(RpgStatField.Base, 1f + level * 0.1f),
                PowerupType.FlatSpeed => RpgStatModifier.AddFlat(RpgStatField.Base, 0.1f),
                PowerupType.FlatCritChance => RpgStatModifier.AddFlat(RpgStatField.Base, 0.03f),
                PowerupType.FlatCritMult => RpgStatModifier.AddFlat(RpgStatField.Base, 0.1f),
                PowerupType.Lifesteal => RpgStatModifier.AddFlat(RpgStatField.Base, 0.05f),
                PowerupType.Thorns => RpgStatModifier.AddFlat(RpgStatField.Base, 3f),
                PowerupType.DodgeChance => RpgStatModifier.AddFlat(RpgStatField.Base, 0.05f),

                // Percentage increases
                PowerupType.PercentHealth => RpgStatModifier.AddPercent(RpgStatField.ModMult, 0.1f),
                PowerupType.PercentDamage => RpgStatModifier.AddPercent(RpgStatField.ModMult, 0.1f),
                PowerupType.PercentSpeed => RpgStatModifier.AddPercent(RpgStatField.ModMult, 0.15f),

                _ => RpgStatModifier.AddFlat(RpgStatField.Base, 0f)
            };

            // Determine which stat to modify
            StatType targetStat = powerup switch
            {
                PowerupType.FlatHealth or PowerupType.PercentHealth => StatType.HealthMax,
                PowerupType.FlatDamage or PowerupType.PercentDamage => StatType.AttackDamage,
                PowerupType.FlatDefense => StatType.Defense,
                PowerupType.FlatRegen => StatType.HealthRegen,
                PowerupType.FlatSpeed or PowerupType.PercentSpeed => StatType.AttackSpeed,
                PowerupType.FlatCritChance => StatType.CritChance,
                PowerupType.FlatCritMult => StatType.CritMultiplier,
                PowerupType.Lifesteal => StatType.Lifesteal,
                PowerupType.Thorns => StatType.Thorns,
                PowerupType.DodgeChance => StatType.DodgeChance,
                _ => StatType.AttackDamage
            };

            // Apply modifier using RpgStatExtensions
            ref var stat = ref player.BaseEntity.Stats.GetRef((int)targetStat);
            RpgStatExtensions.ApplyModifier(ref stat, modifier);

            // Special case: if max HP changed, update RegenFloat
            if (targetStat == StatType.HealthMax)
            {
                float newMax = player.BaseEntity.Stats.Get((int)StatType.HealthMax);
                player.BaseEntity.Health.Value.Max = newMax;
                player.BaseEntity.Health.Value.Current = newMax;
            }
        }

        /// <summary>
        /// Get display string for a powerup.
        /// </summary>
        public static string GetPowerupDescription(PowerupType powerup, int playerLevel)
        {
            int level = playerLevel;

            return powerup switch
            {
                PowerupType.FlatHealth => $"+{10 + level * 2} Max Health",
                PowerupType.PercentHealth => "+10% Max Health",
                PowerupType.FlatDamage => $"+{3 + level * 0.5f:F1} Attack Damage",
                PowerupType.PercentDamage => "+10% Attack Damage",
                PowerupType.FlatDefense => $"+{2 + level * 0.3f:F1} Defense",
                PowerupType.FlatRegen => $"+{1 + level * 0.1f:F1} HP/sec Regen",
                PowerupType.FlatSpeed => "+10% Attack Speed",
                PowerupType.PercentSpeed => "+15% Attack Speed",
                PowerupType.FlatCritChance => "+3% Crit Chance",
                PowerupType.FlatCritMult => "+10% Crit Damage",
                PowerupType.Lifesteal => "+5% Lifesteal",
                PowerupType.Thorns => $"+{3} Thorns Damage",
                PowerupType.DodgeChance => "+5% Dodge Chance",
                _ => "Unknown Powerup"
            };
        }

        // =====================
        // SHRINE EFFECTS
        // =====================

        /// <summary>
        /// Apply shrine effect to player.
        /// Shrines offer risk/reward choices.
        /// </summary>
        public static void ApplyShrineEffect(ref PlayerData player, ShrineEffect effect, out string description)
        {
            var rng = new Random();

            switch (effect)
            {
                case ShrineEffect.BloodAltar:
                    // Lose 25% HP, gain permanent damage
                    float hpLoss = player.BaseEntity.Health.Value.Max * 0.25f;
                    player.BaseEntity.Health.Value.Current -= hpLoss;
                    ApplyPowerup(ref player, PowerupType.FlatDamage);
                    ApplyPowerup(ref player, PowerupType.FlatDamage);
                    description = "Sacrificed 25% HP for +Attack Damage";
                    break;

                case ShrineEffect.FontOfHealth:
                    // Gain 20% Max HP, lose damage
                    ApplyPowerup(ref player, PowerupType.PercentHealth);
                    ApplyPowerup(ref player, PowerupType.PercentHealth);
                    // Reduce damage (not easily done without negative modifiers)
                    description = "Gained +20% Max HP";
                    break;

                case ShrineEffect.CombatMastery:
                    // Gain crit, lose defense
                    ApplyPowerup(ref player, PowerupType.FlatCritChance);
                    ApplyPowerup(ref player, PowerupType.FlatCritChance);
                    description = "Gained +6% Crit Chance";
                    break;

                case ShrineEffect.IronWill:
                    // Gain defense, lose speed
                    ApplyPowerup(ref player, PowerupType.FlatDefense);
                    ApplyPowerup(ref player, PowerupType.FlatDefense);
                    description = "Gained +Defense";
                    break;

                case ShrineEffect.VoidBargain:
                    // 50% chance: double power or lose half
                    if (rng.NextDouble() < 0.5)
                    {
                        ApplyPowerup(ref player, PowerupType.FlatDamage);
                        ApplyPowerup(ref player, PowerupType.FlatDamage);
                        description = "VOID BLESSED YOU! Double power gained!";
                    }
                    else
                    {
                        player.BaseEntity.Health.Value.Current *= 0.5f;
                        description = "VOID CURSED YOU! Lost 50% HP!";
                    }
                    break;

                case ShrineEffect.TimeWarp:
                    // Full heal, future consequences
                    player.BaseEntity.Health.Value.Current = player.BaseEntity.Health.Value.Max;
                    description = "Full heal! But enemies grow stronger...";
                    break;

                default:
                    description = "Nothing happened";
                    break;
            }
        }

        // =====================
        // STAT SCALING
        // =====================

        /// <summary>
        /// Scale enemy stats based on floor and player level.
        /// </summary>
        public static void ScaleEntity(ref Entity entity, int floor, int playerLevel)
        {
            float scaleFactor = 1.0f + (floor - 1) * 0.15f + (playerLevel - 1) * 0.05f;

            // Scale all stats
            float maxHp = entity.Stats.Get((int)StatType.HealthMax) * scaleFactor;
            float damage = entity.Stats.Get((int)StatType.AttackDamage) * scaleFactor;
            float defense = entity.Stats.Get((int)StatType.Defense) * scaleFactor;

            entity.Stats.SetBase((int)StatType.HealthMax, maxHp);
            entity.Stats.SetBase((int)StatType.AttackDamage, damage);
            entity.Stats.SetBase((int)StatType.Defense, defense);

            // Update health
            entity.Health = new Variable.Regen.RegenFloat(maxHp, maxHp, 0f);
        }
    }
}
