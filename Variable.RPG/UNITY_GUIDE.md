# Variable.RPG - Unity Integration Guide

**Complete Unity documentation for the Diamond Architecture RPG system.**

---

## 🎮 Quick Start (MonoBehaviour)

### Step 1: Define Your Game Stats

```csharp
// GameStats.cs
public static class GameStats
{
    // Character Stats
    public const int Health = 0;
    public const int MaxHealth = 1;
    public const int Stamina = 2;
    public const int MaxStamina = 3;
    
    // Combat Stats
    public const int Strength = 10;
    public const int Dexterity = 11;
    public const int Intelligence = 12;
    
    // Defense Stats
    public const int Armor = 20;
    public const int FireResist = 21;
    public const int IceResist = 22;
    public const int LightningResist = 23;
}

// DamageTypes.cs
public static class DamageTypes
{
    public const int Physical = 100;
    public const int Fire = 101;
    public const int Ice = 102;
    public const int Lightning = 103;
    public const int True = 104;  // Ignores mitigation
}
```

### Step 2: Create Damage Config

```csharp
// GameDamageConfig.cs
using Variable.RPG;

public struct GameDamageConfig : IDamageConfig
{
    public bool TryGetMitigationStat(int elementId, out int statId, out bool isFlat)
    {
        switch (elementId)
        {
            case DamageTypes.Physical:
                statId = GameStats.Armor;
                isFlat = true;  // Flat armor reduction
                return true;
            
            case DamageTypes.Fire:
                statId = GameStats.FireResist;
                isFlat = false; // Percentage resistance
                return true;
            
            case DamageTypes.Ice:
                statId = GameStats.IceResist;
                isFlat = false;
                return true;
            
            case DamageTypes.Lightning:
                statId = GameStats.LightningResist;
                isFlat = false;
                return true;
            
            case DamageTypes.True:
                // True damage ignores mitigation
                statId = -1;
                isFlat = false;
                return false;
            
            default:
                statId = -1;
                isFlat = false;
                return false;
        }
    }
}
```

### Step 3: Create Character Component

```csharp
// Character.cs
using UnityEngine;
using Variable.RPG;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private AttributeSheet _stats;
    
    private GameDamageConfig _damageConfig;

    private void Awake()
    {
        // Initialize stats (50 total stats for flexibility)
        _stats = new AttributeSheet(50);
        _damageConfig = new GameDamageConfig();
        
        InitializeBaseStats();
    }

    private void InitializeBaseStats()
    {
        // Health
        _stats.SetBase(GameStats.MaxHealth, 100f);
        _stats.SetBase(GameStats.Health, 100f);
        
        // Combat stats
        _stats.SetBase(GameStats.Strength, 10f);
        _stats.SetBase(GameStats.Dexterity, 10f);
        _stats.SetBase(GameStats.Intelligence, 10f);
        
        // Defense stats
        _stats.SetBase(GameStats.Armor, 5f);
        _stats.SetBase(GameStats.FireResist, 0.1f);    // 10% resist
        _stats.SetBase(GameStats.IceResist, 0.1f);
        _stats.SetBase(GameStats.LightningResist, 0.1f);
    }

    public float GetStat(int statId)
    {
        return _stats.Get(statId);
    }

    public void AddModifier(int statId, float flat, float percent)
    {
        if (statId < 0 || statId >= _stats.Attributes.Length) return;
        AttributeLogic.AddModifier(ref _stats.Attributes[statId], flat, percent);
    }

    public void TakeDamage(DamagePacket[] damages)
    {
        var finalDamage = DamageLogic.ResolveDamage(
            _stats.AsSpan(), 
            damages, 
            _damageConfig);
        
        ApplyDamage(finalDamage);
    }

    private void ApplyDamage(float amount)
    {
        var currentHealth = _stats.Attributes[GameStats.Health].Base;
        currentHealth -= amount;
        
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            OnDeath();
        }
        
        _stats.SetBase(GameStats.Health, currentHealth);
        
        Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}");
    }

    private void OnDeath()
    {
        Debug.Log($"{gameObject.name} died!");
        // Death logic here
    }

    // Debug display
    private void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        var y = 10f;
        GUI.Label(new Rect(10, y, 200, 20), $"Health: {GetStat(GameStats.Health):F0}/{GetStat(GameStats.MaxHealth):F0}");
        y += 20;
        GUI.Label(new Rect(10, y, 200, 20), $"Armor: {GetStat(GameStats.Armor):F1}");
        y += 20;
        GUI.Label(new Rect(10, y, 200, 20), $"Fire Resist: {GetStat(GameStats.FireResist) * 100:F0}%");
    }
}
```

