# 🧠 GameVariable.Intent

**The Traffic Light for your Logic.** 🚦

**GameVariable.Intent** is a high-performance, zero-allocation Hierarchical State Machine (HSM).
Stop writing spaghetti `bool isAttacking; bool isWaiting; bool isDead;` logic. Use an Intent.

---

## 📦 Installation

```bash
dotnet add package Variable.Intent
```

---

## 🚦 The Mental Model: The Traffic Light

Think of every task (AI decision, animation, spell cast) as a car at a traffic light.

1.  **🔴 STOP (Created):** The car exists, but engine is off.
2.  **🟡 GET READY (Waiting):** Engine on. Waiting for the signal (Input/Activation).
3.  **🟢 GO (Running):** Driving down the road (Executing logic).

If you crash? **FAULTED**.
If you finish the race? **RAN_TO_COMPLETION**.

This structure enforces a clean lifecycle for everything in your game.

---

## 🚀 Features

* **🏎️ Zero Allocation:** A struct-based state machine. No GC pressure.
* **⚡ Event-Driven:** Dispatch events (`GET_READY`, `START_RUNNING`) to move the traffic light.
* **👶 Hierarchical:** Can "Pause" (Wait for Children) while sub-tasks run.
* **🛡️ Bulletproof:** You can't run a Red light. The state machine prevents invalid transitions.

---

## 📊 State Machine Diagram

The `IntentState` enforces this flow:

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

### 1. Basic Lifecycle

```csharp
using GameVariable.Intent;

// 1. Create (RED LIGHT) 🔴
var intent = new IntentState();
intent.Start(); // State: CREATED

// 2. Prepare (YELLOW LIGHT) 🟡
intent.DispatchEvent(IntentState.EventId.GET_READY);
// State: WAITING_FOR_ACTIVATION

intent.DispatchEvent(IntentState.EventId.ACTIVATED);
// State: WAITING_TO_RUN

// 3. Go! (GREEN LIGHT) 🟢
intent.DispatchEvent(IntentState.EventId.START_RUNNING);
// State: RUNNING
```

### 2. The Game Loop Pattern

In your `Update()` loop, just check the light color!

```csharp
public void Update()
{
    // What color is the light?
    switch (intent.stateId)
    {
        case IntentState.StateId.RUNNING:
            // 🟢 DRIVE!
            DriveCar();

            if (ArrivedAtDestination())
            {
                intent.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            }
            break;

        case IntentState.StateId.FAULTED:
            // 💥 CRASH!
            PlaySmokeEffect();
            intent.DispatchEvent(IntentState.EventId.RECOVER_RETRY);
            break;
    }
}
```

### 3. Sub-Tasks (Parenting)

Sometimes you need to stop and wait for a duck to cross the road (Sub-task).

```csharp
// In RUNNING state
if (DuckCrossing())
{
    SpawnDuck();

    // Switch to "Waiting for Children"
    intent.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
}

// ... later when duck is gone ...
intent.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
// Back to RUNNING!
```

---

## 🏗️ Architecture

This package uses code generation (StateSmith) to produce highly optimized, flat switch-case state machines. The `IntentState` struct encapsulates the entire state machine logic, ensuring cache coherency and eliminating heap allocations.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
