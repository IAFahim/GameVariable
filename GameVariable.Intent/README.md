# 🧠 GameVariable.Intent

**The Brains of the Operation.** 🤖

**GameVariable.Intent** is a high-performance, zero-allocation Hierarchical State Machine (HSM). It's designed to manage complex AI states, ability lifecycles, and mission logic without the spaghetti code of nested boolean flags.

---

## 📦 Installation

```bash
dotnet add package GameVariable.Intent
```

---

## ❓ Why do I need this?

You might be using `enum` or `bool` flags to manage state:

```csharp
// ❌ The Spaghetti Way
if (isAttacking) {
    if (!isStunned && !isDead) {
         // ...
    }
} else if (isWalking) {
    // ...
}
```

**GameVariable.Intent** gives you a formal, rigorous State Machine that enforces valid transitions.
*   You cannot "Start Running" if you are "Canceled".
*   You cannot "Complete" if you haven't "Started".

It's perfect for:
*   **AI Behaviors:** (Idle -> Chase -> Attack -> Flee)
*   **Ability Logic:** (Charging -> Casting -> Channeling -> Cooldown)
*   **Mission Objectives:** (Locked -> Active -> Completed)

---

## 📊 The Lifecycle Diagram

This isn't just a random graph. It's the lifecycle of *getting things done*.

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

---

## 🎮 Usage Guide

### 1. Basic State Machine

```csharp
using GameVariable.Intent;

// Create and start the state machine
var intent = new IntentState();
intent.Start(); // Enters initial state (CREATED)

// "I'm ready, but not activated yet."
intent.DispatchEvent(IntentState.EventId.GET_READY);
// State: WAITING_FOR_ACTIVATION

// "Go!"
intent.DispatchEvent(IntentState.EventId.ACTIVATED);
// State: WAITING_TO_RUN

// "Action!"
intent.DispatchEvent(IntentState.EventId.START_RUNNING);
// State: RUNNING
```

### 2. Handling Logic Updates

In your game loop, check `stateId` to decide what to do.

```csharp
public void Update()
{
    switch (intent.stateId)
    {
        case IntentState.StateId.RUNNING:
            // Do the actual work here
            MoveTowardsPlayer();

            if (ReachedPlayer())
                intent.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            break;

        case IntentState.StateId.FAULTED:
            // Handle errors
            Debug.LogError("Task failed! Retrying...");
            intent.DispatchEvent(IntentState.EventId.RECOVER_RETRY);
            break;
    }
}
```

### 3. Hierarchical Tasks (Sub-Tasks)

The killer feature: **Sub-Tasks**.
If an AI needs to "Open Door" before it can "Enter Room", you can pause the main task.

```csharp
// In RUNNING state...
if (DoorIsClosed())
{
    // 1. Pause this task
    intent.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
    // State is now: WAITING_FOR_CHILDREN_TO_COMPLETE

    // 2. Start the child task
    doorOpeningTask.Start();
}

// ... later ...

if (doorOpeningTask.IsComplete)
{
    // 3. Resume main task
    intent.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
    // State is back to: RUNNING
}
```

---

## 🏗️ Architecture

This package is powered by a **StateSmith** generated state machine. It is a single `struct` containing a flat `switch` statement, ensuring:
*   **Zero Allocation:** No `new State()` classes.
*   **Cache Locality:** Everything is in one struct.
*   **Predictability:** No dynamic dispatch or v-tables.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