### Step 4: Create Weapon/Attack System

```csharp
// Weapon.cs
using UnityEngine;
using Variable.RPG;

public class Weapon : MonoBehaviour
{
    [Header("Damage Configuration")]
    [SerializeField] private DamageData[] damageTypes;
    
    [System.Serializable]
    public struct DamageData
    {
        public int ElementId;
        public float BaseDamage;
    }

    public void Attack(Character target)
    {
        // Build damage packets
        var packets = new DamagePacket[damageTypes.Length];
        
        for (int i = 0; i < damageTypes.Length; i++)
        {
            packets[i] = new DamagePacket
            {
                ElementId = damageTypes[i].ElementId,
                Amount = damageTypes[i].BaseDamage,
                Flags = CheckCritical() ? DamageFlags.Critical : DamageFlags.None
            };
        }
        
        // Apply damage
        target.TakeDamage(packets);
    }

    private bool CheckCritical()
    {
        return Random.value < 0.1f; // 10% crit chance
    }
}
```

---

## 🔥 Equipment System

### Item with Stat Modifiers

```csharp
// EquipmentItem.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Equipment Item")]
public class EquipmentItem : ScriptableObject
{
    [System.Serializable]
    public struct StatModifier
    {
        public int StatId;
        public float FlatBonus;
        public float PercentBonus;
    }

    public string ItemName;
    public StatModifier[] Modifiers;
}

// EquipmentManager.cs
using UnityEngine;
using Variable.RPG;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Character character;
    private EquipmentItem[] equippedItems = new EquipmentItem[10];

    public void Equip(EquipmentItem item, int slot)
    {
        // Unequip old item
        if (equippedItems[slot] != null)
            Unequip(slot);
        
        // Equip new item
        equippedItems[slot] = item;
        ApplyModifiers(item);
    }

    public void Unequip(int slot)
    {
        if (equippedItems[slot] == null) return;
        
        RemoveModifiers(equippedItems[slot]);
        equippedItems[slot] = null;
    }

    private void ApplyModifiers(EquipmentItem item)
    {
        foreach (var mod in item.Modifiers)
        {
            character.AddModifier(mod.StatId, mod.FlatBonus, mod.PercentBonus);
        }
    }

    private void RemoveModifiers(EquipmentItem item)
    {
        foreach (var mod in item.Modifiers)
        {
            character.AddModifier(mod.StatId, -mod.FlatBonus, -mod.PercentBonus);
        }
    }
}
```

---

## 🎯 Buff/Debuff System

```csharp
// Buff.cs
using UnityEngine;
using Variable.RPG;
using System.Collections;

public class Buff : MonoBehaviour
{
    [System.Serializable]
    public class BuffData
    {
        public int StatId;
        public float FlatBonus;
        public float PercentBonus;
        public float Duration;
    }

    public void ApplyBuff(Character target, BuffData data)
    {
        StartCoroutine(BuffCoroutine(target, data));
    }

    private IEnumerator BuffCoroutine(Character target, BuffData data)
    {
        // Apply buff
        target.AddModifier(data.StatId, data.FlatBonus, data.PercentBonus);
        
        Debug.Log($"Buff applied: +{data.FlatBonus} flat, +{data.PercentBonus * 100}% for {data.Duration}s");
        
        // Wait duration
        yield return new WaitForSeconds(data.Duration);
        
        // Remove buff
        target.AddModifier(data.StatId, -data.FlatBonus, -data.PercentBonus);
        
        Debug.Log("Buff expired");
    }
}

// Example usage
public class BuffTest : MonoBehaviour
{
    public void ApplyStrengthBuff(Character character)
    {
        var buffData = new Buff.BuffData
        {
            StatId = GameStats.Strength,
            FlatBonus = 5f,      // +5 strength
            PercentBonus = 0.2f, // +20% strength
            Duration = 10f       // 10 seconds
        };
        
        GetComponent<Buff>().ApplyBuff(character, buffData);
    }
}
```

