# GameVariable.Intent

A high-performance, zero-allocation Hierarchical State Machine (HSM) for managing game AI behaviors, quest systems, and complex task workflows in Unity.

## Overview

`GameVariable.Intent` provides a robust, event-driven state machine implementation designed for performance-critical game systems. The Intent system is particularly well-suited for:

- **Game AI Behavior Management** - Controlling NPC/intent behaviors with complex state transitions
- **Quest/Task Systems** - Managing game objectives, prerequisites, and completion states
- **Workflow Orchestration** - Sequential and parallel task execution with error handling
- **Hierarchical Task Networks** - Complex task decomposition and child-process coordination

## Architecture

The Intent system follows the **Data-Oriented Design (DOD)** principles:

### Zero-Allocation Struct-Based Design
- Implemented as a `struct` for stack allocation and cache efficiency
- No heap allocations during state transitions
- Ideal for Burst compiler optimization

### Thread-Unsafe Single-Threaded Design
- Optimized for Unity's main thread game loop
- No locking overhead
- Predictable performance characteristics

### Event-Driven State Transitions
- Discrete event-based state changes
- No polling required
- Clear audit trail of state changes

## State Machine Diagram

```
                    ┌─────────────────────────────────────────────────────────────┐
                    │                        HAPPY PATH (Green)                    │
                    └─────────────────────────────────────────────────────────────┘

    [*] ──► CREATED ──PREPARE──► INACTIVE ──ACTIVATE──► PENDING ──START──► RUNNING
        │                                                          │
        │                                                          └──COMPLETE──► COMPLETED
        │
        └──ACTIVATE──► PENDING

                    ┌─────────────────────────────────────────────────────────────┐
                    │                    CHILD PROCESS FLOW (Blue)                 │
                    └─────────────────────────────────────────────────────────────┘

                                    RUNNING ──SPAWN_CHILD──► BLOCKED
                                                               │
                                                               └──RESUME──► RUNNING

                    ┌─────────────────────────────────────────────────────────────┐
                    │                     RECOVERY FLOW (Goldenrod)                │
                    └─────────────────────────────────────────────────────────────┘

                    COMPLETED ──RESTART──► PENDING    FAULTED ──RECOVER──► PENDING

                    ┌─────────────────────────────────────────────────────────────┐
                    │                       ERROR FLOW (Red)                       │
                    └─────────────────────────────────────────────────────────────┘

                    RUNNING ──FAIL──► FAULTED         BLOCKED ──ABORT──► FAULTED

                    ┌─────────────────────────────────────────────────────────────┐
                    │                    CANCELLATION FLOW (Gray)                  │
                    └─────────────────────────────────────────────────────────────┘

    CREATED ──┐
    INACTIVE ──┤
    PENDING ───┴──CANCEL──► CANCELLED ◄──CANCEL──┬── RUNNING
    FAULTED ─────────────────────────────────────┤
                                                  └── BLOCKED
```

## States

| State | Description | Terminal |
|-------|-------------|----------|
| `CREATED` | Initial state after `Start()` is called | No |
| `INACTIVE` | Intent prepared but not yet activated | No |
| `PENDING` | Intent ready to execute | No |
| `RUNNING` | Intent actively executing | No |
| `BLOCKED` | Waiting for child processes to complete | No |
| `COMPLETED` | Intent successfully completed | Yes |
| `FAULTED` | Intent failed with error | Yes |
| `CANCELLED` | Intent was cancelled | Yes |

## Events

| Event | Source States | Target State | Description |
|-------|---------------|--------------|-------------|
| `PREPARE` | CREATED | INACTIVE | Prepare intent without activating |
| `ACTIVATE` | CREATED, INACTIVE | PENDING | Activate intent for execution |
| `START` | PENDING | RUNNING | Begin execution |
| `COMPLETE` | RUNNING | COMPLETED | Mark execution as successful |
| `SPAWN_CHILD` | RUNNING | BLOCKED | Spawn child process, wait for completion |
| `RESUME` | BLOCKED | RUNNING | Resume after child process completes |
| `RESTART` | COMPLETED | PENDING | Restart a completed intent |
| `RECOVER` | FAULTED | PENDING | Recover from a failed intent |
| `FAIL` | RUNNING | FAULTED | Mark execution as failed |
| `ABORT` | BLOCKED | FAULTED | Abort blocked intent |
| `CANCEL` | * | CANCELLED | Cancel intent (from any state) |

