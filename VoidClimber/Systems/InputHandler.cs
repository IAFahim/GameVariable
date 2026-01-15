using System;
using VoidClimber.Core;
using Variable.Input;

namespace VoidClimber.Systems
{
    /// <summary>
    /// Input handling system with combo support using Variable.Input.
    /// Maps keyboard input to game actions and manages combo detection.
    /// </summary>
    public static class InputHandler
    {
        // =====================
        // INPUT MAPPING
        // =====================

        /// <summary>
        /// Map console key to combo input type.
        /// </summary>
        public static ComboInput MapKeyToInput(ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Key switch
            {
                ConsoleKey.A => ComboInput.Attack,
                ConsoleKey.D => ComboInput.Defend,
                ConsoleKey.S => ComboInput.Special,
                ConsoleKey.H => ComboInput.Heal,
                ConsoleKey.U => ComboInput.Ultimate,
                _ => ComboInput.None
            };
        }

        // =====================
        // INPUT PROCESSING
        // =====================

        /// <summary>
        /// Process input based on current game phase.
        /// </summary>
        public static void ProcessInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            switch (state.Phase)
            {
                case GamePhase.Menu:
                    ProcessMenuInput(ref state, keyInfo);
                    break;

                case GamePhase.Exploration:
                    ProcessExplorationInput(ref state, keyInfo);
                    break;

                case GamePhase.Combat:
                    ProcessCombatInput(ref state, keyInfo);
                    break;

                case GamePhase.Shopping:
                    ProcessShopInput(ref state, keyInfo);
                    break;

                case GamePhase.LevelUp:
                    ProcessLevelUpInput(ref state, keyInfo);
                    break;

                case GamePhase.GameOver:
                    ProcessGameOverInput(ref state, keyInfo);
                    break;

                case GamePhase.Victory:
                    ProcessVictoryInput(ref state, keyInfo);
                    break;
            }
        }

        // =====================
        // PHASE-SPECIFIC INPUT
        // =====================

