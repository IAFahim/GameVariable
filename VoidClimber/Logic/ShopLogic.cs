using System;
using VoidClimber.Core;

namespace VoidClimber.Logic
{
    /// <summary>
    /// Shop and economy system using Variable.Inventory logic.
    /// Handles transactions, inventory checks, and pricing.
    /// </summary>
    public static class ShopLogic
    {
        // =====================
        // TRANSACTION SYSTEM
        // =====================

        /// <summary>
        /// Attempt to purchase an item from the shop.
        /// Uses Variable.Inventory.InventoryLogic for validation.
        /// </summary>
        /// <returns>True if purchase was successful</returns>
        public static bool TryPurchase(ref PlayerData player, ShopItemType item, int floor)
        {
            int cost = GetItemCost(item, floor);

            // Check if player has enough gold
            if (!Variable.Inventory.InventoryLogic.HasEnough(player.Gold, cost))
            {
                return false;
            }

            // Attempt to add item based on type
            bool canAdd = item switch
            {
                ShopItemType.HealthPotion => CanAddPotion(ref player),
                ShopItemType.Key => true,  // Keys don't take inventory space
                ShopItemType.DamageBoost => true,  // Temporary buff
                ShopItemType.DefenseBoost => true,
                ShopItemType.Map => true,
                ShopItemType.Reroll => true,
                _ => false
            };

            if (!canAdd) return false;

            // Deduct gold
            player.Gold -= cost;

            // Add item
            switch (item)
            {
                case ShopItemType.HealthPotion:
                    AddPotionToReserve(ref player);
                    break;

                case ShopItemType.Key:
                    player.AddKey();
                    break;

                case ShopItemType.DamageBoost:
                    // Temporary buff for current floor
                    ref var damage = ref player.BaseEntity.Stats.GetRef((int)StatType.AttackDamage);
                    RpgStatModifier modifier = RpgStatModifier.AddFlat(RpgStatField.ModAdd, 10f);
                    RpgStatExtensions.ApplyModifier(ref damage, modifier);
                    break;

                case ShopItemType.DefenseBoost:
                    // Temporary buff for current floor
                    ref var defense = ref player.BaseEntity.Stats.GetRef((int)StatType.Defense);
                    RpgStatModifier defModifier = RpgStatModifier.AddFlat(RpgStatField.ModAdd, 5f);
                    RpgStatExtensions.ApplyModifier(ref defense, defModifier);
                    break;

                case ShopItemType.Map:
                    // Reveal floor layout (in a full implementation)
                    break;

                case ShopItemType.Reroll:
                    // Allow rerolling powerup choices (in a full implementation)
                    break;
            }

            return true;
        }

        /// <summary>
        /// Check if player can add more potions.
        /// </summary>
        private static bool CanAddPotion(ref PlayerData player)
        {
            // Check reserve capacity
            int maxReserve = 10;
            return Variable.Inventory.InventoryLogic.CanAccept(
                player.Potions.Reserve,
                maxReserve,
                1);
        }

        /// <summary>
        /// Add potion to reserve.
        /// </summary>
        private static void AddPotionToReserve(ref PlayerData player)
        {
            int current = player.Potions.Reserve;
            int maxReserve = 10;

            Variable.Inventory.InventoryLogic.AddPartial(
                ref current,
                1,
                maxReserve,
                out int _);

            player.Potions.Reserve = current;
        }

        // =====================
        // PRICING SYSTEM
        // =====================

        /// <summary>
        /// Get the cost of an item based on floor.
        /// Prices increase as you progress.
        /// </summary>
        public static int GetItemCost(ShopItemType item, int floor)
        {
            int baseCost = item switch
            {
                ShopItemType.HealthPotion => 50,
                ShopItemType.Key => 100,
                ShopItemType.DamageBoost => 75,
                ShopItemType.DefenseBoost => 75,
                ShopItemType.Map => 30,
                ShopItemType.Reroll => 25,
                _ => 0
            };

            // Price scales with floor
            return (int)(baseCost * (1.0f + (floor - 1) * 0.1f));
        }

