using System;
using System.Text;
using VoidClimber.Core;

namespace VoidClimber.Systems
{
    /// <summary>
    /// Console rendering system for the game.
    /// Provides clean, formatted output for all game states.
    /// </summary>
    public static class GameRenderer
    {
        // =====================
        // COLORS & FORMATTING
        // =====================

        private const string ColorReset = "\u001b[0m";
        private const string ColorRed = "\u001b[31m";
        private const string ColorGreen = "\u001b[32m";
        private const string ColorYellow = "\u001b[33m";
        private const string ColorBlue = "\u001b[34m";
        private const string ColorMagenta = "\u001b[35m";
        private const string ColorCyan = "\u001b[36m";
        private const string ColorWhite = "\u001b[37m";
        private const string ColorBold = "\u001b[1m";

        // =====================
        // MAIN RENDER FUNCTIONS
        // =====================

        /// <summary>
        /// Render the game based on current phase.
        /// </summary>
        public static void Render(in GameState state)
        {
            Console.Clear();

            switch (state.Phase)
            {
                case GamePhase.Menu:
                    RenderMenu();
                    break;

                case GamePhase.Exploration:
                    RenderExploration(state);
                    break;

                case GamePhase.Combat:
                    RenderCombat(state);
                    break;

                case GamePhase.Shopping:
                    RenderShop(state);
                    break;

                case GamePhase.LevelUp:
                    RenderLevelUp(state);
                    break;

                case GamePhase.GameOver:
                    RenderGameOver(state);
                    break;

                case GamePhase.Victory:
                    RenderVictory(state);
                    break;
            }
        }

        // =====================
        // MENU SCREEN
        // =====================

