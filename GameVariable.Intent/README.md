# GameVariable.Intent

**GameVariable.Intent** provides a high-performance, zero-allocation Hierarchical State Machine (HSM) implementation. It is designed for AI decision-making, character logic, and complex state management in games.

## Installation

```bash
dotnet add package GameVariable.Intent
```

## Features

* **IntentState**: A struct-based state machine optimized for performance.
* **Zero Allocation**: No garbage collection overhead during state transitions.
* **Event-Driven**: Dispatch events to trigger transitions.
* **IIntent Interface**: Standard interface for integration.

## Usage

### Basic State Machine

```csharp
using GameVariable.Intent;

// Create and start the state machine
var intent = new IntentState();
intent.Start(); // Enters initial state (CREATED)

// Dispatch events to transition
intent.DispatchEvent(IntentState.EventId.GET_READY);
// State: WAITING_FOR_ACTIVATION

intent.DispatchEvent(IntentState.EventId.ACTIVATED);
// State: WAITING_TO_RUN
```

### Checking State

```csharp
if (intent.stateId == IntentState.StateId.RUNNING)
{
    // Execute running logic
}
```

## Architecture

This package uses code generation (StateSmith) to produce highly optimized, flat switch-case state machines. The `IntentState` struct encapsulates the entire state machine logic, ensuring cache coherency and eliminating heap allocations.

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
