# 🎲 Variable.Random

### Deterministic Randomness for Games

**The "Dice" of your game.**
Stop using `System.Random` and creating garbage.

## 🧠 Mental Model

Think of `RandomState` as a **Deck of Cards**.
*   It has a specific order (determined by the seed).
*   Every time you draw a card (`Next`), the deck changes state.
*   If you shuffle (Seed) the same way, you get the same cards.

## 👶 ELI5

Computer randomness is just a long list of numbers.
`Variable.Random` remembers where you are in the list.
It doesn't make "new" lists (garbage); it just points to the next number.

## 🚀 Installation

```bash
dotnet add package Variable.Random
```

## 🛠️ Usage

```csharp
// 1. Create a state (seeded)
var rng = new RandomState();
rng.Init(12345);

// 2. Get numbers
rng.NextInt(out int damage);
rng.Range(10, 20, out int gold);
rng.Chance(0.5f, out bool isCrit);

// 3. Pass it around (ref)
GenerateLevel(ref rng);
```
