using System;
using System.Diagnostics;
using VoidClimber.Core;
using VoidClimber.Systems;

namespace VoidClimber
{
    /// <summary>
    /// Main program entry point for VoidClimber.
    /// A comprehensive demo of the GameVariable library ecosystem.
    ///
    /// This game demonstrates:
    /// - Variable.RPG: Stats, damage calculation, modifiers
    /// - Variable.Regen: Health regeneration system
    /// - Variable.Timer: Attack and ability cooldowns
    /// - Variable.Experience: Leveling and progression
    /// - Variable.Reservoir: Potion management (belt + reserve)
    /// - Variable.Inventory: Shop transactions and inventory logic
    /// - Variable.Input: Combo system (framework ready for expansion)
    ///
    /// Architecture: Data-Logic-Extension triad
    /// - Data: Entity, PlayerData, GameState structs
    /// - Logic: CombatLogic, ProgressionLogic, ShopLogic, EnemyGeneration
    /// - Extension: GameLoop, GameRenderer, InputHandler
    /// </summary>
    class Program
    {
        private static GameState _state;
        private static readonly Stopwatch _gameTimer = new();
        private static DateTime _lastTick = DateTime.Now;

        static void Main(string[] args)
        {
            // Initialize console
            Console.Title = "Void Climber - GameVariable Demo";
            Console.CursorVisible = false;

            // Setup input for combo system
            InputHandler.SetupCombos();

            // Initialize game
            InitializeGame();

            // Main game loop
            RunGameLoop();

            // Cleanup
            Cleanup();
        }

        /// <summary>
        /// Initialize the game state.
        /// </summary>
        private static void InitializeGame()
        {
            // Create game state with random seed
            _state = new GameState(Environment.TickCount);

            // Set initial phase
            _state.Phase = GamePhase.Menu;

            // Update shop prices
            ShopLogic.UpdatePrices(ref _state);

            // Start game timer
            _gameTimer.Restart();
            _lastTick = DateTime.Now;
        }

        /// <summary>
        /// Main game loop.
        /// Uses tick-based simulation for real-time systems.
        /// </summary>
        private static void RunGameLoop()
        {
            bool running = true;

            while (running)
            {
                // Calculate delta time
                DateTime now = DateTime.Now;
                float deltaTime = (float)(now - _lastTick).TotalSeconds;
                _lastTick = now;

                // Tick game systems (Regen, Cooldowns, Combat)
                GameLoop.Tick(ref _state, deltaTime);

                // Render current state
                GameRenderer.Render(_state);

                // Process input
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    InputHandler.ProcessInput(ref _state, keyInfo);
                }

                // Check for victory/defeat
                if (_state.Phase == GamePhase.GameOver || _state.Phase == GamePhase.Victory)
                {
                    // Still allow input in these phases
                }

                // Small sleep to prevent CPU spinning
                System.Threading.Thread.Sleep(16);  // ~60 FPS

                // Check if we should exit
                if (_state.Phase == GamePhase.Victory)
                {
                    // Victory screen allows quit
                }
            }
        }

        /// <summary>
        /// Cleanup resources.
        /// </summary>
        private static void Cleanup()
        {
            // Dispose of unmanaged resources (RpgStatSheet)
            _state.Player.Dispose();

            if (_state.CurrentEnemy.Stats.Ptr != null)
            {
                _state.CurrentEnemy.Dispose();
            }

            // Restore console
            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        }
    }
}
