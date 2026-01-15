using System;
using VoidClimber.Core;
using Variable.RPG;

namespace VoidClimber.Logic
{
    /// <summary>
    /// Combat system using Variable.RPG damage resolution.
    /// Demonstrates the Diamond Architecture for damage calculation.
    /// </summary>
    public static class CombatLogic
    {
        // =====================
        // DAMAGE CONFIGURATION
        // =====================

        /// <summary>
        /// Damage configuration implementing IDamageConfig.
        /// Defines how damage elements interact with defender stats.
        /// </summary>
        private readonly struct DamageConfig : IDamageConfig
        {
            public bool TryGetMitigationStat(int elementId, out int statId, out bool isFlat)
            {
                // Physical damage (0) is mitigated by Defense (3) via flat reduction
                if (elementId == (int)DamageElement.Physical)
                {
                    statId = (int)StatType.Defense;
                    isFlat = true;
                    return true;
                }

                // Fire damage (1) is mitigated by max health percentage (damage reduction)
                if (elementId == (int)DamageElement.Fire)
                {
                    statId = (int)StatType.HealthMax;
                    isFlat = false;  // Percentage-based
                    return true;
                }

                // All other damage types have no mitigation in MVP
                statId = -1;
                isFlat = false;
                return false;
            }
        }

        // =====================
        // DAMAGE RESOLUTION
        // =====================

        /// <summary>
        /// Resolve an attack from attacker to defender.
        /// Uses Variable.RPG's diamond architecture for damage calculation.
        /// </summary>
        /// <param name="attacker">Entity initiating the attack</param>
        /// <param name="defender">Entity being attacked</param>
        /// <param name="damageDealt">Final damage after mitigation</param>
        /// <param name="isKill">Whether the attack killed the defender</param>
        /// <param name="isCrit">Whether the attack was a critical hit</param>
        public static void ResolveAttack(
            ref Entity attacker,
            ref Entity defender,
            out float damageDealt,
            out bool isKill,
            out bool isCrit)
        {
            isKill = false;
            isCrit = false;

            // 1. Get attacker stats
            float baseDamage = attacker.Stats.Get((int)StatType.AttackDamage);
            float critChance = attacker.Stats.Get((int)StatType.CritChance);
            float critMultiplier = attacker.Stats.Get((int)StatType.CritMultiplier);
            float attackSpeed = attacker.Stats.Get((int)StatType.AttackSpeed);

            // 2. Roll for critical hit
            var rng = new Random();
            isCrit = rng.NextDouble() < critChance;

            // 3. Apply crit multiplier if applicable
            if (isCrit)
            {
                baseDamage *= critMultiplier;
            }

            // 4. Create damage packet
            // Using stackalloc for zero-allocation performance
            Span<DamagePacket> packets = stackalloc DamagePacket[1];
            packets[0] = new DamagePacket
            {
                ElementId = (int)DamageElement.Physical,
                Amount = baseDamage,
                Flags = isCrit ? 1 : 0  // Store crit flag
            };

            // 5. Resolve damage using Variable.RPG extension
            // This applies mitigation based on defender's stats
            damageDealt = defender.Stats.AsSpan().ResolveDamage(packets, new DamageConfig());

            // 6. Apply damage to health using BoundedFloat logic
            Variable.Bounded.BoundedExtensions.TryConsume(ref defender.Health.Value, damageDealt);

            // 7. Apply lifesteal if attacker has it
            float lifesteal = attacker.Stats.Get((int)StatType.Lifesteal);
            if (lifesteal > 0)
            {
                float healAmount = damageDealt * lifesteal;
                float currentHp = attacker.Health.Value.Current;
                float maxHp = attacker.Health.Value.Max;

                Variable.Inventory.InventoryLogic.AddPartial(
                    ref currentHp, healAmount, maxHp, out _);

                attacker.Health.Value.Current = currentHp;
            }

            // 8. Apply thorns if defender has it
            float thorns = defender.Stats.Get((int)StatType.Thorns);
            if (thorns > 0)
            {
                float thornDamage = thorns;
                Variable.Bounded.BoundedExtensions.TryConsume(ref attacker.Health.Value, thornDamage);
            }

            // 9. Check for death
            isKill = Variable.Bounded.BoundedExtensions.IsEmpty(defender.Health.Value);
            if (isKill)
            {
                defender.IsAlive = false;
            }
        }