---

## ⚔️ Combat Example (Player vs Enemy)

```csharp
// CombatExample.cs
using UnityEngine;
using Variable.RPG;

public class CombatExample : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private Character enemy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayerAttack();
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlayerFireball();
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EnemyAttack();
    }

    private void PlayerAttack()
    {
        // Physical attack based on strength
        var strength = player.GetStat(GameStats.Strength);
        
        var damages = new DamagePacket[]
        {
            new DamagePacket
            {
                ElementId = DamageTypes.Physical,
                Amount = 10f + strength * 2f,
                Flags = DamageFlags.None
            }
        };
        
        enemy.TakeDamage(damages);
    }

    private void PlayerFireball()
    {
        // Magical attack based on intelligence
        var intelligence = player.GetStat(GameStats.Intelligence);
        
        var damages = new DamagePacket[]
        {
            new DamagePacket
            {
                ElementId = DamageTypes.Fire,
                Amount = 30f + intelligence * 3f,
                Flags = DamageFlags.CanDodge
            }
        };
        
        enemy.TakeDamage(damages);
    }

    private void EnemyAttack()
    {
        // Multi-element attack (grenade-style)
        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = DamageTypes.Physical, Amount = 15f },
            new DamagePacket { ElementId = DamageTypes.Fire, Amount = 20f },
            new DamagePacket { ElementId = DamageTypes.Lightning, Amount = 10f }
        };
        
        player.TakeDamage(damages);
    }
}
```

---

## 🚀 Unity DOTS (ECS)

### Components

```csharp
using Unity.Entities;
using Variable.RPG;

// Character stats component
public struct CharacterStatsComponent : IComponentData
{
    public BlobAssetReference<AttributeSheetBlob> Stats;
}

// Blob asset for ECS
public struct AttributeSheetBlob
{
    public BlobArray<Attribute> Attributes;
}

// Damage event
public struct TakeDamageEvent : IComponentData
{
    public int DamageCount;
    public BlobAssetReference<DamageArrayBlob> Damages;
}

public struct DamageArrayBlob
{
    public BlobArray<DamagePacket> Packets;
}
```

### Damage Processing System

```csharp
using Unity.Entities;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class DamageProcessingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var config = new GameDamageConfig();

        Entities
            .WithoutBurst()  // Span not yet Burst-compatible
            .ForEach((Entity entity, 
                     ref CharacterStatsComponent stats, 
                     in TakeDamageEvent damageEvent) =>
            {
                unsafe
                {
                    ref var statsBlob = ref stats.Stats.Value;
                    ref var damagesBlob = ref damageEvent.Damages.Value;
                    
                    // Convert BlobArray to Span
                    var statsSpan = new Span<Attribute>(
                        statsBlob.Attributes.GetUnsafePtr(),
                        statsBlob.Attributes.Length);
                    
                    var damagesSpan = new ReadOnlySpan<DamagePacket>(
                        damagesBlob.Packets.GetUnsafePtr(),
                        damagesBlob.Packets.Length);
                    
                    // Calculate damage
                    var finalDamage = DamageLogic.ResolveDamage(
                        statsSpan, 
                        damagesSpan, 
                        config);
                    
                    // Apply damage
                    var currentHealth = statsSpan[GameStats.Health].Base;
                    currentHealth -= finalDamage;
                    statsSpan[GameStats.Health].Base = currentHealth;
                    
                    // Remove damage event
                    ecb.RemoveComponent<TakeDamageEvent>(entity);
                    
                    if (currentHealth <= 0f)
                    {
                        ecb.AddComponent<DeadTag>(entity);
                    }
                }
            })
            .Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}

public struct DeadTag : IComponentData { }
```