        /// <summary>
        /// Get selling price for an item (50% of purchase cost).
        /// </summary>
        public static int GetSellPrice(ShopItemType item, int floor)
        {
            return GetItemCost(item, floor) / 2;
        }

        /// <summary>
        /// Update shop prices for the current floor.
        /// </summary>
        public static void UpdatePrices(ref GameState state)
        {
            state.PotionCost = GetItemCost(ShopItemType.HealthPotion, state.Floor);
            state.KeyCost = GetItemCost(ShopItemType.Key, state.Floor);
        }

        // =====================
        // SHOP GENERATION
        // =====================

        /// <summary>
        /// Generate shop inventory for this floor.
        /// Returns array of available items.
        /// </summary>
        public static ShopItemType[] GenerateShopInventory(in GameState state)
        {
            var rng = state.GetRng();
            int itemCount = 3 + state.Floor / 3;  // More items on higher floors

            var inventory = new ShopItemType[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                inventory[i] = rng.Next(0, 100) switch
                {
                    < 40 => ShopItemType.HealthPotion,  // Always available
                    < 60 => ShopItemType.Key,
                    < 75 => ShopItemType.DamageBoost,
                    < 90 => ShopItemType.DefenseBoost,
                    < 97 => ShopItemType.Map,
                    _ => ShopItemType.Reroll
                };
            }

            return inventory;
        }

        /// <summary>
        /// Get display name for shop item.
        /// </summary>
        public static string GetItemName(ShopItemType item)
        {
            return item switch
            {
                ShopItemType.HealthPotion => "Health Potion",
                ShopItemType.Key => "Treasure Key",
                ShopItemType.DamageBoost => "Temporary Damage Buff",
                ShopItemType.DefenseBoost => "Temporary Defense Buff",
                ShopItemType.Map => "Floor Map",
                ShopItemType.Reroll => "Powerup Reroll",
                _ => "Unknown Item"
            };
        }

        /// <summary>
        /// Get description for shop item.
        /// </summary>
        public static string GetItemDescription(ShopItemType item)
        {
            return item switch
            {
                ShopItemType.HealthPotion => "Restores 25 HP when used",
                ShopItemType.Key => "Opens locked treasure rooms",
                ShopItemType.DamageBoost => "+10 Damage for this floor",
                ShopItemType.DefenseBoost => "+5 Defense for this floor",
                ShopItemType.Map => "Reveals all rooms on this floor",
                ShopItemType.Reroll => "Reroll level-up powerup choices",
                _ => "No description"
            };
        }

        // =====================
        // TRANSACTIONS
        // =====================

        /// <summary>
        /// Sell an item from player inventory.
        /// </summary>
        public static bool TrySell(ref PlayerData player, ShopItemType item, int floor)
        {
            // Check if player has the item
            bool hasItem = item switch
            {
                ShopItemType.HealthPotion => player.Potions.Volume > 0 || player.Potions.Reserve > 0,
                ShopItemType.Key => player.Keys > 0,
                _ => false
            };

            if (!hasItem) return false;

            // Remove item and add gold
            int sellPrice = GetSellPrice(item, floor);

            switch (item)
            {
                case ShopItemType.HealthPotion:
                    if (player.Potions.Volume > 0)
                    {
                        player.Potions.Volume--;
                    }
                    else if (player.Potions.Reserve > 0)
                    {
                        player.Potions.Reserve--;
                    }
                    break;

                case ShopItemType.Key:
                    player.Keys--;
                    break;
            }

            player.AddGold(sellPrice);
            return true;
        }

        /// <summary>
        /// Get shop discount based on player stats/shrines visited.
        /// </summary>
        public static float GetShopDiscount(in PlayerData player)
        {
            float discount = 0f;

            // Discount based on shrines visited (just for fun)
            if (player.ShrinesVisited > 0)
            {
                discount += player.ShrinesVisited * 0.05f;  // 5% per shrine
            }

            // Cap at 50%
            return Math.Min(discount, 0.5f);
        }

        /// <summary>
        /// Apply discount to cost.
        /// </summary>
        public static int ApplyDiscount(int baseCost, float discount)
        {
            return (int)(baseCost * (1.0f - discount));
        }
    }
}
