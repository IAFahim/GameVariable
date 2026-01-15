using System;
using VoidClimber.Core;
using VoidClimber.Logic;

namespace VoidClimber.Systems
{
    /// <summary>
    /// Main game loop with tick-based simulation.
    /// Demonstrates real-time systems (Regen, Cooldown, Timer) in a turn-based game.
    /// </summary>
    public static class GameLoop
    {
        private const float TICK_RATE = 0.1f;  // 100ms per tick

        // =====================
        // CORE LOOP
        // =====================

        /// <summary>
        /// Main game tick - advances all time-based systems.
        /// </summary>
        /// <param name="state">Current game state</param>
        /// <param name="deltaTime">Time since last tick (seconds)</param>
        public static void Tick(ref GameState state, float deltaTime)
        {
            // Apply global time scale
            float scaledDelta = deltaTime * state.GlobalTimeScale;

            // 1. Regenerate player health (Variable.Regen)
            if (state.Player.BaseEntity.IsAlive)
            {
                state.Player.BaseEntity.Health.Tick(scaledDelta);

                // Sync max HP from stats (in case it changed from buffs)
                SyncHealthToMax(ref state.Player.BaseEntity);
            }

            // 2. Regenerate enemy health
            if (state.Phase == GamePhase.Combat && state.CurrentEnemy.IsAlive)
            {
                state.CurrentEnemy.Health.Tick(scaledDelta);
                SyncHealthToMax(ref state.CurrentEnemy);
            }

            // 3. Update cooldowns (Variable.Timer)
            UpdateCooldowns(ref state, scaledDelta);

            // 4. Process combat if in combat phase
            if (state.Phase == GamePhase.Combat)
            {
                ProcessCombat(ref state, scaledDelta);
            }
        }

        // =====================
        // HEALTH SYNCHRONIZATION
        // =====================

        /// <summary>
        /// Synchronize RegenFloat max value with RpgStatSheet.
        /// This ensures that stat changes (buffs, level-ups) update health bounds.
        /// </summary>
        private static void SyncHealthToMax(ref Entity entity)
        {
            float statMaxHp = entity.Stats.Get((int)StatType.HealthMax);
            float currentMax = entity.Health.Value.Max;

            // Update max if stat changed (with small tolerance for float comparison)
            if (Math.Abs(statMaxHp - currentMax) > 0.01f)
            {
                entity.Health.Value.Max = statMaxHp;

                // Ensure current doesn't exceed new max
                if (entity.Health.Value.Current > statMaxHp)
                {
                    entity.Health.Value.Current = statMaxHp;
                }
            }
        }

        // =====================
        // COOLDOWN MANAGEMENT
        // =====================

        /// <summary>
        /// Update all cooldowns for player and enemy.
        /// </summary>
        private static void UpdateCooldowns(ref GameState state, float deltaTime)
        {
            // Player cooldowns
            if (state.Player.BaseEntity.IsAlive)
            {
                // Attack cooldown affected by attack speed stat
                float attackSpeed = state.Player.BaseEntity.Stats.Get((int)StatType.AttackSpeed);
                state.Player.BaseEntity.AttackCooldown.Tick(deltaTime * attackSpeed);

                // Special cooldown (not affected by speed)
                state.Player.BaseEntity.SpecialCooldown.Tick(deltaTime);
            }

            // Enemy cooldowns
            if (state.Phase == GamePhase.Combat && state.CurrentEnemy.IsAlive)
            {
                float enemySpeed = state.CurrentEnemy.Stats.Get((int)StatType.AttackSpeed);
                state.CurrentEnemy.AttackCooldown.Tick(deltaTime * enemySpeed);
                state.CurrentEnemy.SpecialCooldown.Tick(deltaTime);
            }
        }

        // =====================
        // COMBAT PROCESSING
        // =====================

        /// <summary>
        /// Process automatic combat (enemy attacks, DoT effects).
        /// Player attacks are triggered by input, not automatically.
        /// </summary>
        private static void ProcessCombat(ref GameState state, float deltaTime)
        {
            // Check if combat should end
            if (!state.Player.BaseEntity.IsAlive)
            {
                state.EndCombat(false);  // Player died
                return;
            }

            if (!state.CurrentEnemy.IsAlive)
            {
                state.EndCombat(true);  // Player won
                return;
            }

            // Process damage over time effects
            if (state.Player.BaseEntity.PoisonStacks > 0)
            {
                CombatLogic.ApplyDamageOverTime(ref state.Player.BaseEntity,
                    state.Player.BaseEntity.PoisonStacks, DamageElement.Poison);
            }

            if (state.Player.BaseEntity.BurnStacks > 0)
            {
                CombatLogic.ApplyDamageOverTime(ref state.Player.BaseEntity,
                    state.Player.BaseEntity.BurnStacks, DamageElement.Fire);
            }

            if (state.CurrentEnemy.PoisonStacks > 0)
            {
                CombatLogic.ApplyDamageOverTime(ref state.CurrentEnemy,
                    state.CurrentEnemy.PoisonStacks, DamageElement.Poison);
            }

            // Enemy AI: Attack when ready
            if (state.CurrentEnemy.CanAttack())
            {
                // Check if player dodges
                if (!CombatLogic.TryDodge(state.Player.BaseEntity))
                {
                    CombatLogic.ResolveAttack(
                        ref state.CurrentEnemy,
                        ref state.Player.BaseEntity,
                        out float damage,
                        out bool _,
                        out bool _);

                    state.LastPlayerDamageTaken = damage;
                    state.Player.TotalDamageTaken += damage;
                }

                // Reset enemy cooldown
                state.CurrentEnemy.AttackCooldown.Reset();
                state.CombatTurn++;
            }
        }