---

## 🎨 UI Integration

```csharp
// StatDisplay.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI strengthText;

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var health = character.GetStat(GameStats.Health);
        var maxHealth = character.GetStat(GameStats.MaxHealth);
        var armor = character.GetStat(GameStats.Armor);
        var strength = character.GetStat(GameStats.Strength);
        
        healthText.text = $"{health:F0} / {maxHealth:F0}";
        healthBar.value = health / maxHealth;
        armorText.text = $"Armor: {armor:F1}";
        strengthText.text = $"STR: {strength:F0}";
    }
}
```

---

## 🛠️ Unity Editor Tools

### Custom Inspector for Character

```csharp
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var character = (Character)target;
        
        if (!Application.isPlaying) return;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime Stats", EditorStyles.boldLabel);
        
        EditorGUILayout.LabelField($"Health: {character.GetStat(GameStats.Health):F1}");
        EditorGUILayout.LabelField($"Armor: {character.GetStat(GameStats.Armor):F1}");
        EditorGUILayout.LabelField($"Strength: {character.GetStat(GameStats.Strength):F1}");
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Deal 20 Physical Damage"))
        {
            character.TakeDamage(new[] {
                new Variable.RPG.DamagePacket { 
                    ElementId = DamageTypes.Physical, 
                    Amount = 20f 
                }
            });
        }
        
        if (GUILayout.Button("Heal to Full"))
        {
            character.AddModifier(GameStats.Health, 1000f, 0f);
        }
    }
}
#endif
```

---

## 📊 Performance Tips

### 1. Cache AttributeSheet

```csharp
// Bad - creates new sheet every frame
void Update()
{
    var sheet = new AttributeSheet(50);  // ALLOCATION!
}

// Good - cache it
private AttributeSheet _stats;

void Awake()
{
    _stats = new AttributeSheet(50);  // Once
}
```

### 2. Use struct Config

```csharp
// Good - struct config (zero boxing)
private GameDamageConfig _config;  // Stack only

void TakeDamage(DamagePacket[] damages)
{
    DamageLogic.ResolveDamage(stats, damages, _config);  // No allocation
}
```

### 3. Batch Damage Packets

```csharp
// Bad - multiple calls
DamageLogic.ResolveDamage(stats, new[] { dmg1 }, config);
DamageLogic.ResolveDamage(stats, new[] { dmg2 }, config);

// Good - single call
var damages = new[] { dmg1, dmg2 };
DamageLogic.ResolveDamage(stats, damages, config);
```

---

## 🎯 Common Patterns

### Level-Up System

```csharp
public void LevelUp()
{
    // Increase base stats
    AddModifier(GameStats.MaxHealth, 10f, 0.05f);  // +10, +5%
    AddModifier(GameStats.Strength, 2f, 0f);
    AddModifier(GameStats.Dexterity, 2f, 0f);
    
    // Heal to full
    var maxHealth = GetStat(GameStats.MaxHealth);
    _stats.SetBase(GameStats.Health, maxHealth);
}
```

### Potion System

```csharp
public void DrinkHealthPotion()
{
    var currentHealth = GetStat(GameStats.Health);
    var maxHealth = GetStat(GameStats.MaxHealth);
    
    var healAmount = maxHealth * 0.5f;  // 50% heal
    var newHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    
    _stats.SetBase(GameStats.Health, newHealth);
}
```

---

## 📚 See Also

- **Main README** - Architecture & API reference
- **Tests** - Variable.RPG.Tests (14 examples)

---

**Unity-Ready. MonoBehaviour-Friendly. ECS-Compatible.** 🎮
