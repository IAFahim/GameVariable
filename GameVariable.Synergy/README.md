<div align="center">

# 🎻 GameVariable.Synergy

### The Ultimate Team Player

**The Orchestra for your GameVariable Instruments.** 🎶
Harmonizes Health, Mana, Cooldowns, and XP into production-ready systems.

[Mental Model](#-mental-model) •
[ELI5](#-eli5) •
[Installation](#-installation) •
[Usage](#-usage)

</div>

---

## 🧠 Mental Model

Think of **GameVariable.Synergy** as **The Orchestra**.

*   `Variable.Bounded` is a Violin.
*   `Variable.Timer` is a Drum.
*   `Variable.Core` is the Sheet Music.

Individually, they make sounds. Together, coordinated by **Synergy**, they make music (Gameplay).

---

## 👶 ELI5

Imagine you have a **Health Bar**, a **Magic Bar**, and an **Ability Timer**.
Instead of writing code to glue them together manually every time ("Check mana... if enough, cast... subtract mana... reset timer..."), you use a **Synergy** kit that comes pre-glued. It's like buying a fully assembled LEGO castle instead of just the bricks.

---

## 📦 Installation

```bash
dotnet add package GameVariable.Synergy
```

## 🚀 Usage

### The Synergy Character

```csharp
// 1. Create the character state
SynergyCharacter hero = new SynergyCharacter(
    maxHealth: 100f,
    maxMana: 50f,
    manaRegen: 5f,
    attackCooldown: 1.5f
);

// 2. Update loop (e.g. Unity Update)
public void Update() {
    hero.Update(Time.deltaTime);
}

// 3. Try to attack
public void OnAttackButton() {
    if (hero.TryCastAbility(cost: 10f)) {
        Console.WriteLine("Fireball! 🔥");
    } else {
        Console.WriteLine("Not ready or not enough mana! ⏳");
    }
}
```