        /// <summary>
        /// Resolve a special attack with multiple damage types.
        /// Demonstrates multi-element damage resolution.
        /// </summary>
        public static void ResolveSpecialAttack(
            ref Entity attacker,
            ref Entity defender,
            out float totalDamage,
            out bool isKill)
        {
            totalDamage = 0f;
            isKill = false;

            // Create multiple damage packets (physical + fire)
            Span<DamagePacket> packets = stackalloc DamagePacket[2];

            float baseDamage = attacker.Stats.Get((int)StatType.AttackDamage);

            packets[0] = new DamagePacket
            {
                ElementId = (int)DamageElement.Physical,
                Amount = baseDamage * 0.6f,  // 60% physical
                Flags = 0
            };

            packets[1] = new DamagePacket
            {
                ElementId = (int)DamageElement.Fire,
                Amount = baseDamage * 0.4f,  // 40% fire
                Flags = 0
            };

            // Resolve all damage types
            totalDamage = defender.Stats.AsSpan().ResolveDamage(packets, new DamageConfig());

            // Apply damage
            Variable.Bounded.BoundedExtensions.TryConsume(ref defender.Health.Value, totalDamage);

            // Check death
            isKill = Variable.Bounded.BoundedExtensions.IsEmpty(defender.Health.Value);
            if (isKill)
            {
                defender.IsAlive = false;
            }
        }

        // =====================
        // UTILITY METHODS
        // =====================

        /// <summary>
        /// Calculate expected damage range for an entity.
        /// Useful for display purposes.
        /// </summary>
        public static (float min, float max) GetDamageRange(in Entity entity)
        {
            float baseDamage = entity.Stats.Get((int)StatType.AttackDamage);
            float critChance = entity.Stats.Get((int)StatType.CritChance);
            float critMultiplier = entity.Stats.Get((int)StatType.CritMultiplier);

            float minDamage = baseDamage;
            float maxDamage = baseDamage * critMultiplier;

            // Average in crit chance
            float avgDamage = minDamage * (1 - critChance) + maxDamage * critChance;

            return (minDamage, avgDamage);
        }

        /// <summary>
        /// Check if an entity can dodge.
        /// </summary>
        public static bool TryDodge(in Entity defender)
        {
            float dodgeChance = defender.Stats.Get((int)StatType.DodgeChance);
            if (dodgeChance <= 0) return false;

            var rng = new Random();
            return rng.NextDouble() < dodgeChance;
        }

        /// <summary>
        /// Apply damage over time (poison/burn).
        /// </summary>
        public static void ApplyDamageOverTime(ref Entity entity, int stacks, DamageElement type)
        {
            if (stacks <= 0 || !entity.IsAlive) return;

            float damagePerStack = type switch
            {
                DamageElement.Poison => 3f,
                DamageElement.Fire => 5f,
                _ => 0f
            };

            float totalDamage = damagePerStack * stacks;

            // Apply damage (ignores defense/m mitigation)
            Variable.Bounded.BoundedExtensions.TryConsume(ref entity.Health.Value, totalDamage);

            // Check death
            if (Variable.Bounded.BoundedExtensions.IsEmpty(entity.Health.Value))
            {
                entity.IsAlive = false;
            }
        }

        /// <summary>
        /// Reset all cooldowns after combat.
        /// </summary>
        public static void ResetCombat(ref Entity entity)
        {
            entity.AttackCooldown.Reset();
            entity.SpecialCooldown.Reset();
            entity.StunTurns = 0;
            entity.PoisonStacks = 0;
            entity.BurnStacks = 0;
        }
    }
}
