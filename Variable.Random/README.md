# Variable.Random

A **deterministic**, **zero-allocation** Random Number Generator (RNG) for C# game development.

> **"God does not play dice with the universe... but we do, deterministically."**

## Mental Model
Think of `RandomState` as a specific **deck of cards**.
*   **Initialize**: You shuffle the deck using a "seed" (a number). If you use the same seed, the deck is always shuffled exactly the same way.
*   **Next...**: You draw the next card. Since the deck order is fixed by the seed, you always get the same sequence of cards.
*   **Zero-Allocation**: We don't buy a new deck every time. We just keep passing the same deck (`ref RandomState`) around.

## ELI5
Standard `System.Random` creates "garbage" (memory) that the computer has to clean up later, which can slow down games (lag spikes). It also behaves differently on different computers sometimes.
`Variable.Random` is:
1.  **Fast**: Uses the Xoshiro128** algorithm (super fast).
2.  **Clean**: No garbage memory.
3.  **Reliable**: Same seed = Same result, everywhere.

## Why do I need this?
*   **Replays**: If you save the seed, you can replay the entire game session exactly as it happened.
*   **Procedural Generation**: Generate the same world map for every player who uses the seed "CoolWorld123".
*   **Performance**: Don't waste CPU time cleaning up memory.

## Usage

### 1. Define State
Store the state in your data struct.
```csharp
public struct GameState
{
    public RandomState Rng;
}
```

### 2. Initialize
Initialize it once.
```csharp
RandomLogic.Initialize(ref gameState.Rng, 12345);
```

### 3. Generate Numbers
Pass it by `ref` to modify the state (draw the card).
```csharp
// Get a float between 0.0 and 1.0
RandomLogic.NextFloat(ref gameState.Rng, out float randomVal);

// Get an int between 0 and 100
RandomLogic.NextInt(ref gameState.Rng, 0, 100, out int randomInt);

// Get a boolean (coin flip)
RandomLogic.NextBool(ref gameState.Rng, out bool isHeads);
```

## Installation
Reference the `Variable.Random` project or package.