## Quick Start

### Basic Usage

```csharp
using GameVariable.Intent;

// Create and initialize the state machine
var intentState = new IntentState();
intentState.Start();

// Activate and start the intent
intentState.DispatchEvent(IntentState.EventId.ACTIVATE);
intentState.DispatchEvent(IntentState.EventId.START);

// Check current state
if (intentState.stateId == IntentState.StateId.RUNNING)
{
    // Intent is running
}

// Mark as completed
intentState.DispatchEvent(IntentState.EventId.COMPLETE);

// Restart for another execution
intentState.DispatchEvent(IntentState.EventId.RESTART);
```

### Error Handling

```csharp
var intentState = new IntentState();
intentState.Start();
intentState.DispatchEvent(IntentState.EventId.ACTIVATE);
intentState.DispatchEvent(IntentState.EventId.START);

// Something went wrong
intentState.DispatchEvent(IntentState.EventId.FAIL);

// Recover from failure
if (intentState.stateId == IntentState.StateId.FAULTED)
{
    intentState.DispatchEvent(IntentState.EventId.RECOVER);
}
```

### Child Process Coordination

```csharp
var intentState = new IntentState();
intentState.Start();
intentState.DispatchEvent(IntentState.EventId.ACTIVATE);
intentState.DispatchEvent(IntentState.EventId.START);

// Spawn child task
intentState.DispatchEvent(IntentState.EventId.SPAWN_CHILD);

// Current state is now BLOCKED
// ... child task executes ...

// Resume when child completes
intentState.DispatchEvent(IntentState.EventId.RESUME);
```

### Cancellation

```csharp
var intentState = new IntentState();
intentState.Start();

// Cancel from any state
intentState.DispatchEvent(IntentState.EventId.CANCEL);

// Current state is now CANCELLED
```

## Design Philosophy

This library follows the **"Performance by Structure, Readability by Naming"** philosophy:

### Clean Code Principles

1. **No Mental Mapping** - Variable names are self-explanatory. No single-letter variables or cryptic abbreviations.
2. **Pronounceable Names** - All identifiers can be read aloud in a code review.
3. **Searchable Names** - Names are unique enough for instant `Ctrl+F` navigation.
4. **No Type Encoding** - No Hungarian notation. The IDE handles types.

### Data-Logic-Extension Triad

While the generated code is from StateSmith, the design follows a clean separation:

- **Layer A (Data)**: `IntentState` struct - Pure memory storage
- **Layer B (Logic)**: Event handlers - Stateless computation
- **Layer C (Adapter)**: `IIntent<TState, TEvent>` interface - Public API

## Performance Characteristics

- **Zero Allocations**: No heap allocations during state transitions
- **Cache-Friendly**: Struct-based design with sequential memory layout
- **Burst Compatible**: Can be compiled with Unity Burst Compiler
- **Predictable**: O(1) event dispatch with no dynamic branching

## Thread Safety

**The Intent state machine is NOT thread-safe.** It is designed for single-threaded Unity game loops. If you need multi-threaded state machines, implement external synchronization.

## State Machine Generation

This state machine is generated using [StateSmith](https://github.com/StateSmith/StateSmith) v0.19.0. To modify the state machine:

1. Edit `IntentState.puml` (PlantUML source)
2. Run StateSmith to regenerate `IntentState.cs`
3. Update tests to match new behavior

## Testing

The library includes comprehensive unit tests covering all state transitions and event flows:

```bash
dotnet test GameVariable.Intent.Tests
```

Tests are organized by flow type:
- **Happy Path Tests** - Normal execution flow
- **Child Process Tests** - Parent-child task coordination
- **Recovery Tests** - Error recovery scenarios
- **Error Tests** - Failure state transitions
- **Cancellation Tests** - Cancellation from all states

## License

[Your License Here]

## Contributing

Contributions are welcome! Please ensure:
1. All tests pass
2. Code follows the project's Clean Code mandates
3. XML documentation is included for public APIs
4. Tests are added for new state transitions

## See Also

- [StateSmith Documentation](https://github.com/StateSmith/StateSmith/wiki)
- PlantUML diagram: `IntentState.puml`
- Interactive simulator: `IntentState.sim.html`