        private static void ProcessMenuInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            // Any key starts the game
            state.Phase = GamePhase.Exploration;
            state.Floor = 1;
            state.Room = 0;
        }

        private static void ProcessExplorationInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    GameLoop.AdvanceRoom(ref state);
                    break;

                case ConsoleKey.P:
                    if (GameLoop.HandlePlayerPotion(ref state))
                    {
                        Console.WriteLine("You drink a potion and restore 25 HP!");
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        Console.WriteLine("No potions available!");
                        System.Threading.Thread.Sleep(500);
                    }
                    GameRenderer.Render(state);
                    break;

                case ConsoleKey.C:
                    GameRenderer.RenderCharacterInfo(state);
                    Console.ReadKey();
                    GameRenderer.Render(state);
                    break;

                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }

        private static void ProcessCombatInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            // Map input to combo system
            var input = MapKeyToInput(keyInfo);

            if (input != ComboInput.None)
            {
                // Add to input buffer
                state.InputBuffer.Add((int)input);

                // Check for combos (simplified for MVP - just trigger actions)
                if (input == ComboInput.Attack)
                {
                    if (GameLoop.HandlePlayerAttack(ref state))
                    {
                        // Successful attack
                        state.ComboCount++;
                    }
                }
                else if (input == ComboInput.Special)
                {
                    if (GameLoop.HandlePlayerSpecial(ref state))
                    {
                        // Successful special
                        state.ComboCount++;
                    }
                }
                else if (input == ComboInput.Heal)
                {
                    GameLoop.HandlePlayerPotion(ref state);
                }
                else if (keyInfo.Key == ConsoleKey.F)
                {
                    if (GameLoop.HandlePlayerFlee(ref state))
                    {
                        Console.WriteLine("You escaped successfully!");
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        Console.WriteLine("Failed to escape!");
                        System.Threading.Thread.Sleep(500);
                    }
                }

                GameRenderer.Render(state);
            }
        }

        private static void ProcessShopInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                // Leave shop
                state.Phase = GamePhase.Exploration;
                state.ShopVisits++;
                state.Player.ShopsVisited++;
            }
            else if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D3)
            {
                // Try to buy item
                int itemIndex = keyInfo.Key - ConsoleKey.D1;
                var inventory = ShopLogic.GenerateShopInventory(state);

                if (itemIndex < inventory.Length)
                {
                    var item = inventory[itemIndex];
                    if (ShopLogic.TryPurchase(ref state.Player, item, state.Floor))
                    {
                        Console.WriteLine($"\nPurchased {ShopLogic.GetItemName(item)}!");
                        System.Threading.Thread.Sleep(500);
                        GameRenderer.Render(state);
                    }
                    else
                    {
                        Console.WriteLine($"\nNot enough gold or inventory full!");
                        System.Threading.Thread.Sleep(500);
                        GameRenderer.Render(state);
                    }
                }
            }
        }

        private static void ProcessLevelUpInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D3)
            {
                int selection = keyInfo.Key - ConsoleKey.D1;

                if (selection < state.PowerupCount)
                {
                    // Apply powerup
                    ProgressionLogic.ApplyPowerup(ref state.Player, state.PowerupOptions[selection]);

                    Console.WriteLine($"\nPowerup applied: {ProgressionLogic.GetPowerupDescription(state.PowerupOptions[selection], state.Player.GetLevel())}");
                    System.Threading.Thread.Sleep(1000);

                    // Clear powerups and return to exploration
                    state.PowerupCount = 0;
                    state.Phase = GamePhase.Exploration;
                    GameRenderer.Render(state);
                }
            }
        }

        private static void ProcessGameOverInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.R)
            {
                // Restart game
                GameLoop.ResetGame(ref state, Environment.TickCount);
            }
            else if (keyInfo.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
        }

        private static void ProcessVictoryInput(ref GameState state, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
        }

        // =====================
        // COMBO SYSTEM (Advanced)
        // =====================

        /// <summary>
        /// Setup combo graph for advanced input detection.
        /// This is a placeholder for future expansion.
        /// </summary>
        public static void SetupCombos()
        {
            // In a full implementation, this would set up the ComboGraph
            // Example:
            // var builder = new ComboGraphBuilder();
            // int nodeIdle = builder.AddNode((int)ComboAction.None);
            // int nodeLight = builder.AddNode((int)ComboAction.LightAttack);
            // int nodeHeavy = builder.AddNode((int)ComboAction.HeavyAttack);
            //
            // builder.AddEdge(nodeIdle, nodeLight, (int)ComboInput.Attack);
            // builder.AddEdge(nodeLight, nodeHeavy, (int)ComboInput.Attack);
            //
            // var graph = builder.Build();
        }

        /// <summary>
        /// Check input buffer for combo patterns.
        /// Simplified version for MVP.
        /// </summary>
        public static ComboAction CheckCombos(in GameState state)
        {
            // In a full implementation, this would use ComboGraph.CheckInput
            // For MVP, we just return the last action
            return state.LastCombo;
        }

        // =====================
        // UTILITY FUNCTIONS
        // =====================

        /// <summary>
        /// Get help text for current phase.
        /// </summary>
        public static string GetHelpText(GamePhase phase)
        {
            return phase switch
            {
                GamePhase.Menu => "Press any key to start",
                GamePhase.Exploration => "[ENTER] Continue  [P] Potion  [C] Character  [Q] Quit",
                GamePhase.Combat => "[A] Attack  [S] Special  [P] Potion  [F] Flee",
                GamePhase.Shopping => "[1-3] Buy item  [ENTER] Leave",
                GamePhase.LevelUp => "[1-3] Select powerup",
                GamePhase.GameOver => "[R] Restart  [Q] Quit",
                GamePhase.Victory => "[Q] Quit",
                _ => ""
            };
        }
    }
}
