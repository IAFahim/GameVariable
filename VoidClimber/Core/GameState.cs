using System;
using System.Runtime.InteropServices;
using Variable.Input;

namespace VoidClimber.Core
{
    /// <summary>
    /// Complete game state containing all data for the current session.
    /// This is the single source of truth passed to all systems.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct GameState
    {
        // =====================
        // GAME STATE
        // =====================

        /// <summary>Current phase of the game</summary>
        public GamePhase Phase;

        /// <summary>Current floor number (1-based)</summary>
        public int Floor;

        /// <summary>Current room number on this floor</summary>
        public int Room;

        /// <summary>Total rooms on this floor</summary>
        public int RoomsPerFloor;

        /// <summary>Random seed for procedural generation</summary>
        public int Seed;

        // =====================
        // ENTITIES
        // =====================

        /// <summary>Player data</summary>
        public PlayerData Player;

        /// <summary>Current enemy (if in combat)</summary>
        public Entity CurrentEnemy;

        /// <summary>Enemy type for this encounter</summary>
        public EnemyType CurrentEnemyType;

        /// <summary>Current room type</summary>
        public RoomType CurrentRoomType;

        // =====================
        // COMBAT STATE
        // =====================

        /// <summary>True if player can act this turn</summary>
        public bool IsPlayerTurn;

        /// <summary>Combat turn counter</summary>
        public int CombatTurn;

        /// <summary>Global time scale (affects cooldowns)</summary>
        public float GlobalTimeScale;

        /// <summary>Last damage dealt by player (for display)</summary>
        public float LastPlayerDamage;

        /// <summary>Last damage taken by player (for display)</summary>
        public float LastPlayerDamageTaken;

        // =====================
        // INPUT SYSTEM - Variable.Input
        // =====================

        /// <summary>Input buffer for combo detection</summary>
        public InputRingBuffer InputBuffer;

        /// <summary>Last combo action triggered</summary>
        public ComboAction LastCombo;

        /// <summary>Combo counter (consecutive successful combos)</summary>
        public int ComboCount;

        // =====================
        // SHOP/ECONOMY STATE
        // =====================

        /// <number>Shop visit count this floor</summary>
        public int ShopVisits;

        /// <summary>Prices (fluctuate based on floor)</summary>
        public int PotionCost;
        public int KeyCost;

        // =====================
        // POWERUP SELECTION
        // =====================

        /// <summary>Available powerups when leveling up</summary>
        public PowerupType[] PowerupOptions;

        /// <summary>Number of powerup options</summary>
        public int PowerupCount;

        // =====================
        // ACHIEVEMENTS/STATS
        // =====================

        /// <summary>Number of elites defeated</summary>
        public int ElitesKilled;

        /// <summary>Number of bosses defeated</summary>
        public int BossesKilled;

        /// <summary>Number of shrines visited</summary>
        public int ShrinesVisited;

        /// <summary>Number of shops visited</summary>
        public int ShopsVisited;

        // =====================
        // CONSTRUCTORS
        // =====================

        /// <summary>
        /// Create a new game state with default values.
        /// </summary>
        public GameState(int seed = 0)
        {
            Phase = GamePhase.Menu;
            Floor = 1;
            Room = 0;
            RoomsPerFloor = 10;
            Seed = seed;

            // Initialize player
            Player = new PlayerData("Hero", 100f, 100);

            // Initialize enemy (invalid until combat)
            CurrentEnemy = default;
            CurrentEnemyType = EnemyType.Skeleton;
            CurrentRoomType = RoomType.Encounter;

            // Combat state
            IsPlayerTurn = true;
            CombatTurn = 0;
            GlobalTimeScale = 1.0f;
            LastPlayerDamage = 0f;
            LastPlayerDamageTaken = 0f;

            // Input system
            InputBuffer = new InputRingBuffer();
            LastCombo = ComboAction.None;
            ComboCount = 0;

            // Economy
            ShopVisits = 0;
            PotionCost = 50;
            KeyCost = 100;

            // Powerups
            PowerupOptions = new PowerupType[3];
            PowerupCount = 0;

            // Achievements
            ElitesKilled = 0;
            BossesKilled = 0;
            ShrinesVisited = 0;
            ShopsVisited = 0;
        }

        // =====================
        // FLOOR MANAGEMENT
        // =====================

        /// <summary>
        /// Advance to the next room.
        /// Returns true if floor was cleared.
        /// </summary>
        public bool AdvanceRoom()
        {
            Room++;

            // Check if floor cleared
            if (Room > RoomsPerFloor)
            {
                Floor++;
                Room = 1;
                Player.FloorsCleared++;
                if (Floor > Player.HighestFloor)
                {
                    Player.HighestFloor = Floor;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reset room counter for new floor.
        /// </summary>
        public void ResetFloorProgress()
        {
            Room = 1;
        }

        /// <summary>
        /// Get progress through current floor (0-1).
        /// </summary>
        public readonly float GetFloorProgress()
        {
            return (float)Room / RoomsPerFloor;
        }

        // =====================
        // COMBAT MANAGEMENT
        // =====================

        /// <summary>
        /// Start combat with an enemy.
        /// </summary>
        public void StartCombat(in Entity enemy, EnemyType enemyType)
        {
            CurrentEnemy = enemy;
            CurrentEnemyType = enemyType;
            Phase = GamePhase.Combat;
            IsPlayerTurn = true;
            CombatTurn = 1;
            LastPlayerDamage = 0f;
            LastPlayerDamageTaken = 0f;
        }

        /// <summary>
        /// End combat (victory or defeat).
        /// </summary>
        public void EndCombat(bool victory)
        {
            if (victory)
            {
                // Grant rewards
                int goldReward = CalculateGoldReward();
                int xpReward = CalculateXpReward();

                Player.AddGold(goldReward);
                Player.Kills++;

                // Add XP and check for level up
                var levelsGained = Variable.Experience.ExperienceExtensions.Add(
                    ref Player.Experience,
                    xpReward,
                    new Logic.LinearXpFormula());

                if (levelsGained > 0)
                {
                    Player.BaseEntity.Level += levelsGained;
                    Phase = GamePhase.LevelUp;
                }
                else
                {
                    Phase = GamePhase.Exploration;
                }
            }
            else
            {
                Phase = GamePhase.GameOver;
            }

            // Reset combat state
            IsPlayerTurn = true;
            CombatTurn = 0;
        }

        /// <summary>
        /// Calculate gold reward based on enemy and floor.
        /// </summary>
        private readonly int CalculateGoldReward()
        {
            int baseGold = CurrentEnemyType switch
            {
                EnemyType.Goblin => 10,
                EnemyType.Slime => 5,
                EnemyType.Skeleton => 15,
                EnemyType.Orc => 25,
                EnemyType.Lich => 40,
                EnemyType.Golem => 35,
                EnemyType.Dragon => 100,  // Boss
                EnemyType.VoidTerror => 150,  // Boss
                _ => 10
            };

            return (int)(baseGold * (1 + Floor * 0.1f));
        }

        /// <summary>
        /// Calculate XP reward based on enemy and floor.
        /// </summary>
        private readonly int CalculateXpReward()
        {
            int baseXp = CurrentEnemyType switch
            {
                EnemyType.Goblin => 20,
                EnemyType.Slime => 10,
                EnemyType.Skeleton => 30,
                EnemyType.Orc => 50,
                EnemyType.Lich => 80,
                EnemyType.Golem => 70,
                EnemyType.Dragon => 200,  // Boss
                EnemyType.VoidTerror => 300,  // Boss
                _ => 20
            };

            return (int)(baseXp * (1 + Floor * 0.15f));
        }

        // =====================
        // UTILITY METHODS
        // =====================

        /// <summary>
        /// Check if player is alive.
        /// </summary>
        public readonly bool IsPlayerAlive()
        {
            return Player.BaseEntity.IsAlive;
        }

        /// <summary>
        /// Check if enemy is alive.
        /// </summary>
        public readonly bool IsEnemyAlive()
        {
            return Phase == GamePhase.Combat && CurrentEnemy.IsAlive;
        }

        /// <summary>
        /// Get random number generator with current seed.
        /// </summary>
        public readonly Random GetRng()
        {
            return new Random(Seed + Floor * 1000 + Room);
        }
    }
}
