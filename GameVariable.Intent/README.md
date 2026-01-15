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

## State Machine Diagram

The `IntentState` implements the following flow:

```mermaid
stateDiagram-v2
    [*] --> CREATED

    state CREATED {
    }

    CREATED --> WAITING_TO_RUN : ACTIVATED
    CREATED --> WAITING_FOR_ACTIVATION : GET_READY
    CREATED --> CANCELED : CANCEL

    state WAITING_FOR_ACTIVATION {
    }

    WAITING_FOR_ACTIVATION --> WAITING_TO_RUN : ACTIVATED
    WAITING_FOR_ACTIVATION --> CANCELED : CANCEL

    state WAITING_TO_RUN {
    }

    WAITING_TO_RUN --> RUNNING : START_RUNNING
    WAITING_TO_RUN --> CANCELED : CANCEL

    state RUNNING {
    }

    RUNNING --> RAN_TO_COMPLETION : COMPLETED_SUCCESSFULLY
    RUNNING --> WAITING_FOR_CHILDREN_TO_COMPLETE : CHILD_TASK_CREATED
    RUNNING --> FAULTED : UNABLE_TO_COMPLETE
    RUNNING --> CANCELED : CANCEL

    state WAITING_FOR_CHILDREN_TO_COMPLETE {
    }

    WAITING_FOR_CHILDREN_TO_COMPLETE --> RUNNING : ALL_CHILDREN_COMPLETED
    WAITING_FOR_CHILDREN_TO_COMPLETE --> FAULTED : UNABLE_TO_RECOVER
    WAITING_FOR_CHILDREN_TO_COMPLETE --> CANCELED : CANCEL

    state RAN_TO_COMPLETION {
    }

    RAN_TO_COMPLETION --> WAITING_TO_RUN : RUN_AGAIN

    state FAULTED {
    }

    FAULTED --> WAITING_TO_RUN : RECOVER_RETRY
    FAULTED --> CANCELED : UNABLE_TO_RECOVER
```

## Usage

### 1. Basic State Machine

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

intent.DispatchEvent(IntentState.EventId.START_RUNNING);
// State: RUNNING
```

### 2. Handling Logic Updates

In your game loop, check the state and execute logic:

```csharp
public void Update()
{
    switch (intent.stateId)
    {
        case IntentState.StateId.RUNNING:
            ExecuteTask();
            if (TaskIsComplete())
                intent.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            break;

        case IntentState.StateId.FAULTED:
            Debug.LogError("Task failed!");
            intent.DispatchEvent(IntentState.EventId.RECOVER_RETRY);
            break;
    }
}
```

### 3. Hierarchical Tasks

You can use the `CHILD_TASK_CREATED` event to suspend the current task while a sub-task completes.

```csharp
// In RUNNING state
if (NeedsSubTask())
{
    SpawnChildTask();
    intent.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
    // State: WAITING_FOR_CHILDREN_TO_COMPLETE
}

// ... later when child completes ...
intent.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
// State: RUNNING (resumes)
```

## Architecture

This package uses code generation (StateSmith) to produce highly optimized, flat switch-case state machines. The `IntentState` struct encapsulates the entire state machine logic, ensuring cache coherency and eliminating heap allocations.

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