        public static void RenderMenu()
        {
            Console.WriteLine($"{ColorBold}{ColorCyan}");
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║         VOID CLIMBER v1.0              ║");
            Console.WriteLine("║    A GameVariable Library Demo         ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.WriteLine(ColorReset);

            Console.WriteLine($"\n{ColorYellow}Features Demonstrated:{ColorReset}");
            Console.WriteLine("  • Variable.RPG - Stats, damage, modifiers");
            Console.WriteLine("  • Variable.Regen - Health regeneration");
            Console.WriteLine("  • Variable.Timer - Attack cooldowns");
            Console.WriteLine("  • Variable.Experience - Leveling system");
            Console.WriteLine("  • Variable.Reservoir - Potion management");
            Console.WriteLine("  • Variable.Inventory - Transaction logic");
            Console.WriteLine("  • Variable.Input - Combo system");

            Console.WriteLine($"\n{ColorGreen}Press any key to start...{ColorReset}");
        }

        // =====================
        // EXPLORATION SCREEN
        // =====================

        public static void RenderExploration(in GameState state)
        {
            // Header
            RenderHeader(state);

            Console.WriteLine($"\n{ColorBold}{ColorCyan}══════════════════════════════════════════{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorYellow}EXPLORATION - Floor {state.Floor}, Room {state.Room}/{state.RoomsPerFloor}{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorCyan}══════════════════════════════════════════{ColorReset}");

            // Room info
            Console.WriteLine($"\n{ColorGreen}Room Type: {state.CurrentRoomType}{ColorReset}");

            // Progress bar
            float progress = state.GetFloorProgress();
            int filled = (int)(progress * 40);
            Console.WriteLine($"\nFloor Progress:");
            Console.WriteLine($"{ColorCyan}[{new string('█', filled)}{new string('░', 40 - filled)}]{ColorReset} {(progress * 100):F0}%");

            // Options
            Console.WriteLine($"\n{ColorYellow}Options:{ColorReset}");
            Console.WriteLine("  [ENTER] - Continue to next room");
            Console.WriteLine("  [P] - Use Potion");
            Console.WriteLine("  [C] - Character Info");
            Console.WriteLine("  [Q] - Quit Game");
        }

        // =====================
        // COMBAT SCREEN
        // =====================

        public static void RenderCombat(in GameState state)
        {
            // Header
            RenderHeader(state);

            Console.WriteLine($"\n{ColorBold}{ColorRed}══════════════════════════════════════════{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorRed}COMBAT - Turn {state.CombatTurn}{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorRed}══════════════════════════════════════════{ColorReset}");

            // Enemy info
            Console.WriteLine($"\n{ColorBold}{ColorYellow}ENEMY{ColorReset}");
            Console.WriteLine($"  {state.CurrentEnemy.Name} (Lvl {state.CurrentEnemy.Level})");
            Console.WriteLine($"  HP: {state.CurrentEnemy.Health.Value.Current:F0}/{state.CurrentEnemy.Health.Value.Max:F0}");
            Console.WriteLine($"  {GetHealthBar(state.CurrentEnemy.Health)}");
            Console.WriteLine($"  {EnemyGeneration.GetEnemyDescription(state.CurrentEnemy)}");

            // Player info
            Console.WriteLine($"\n{ColorBold}{ColorGreen}PLAYER{ColorReset}");
            Console.WriteLine($"  HP: {state.Player.BaseEntity.Health.Value.Current:F0}/{state.Player.BaseEntity.Health.Value.Max:F0}");
            Console.WriteLine($"  {GetHealthBar(state.Player.BaseEntity.Health)}");

            // Cooldowns
            Console.WriteLine($"\n{ColorCyan}Cooldowns:{ColorReset}");
            Console.WriteLine($"  Attack: {(state.Player.BaseEntity.AttackCooldown.IsReady() ? "{ColorGreen}READY{ColorReset}" : $"{state.Player.BaseEntity.AttackCooldown.GetProgress() * 100:F0}%")}");
            Console.WriteLine($"  Special: {(state.Player.BaseEntity.SpecialCooldown.IsReady() ? "{ColorGreen}READY{ColorReset}" : $"{state.Player.BaseEntity.SpecialCooldown.GetProgress() * 100:F0}%")}");

            // Last damage
            if (state.LastPlayerDamage > 0)
            {
                Console.WriteLine($"\n{ColorGreen}You dealt {state.LastPlayerDamage:F1} damage!{ColorReset}");
            }
            if (state.LastPlayerDamageTaken > 0)
            {
                Console.WriteLine($"{ColorRed}You took {state.LastPlayerDamageTaken:F1} damage!{ColorReset}");
            }

            // Options
            Console.WriteLine($"\n{ColorYellow}Actions:{ColorReset}");
            Console.WriteLine("  [A] - Attack");
            Console.WriteLine("  [S] - Special Attack");
            Console.WriteLine("  [P] - Use Potion");
            Console.WriteLine("  [F] - Attempt to Flee (50%)");
        }

        // =====================
        // SHOP SCREEN
        // =====================

        public static void RenderShop(in GameState state)
        {
            RenderHeader(state);

            Console.WriteLine($"\n{ColorBold}{ColorYellow}══════════════════════════════════════════{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorCyan}SHOP - Welcome, adventurer!{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorYellow}══════════════════════════════════════════{ColorReset}");

            // Generate shop inventory
            var inventory = ShopLogic.GenerateShopInventory(state);

            Console.WriteLine($"\n{ColorGreen}Your Gold: {state.Player.Gold}g{ColorReset}\n");

            // Display items
            for (int i = 0; i < inventory.Length; i++)
            {
                var item = inventory[i];
                int cost = ShopLogic.GetItemCost(item, state.Floor);

                Console.WriteLine($"  [{i + 1}] {ColorCyan}{ShopLogic.GetItemName(item)}{ColorReset}");
                Console.WriteLine($"      Cost: {ColorYellow}{cost}g{ColorReset}");
                Console.WriteLine($"      {ShopLogic.GetItemDescription(item)}");
            }

            Console.WriteLine($"\n{ColorYellow}[1-{inventory.Length}] - Buy item  [ENTER] - Leave shop{ColorReset}");
        }

        // =====================
        // LEVEL UP SCREEN
        // =====================

        public static void RenderLevelUp(in GameState state)
        {
            Console.Clear();

            Console.WriteLine($"{ColorBold}{ColorYellow}");
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║           LEVEL UP!                    ║");
            Console.WriteLine($"║   You are now level {state.Player.GetLevel()}!              ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.WriteLine(ColorReset);

            Console.WriteLine($"\n{ColorGreen}Choose a powerup:{ColorReset}\n");

            // Generate options if not already generated
            if (state.PowerupCount == 0)
            {
                Span<PowerupType> options = stackalloc PowerupType[3];
                ProgressionLogic.GeneratePowerups(state, options);

                for (int i = 0; i < 3; i++)
                {
                    state.PowerupOptions[i] = options[i];
                }
                state.PowerupCount = 3;
            }

            // Display options
            for (int i = 0; i < state.PowerupCount; i++)
            {
                var powerup = state.PowerupOptions[i];
                Console.WriteLine($"  [{i + 1}] {ColorCyan}{ProgressionLogic.GetPowerupDescription(powerup, state.Player.GetLevel())}{ColorReset}");
            }

            Console.WriteLine($"\n{ColorYellow}[1-3] - Select powerup{ColorReset}");
        }

        // =====================
        // GAME OVER SCREEN
        // =====================

        public static void RenderGameOver(in GameState state)
        {
            Console.Clear();

            Console.WriteLine($"{ColorBold}{ColorRed}");
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║            GAME OVER                    ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.WriteLine(ColorReset);

            Console.WriteLine($"\n{ColorYellow}Statistics:{ColorReset}");
            Console.WriteLine($"  Floor Reached: {state.Floor}");
            Console.WriteLine($"  Level Achieved: {state.Player.GetLevel()}");
            Console.WriteLine($"  Enemies Killed: {state.Player.Kills}");
            Console.WriteLine($"  Gold Collected: {state.Player.Gold}");
            Console.WriteLine($"  Total Damage Dealt: {state.Player.TotalDamageDealt:F0}");
            Console.WriteLine($"  Highest Floor: {state.Player.HighestFloor}");

            Console.WriteLine($"\n{ColorGreen}Press [R] to restart or [Q] to quit{ColorReset}");
        }

        // =====================
        // VICTORY SCREEN
        // =====================

        public static void RenderVictory(in GameState state)
        {
            Console.Clear();

            Console.WriteLine($"{ColorBold}{ColorYellow}");
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║            VICTORY!                     ║");
            Console.WriteLine("║      You have conquered the void!       ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.WriteLine(ColorReset);

            Console.WriteLine($"\n{ColorYellow}Final Statistics:{ColorReset}");
            Console.WriteLine($"  Floors Cleared: {state.FloorsCleared}");
            Console.WriteLine($"  Final Level: {state.Player.GetLevel()}");
            Console.WriteLine($"  Total Kills: {state.Player.Kills}");
            Console.WriteLine($"  Total Gold: {state.Player.Gold}");
            Console.WriteLine($"  Damage Dealt: {state.Player.TotalDamageDealt:F0}");

            Console.WriteLine($"\n{ColorGreen}Congratulations, Hero!{ColorReset}");
            Console.WriteLine($"\nPress [Q] to quit");
        }

        // =====================
        // UTILITY FUNCTIONS
        // =====================

        /// <summary>
        /// Render the header with player stats.
        /// </summary>
        private static void RenderHeader(in GameState state)
        {
            Console.WriteLine($"{ColorBold}{ColorCyan}┌────────────────────────────────────────────┐{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorCyan}│ VOID CLIMBER{ColorReset} - Floor {state.Floor} - Lvl {state.Player.GetLevel()}{ColorBold}{ColorCyan} │{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorCyan}└────────────────────────────────────────────┘{ColorReset}");

            // Stats bar
            int level = state.Player.GetLevel();
            float xp = state.Player.Experience.Value.Current;
            float xpMax = state.Player.Experience.Value.Max;
            int gold = state.Player.Gold;
            int potions = state.Player.GetTotalPotions();

            Console.WriteLine($"{ColorGreen}HP:{ColorReset} {state.Player.BaseEntity.Health.Value.Current:F0}/{state.Player.BaseEntity.Health.Value.Max:F0} {GetHealthBar(state.Player.BaseEntity.Health)}");
            Console.WriteLine($"{ColorYellow}XP:{ColorReset} {xp:F0}/{xpMax:F0} {state.Player.GetXpBar()}  {ColorYellow}Gold:{ColorReset} {gold}g  {ColorMagenta}Potions:{ColorReset} {potions}");
        }

        /// <summary>
        /// Get a health bar string.
        /// </summary>
        private static string GetHealthBar(in Variable.Regen.RegenFloat health, int width = 20)
        {
            float ratio = Variable.Bounded.BoundedExtensions.GetRatio(health.Value);
            int filled = (int)(ratio * width);
            int empty = width - filled;

            string color = ratio > 0.6 ? ColorGreen : (ratio > 0.3 ? ColorYellow : ColorRed);
            return $"{color}[{new string('█', filled)}{new string('░', empty)}]{ColorReset}";
        }

        /// <summary>
        /// Render character info screen.
        /// </summary>
        public static void RenderCharacterInfo(in GameState state)
        {
            Console.Clear();

            Console.WriteLine($"{ColorBold}{ColorCyan}══════════════════════════════════════════{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorYellow}CHARACTER INFO{ColorReset}");
            Console.WriteLine($"{ColorBold}{ColorCyan}══════════════════════════════════════════{ColorReset}");

            var player = state.Player.BaseEntity;

            Console.WriteLine($"\n{ColorGreen}Level {state.Player.GetLevel()} {state.Player.BaseEntity.Name}{ColorReset}\n");

            Console.WriteLine($"{ColorCyan}Core Stats:{ColorReset}");
            Console.WriteLine($"  Health Max:     {player.Stats.Get((int)StatType.HealthMax):F1}");
            Console.WriteLine($"  Health Regen:   {player.Stats.Get((int)StatType.HealthRegen):F1}/sec");
            Console.WriteLine($"  Attack Damage:  {player.Stats.Get((int)StatType.AttackDamage):F1}");
            Console.WriteLine($"  Defense:        {player.Stats.Get((int)StatType.Defense):F1}");

            Console.WriteLine($"\n{ColorCyan}Combat Stats:{ColorReset}");
            Console.WriteLine($"  Attack Speed:   {player.Stats.Get((int)StatType.AttackSpeed):F2}x");
            Console.WriteLine($"  Crit Chance:    {player.Stats.Get((int)StatType.CritChance) * 100:F1}%");
            Console.WriteLine($"  Crit Multiplier:{player.Stats.Get((int)StatType.CritMultiplier):F2}x");
            Console.WriteLine($"  Dodge Chance:   {player.Stats.Get((int)StatType.DodgeChance) * 100:F1}%");
            Console.WriteLine($"  Lifesteal:      {player.Stats.Get((int)StatType.Lifesteal) * 100:F1}%");
            Console.WriteLine($"  Thorns:         {player.Stats.Get((int)StatType.Thorns):F1}");

            Console.WriteLine($"\n{ColorCyan}Progression:{ColorReset}");
            Console.WriteLine($"  Total Kills:    {state.Player.Kills}");
            Console.WriteLine($"  Floors Cleared: {state.Player.FloorsCleared}");

            Console.WriteLine($"\n{ColorYellow}Press any key to return...{ColorReset}");
        }
    }
}