        // =====================
        // INPUT HANDLING
        // =====================

        /// <summary>
        /// Handle player attack input.
        /// </summary>
        public static bool HandlePlayerAttack(ref GameState state)
        {
            // Check if can attack
            if (!state.Player.BaseEntity.CanAttack())
            {
                return false;
            }

            // Check if enemy is alive
            if (!state.CurrentEnemy.IsAlive)
            {
                return false;
            }

            // Perform attack
            CombatLogic.ResolveAttack(
                ref state.Player.BaseEntity,
                ref state.CurrentEnemy,
                out float damage,
                out bool kill,
                out bool crit);

            state.LastPlayerDamage = damage;
            state.Player.TotalDamageDealt += damage;

            // Reset cooldown
            float attackSpeed = state.Player.BaseEntity.Stats.Get((int)StatType.AttackSpeed);
            float cooldownDuration = 1.0f / attackSpeed;
            state.Player.BaseEntity.AttackCooldown = new Variable.Timer.Cooldown(cooldownDuration);

            // Check if enemy died
            if (kill)
            {
                state.EndCombat(true);
            }

            return true;
        }

        /// <summary>
        /// Handle player special attack input.
        /// </summary>
        public static bool HandlePlayerSpecial(ref GameState state)
        {
            // Check if can use special
            if (!state.Player.BaseEntity.CanUseSpecial())
            {
                return false;
            }

            // Perform special attack
            CombatLogic.ResolveSpecialAttack(
                ref state.Player.BaseEntity,
                ref state.CurrentEnemy,
                out float damage,
                out bool kill);

            state.LastPlayerDamage = damage;
            state.Player.TotalDamageDealt += damage;

            // Reset cooldown
            state.Player.BaseEntity.SpecialCooldown.Reset();

            // Check if enemy died
            if (kill)
            {
                state.EndCombat(true);
            }

            return true;
        }

        /// <summary>
        /// Handle player using a potion.
        /// </summary>
        public static bool HandlePlayerPotion(ref GameState state)
        {
            return state.Player.UsePotion();
        }

        /// <summary>
        /// Handle player trying to flee.
        /// </summary>
        public static bool HandlePlayerFlee(ref GameState state)
        {
            // 50% chance to flee
            var rng = new Random();
            if (rng.NextDouble() < 0.5)
            {
                state.Phase = GamePhase.Exploration;
                CombatLogic.ResetCombat(ref state.Player.BaseEntity);
                return true;
            }

            return false;
        }

        // =====================
        // FLOOR PROGRESSION
        // =====================

        /// <summary>
        /// Advance to the next room.
        /// </summary>
        public static void AdvanceRoom(ref GameState state)
        {
            state.AdvanceRoom();

            // Generate new room type
            state.CurrentRoomType = EnemyGeneration.GenerateRoomType(state);

            // Handle different room types
            switch (state.CurrentRoomType)
            {
                case RoomType.Encounter:
                case RoomType.Elite:
                case RoomType.Boss:
                    // Spawn enemy
                    var enemy = EnemyGeneration.GenerateEnemy(state, state.CurrentRoomType);
                    state.StartCombat(enemy, EnemyGeneration.GetEnemyName(enemy) switch
                    {
                        "Goblin" => EnemyType.Goblin,
                        "Slime" => EnemyType.Slime,
                        "Skeleton" => EnemyType.Skeleton,
                        "Orc Warrior" => EnemyType.Orc,
                        "Dark Lich" => EnemyType.Lich,
                        "Stone Golem" => EnemyType.Golem,
                        "Ancient Dragon" => EnemyType.Dragon,
                        "Void Terror" => EnemyType.VoidTerror,
                        _ => EnemyType.Skeleton
                    });
                    break;

                case RoomType.Shop:
                    state.Phase = GamePhase.Shopping;
                    ShopLogic.UpdatePrices(ref state);
                    break;

                case RoomType.Treasure:
                    // Give gold/items
                    state.Player.AddGold(50 * state.Floor);
                    break;

                case RoomType.Shrine:
                    // Activate shrine
                    state.Phase = GamePhase.Exploration;  // TODO: Implement shrine UI
                    state.Player.ShrinesVisited++;
                    break;

                case RoomType.Rest:
                    // Full heal
                    state.Player.BaseEntity.Health.Value.Current =
                        state.Player.BaseEntity.Health.Value.Max;
                    break;

                case RoomType.Mystery:
                    // Random outcome
                    var rng = state.GetRng();
                    if (rng.NextDouble() < 0.5)
                    {
                        state.Player.AddGold(25 * state.Floor);
                    }
                    else
                    {
                        // Spawn enemy
                        var enemy = EnemyGeneration.GenerateEnemy(state, RoomType.Encounter);
                        state.StartCombat(enemy, EnemyType.Skeleton);
                    }
                    break;
            }
        }

        // =====================
        // GAME STATE MANAGEMENT
        // =====================

        /// <summary>
        /// Reset game to initial state.
        /// </summary>
        public static void ResetGame(ref GameState state, int newSeed = 0)
        {
            // Dispose of old player data
            state.Player.Dispose();

            // Create new state
            state = new GameState(newSeed);
        }

        /// <summary>
        /// Check for win condition (e.g., cleared 10 floors).
        /// </summary>
        public static bool CheckVictory(in GameState state)
        {
            return state.Floor >= 10 && !state.CurrentEnemy.IsAlive;
        }
    }
}
