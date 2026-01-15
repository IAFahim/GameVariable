using System;

namespace VoidClimber.Core
{
    /// <summary>
    /// Represents all RPG stats in the game.
    /// Maps directly to RpgStatSheet indices.
    /// </summary>
    public enum StatType
    {
        HealthMax = 0,      // Maximum health pool
        HealthRegen = 1,    // Health regeneration per second
        AttackDamage = 2,   // Base attack damage
        Defense = 3,        // Damage mitigation (flat)
        AttackSpeed = 4,    // Attack speed multiplier
        CritChance = 5,     // Critical hit chance (0-1)
        CritMultiplier = 6, // Critical damage multiplier
        DodgeChance = 7,    // Dodge chance (0-1)
        Lifesteal = 8,      // Percentage of damage healed
        Thorns = 9          // Damage reflected when hit
    }

    /// <summary>
    /// Damage types for the combat system.
    /// Used in IDamageConfig for damage resolution.
    /// </summary>
    public enum DamageElement
    {
        Physical = 0,  // Mitigated by Defense (flat)
        Fire = 1,      // Magic damage (no mitigation in MVP)
        Ice = 2,       // Magic damage with slow effect
        Lightning = 3, // Magic damage with chain effect
        Poison = 4     // Damage over time
    }

    /// <summary>
    /// Main game phases for state machine.
    /// Controls what actions are available to the player.
    /// </summary>
    public enum GamePhase
    {
        Menu,          // Main menu and game over screen
        Exploration,   // Moving between rooms/floors
        Combat,        // Active combat encounter
        Shopping,      // Shop interaction
        LevelUp,       // Selecting powerups
        GameOver,      // Player death screen
        Victory        // Winning the game
    }

    /// <summary>
    /// Enemy types with different stat distributions.
    /// Used for procedural generation.
    /// </summary>
    public enum EnemyType
    {
        Goblin,        // Fast, low damage
        Orc,           // Slow, high damage
        Skeleton,      // Balanced
        Dragon,        // Boss: high everything
        VoidTerror,    // Boss: special abilities
        Slime,         // Low stats, spawns in groups
        Lich,          // Magic user
        Golem          // High defense, slow
    }

    /// <summary>
    /// Room types in the procedural dungeon.
    /// </summary>
    public enum RoomType
    {
        Encounter,     // Standard enemy fight
        Elite,         // Tough enemy with better rewards
        Boss,          // Floor guardian
        Treasure,      // Gold/items only
        Shrine,        // Buff/debuff choice
        Shop,          // Spend gold
        Rest,          // Restore health
        Mystery        // Random outcome
    }

    /// <summary>
    /// Powerup types offered during level up.
    /// </summary>
    public enum PowerupType
    {
        FlatHealth,        // +Max HP
        PercentHealth,     // +% Max HP
        FlatDamage,        // +Attack Damage
        PercentDamage,     // +% Attack Damage
        FlatDefense,       // +Defense
        FlatRegen,         // +Health Regen
        FlatSpeed,         // +Attack Speed
        PercentSpeed,      // +% Attack Speed
        FlatCritChance,    // +Crit Chance
        FlatCritMult,      // +Crit Multiplier
        Lifesteal,         // +Lifesteal
        Thorns,            // +Thorns
        DodgeChance        // +Dodge Chance
    }

    /// <summary>
    /// Shop items available for purchase.
    /// </summary>
    public enum ShopItemType
    {
        HealthPotion,      // Restore health
        DamageBoost,       // Temporary damage buff
        DefenseBoost,      // Temporary defense buff
        Key,               // Opens treasure rooms
        Map,               // Reveals floor layout
        Reroll,            // Reroll powerup choices
    }

    /// <summary>
    /// Shrine effects for player choice.
    /// </summary>
    public enum ShrineEffect
    {
        BloodAltar,       // Lose HP, gain permanent damage
        FontOfHealth,     // Gain max HP, lose damage
        CombatMastery,    // Gain crit, lose defense
        IronWill,         // Gain defense, lose speed
        VoidBargain,      // 50% chance: double power or lose half
        TimeWarp,         // Full heal, all enemies become elites
    }

    /// <summary>
    /// Input types for combo system.
    /// Maps to keyboard input in console.
    /// </summary>
    public enum ComboInput
    {
        None = 0,
        Attack = 1,       // 'A' key
        Defend = 2,       // 'D' key
        Special = 3,      // 'S' key
        Heal = 4,         // 'H' key
        Ultimate = 5      // 'U' key
    }

    /// <summary>
    /// Combo actions that can be triggered.
    /// </summary>
    public enum ComboAction
    {
        None = 0,
        // Basic attacks
        LightAttack = 10,
        HeavyAttack = 11,
        // Combo chains
        DoubleSlash = 20,
        Whirlwind = 21,
        PowerStrike = 22,
        DefensiveStance = 23,
        VampiricStrike = 24,
        UltimateBurst = 30,
        DivineShield = 31,
        Execute = 32
    }
}
