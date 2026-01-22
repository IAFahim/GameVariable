# 🧠 GameVariable.Intent

**High-Performance AI & Task State Machines.** 🤖

**GameVariable.Intent** provides a zero-allocation, struct-based Hierarchical State Machine (HSM) designed for managing complex task lifecycles (like AI goals, quests, or job systems).

---

## 📦 Installation

```bash
dotnet add package Variable.Intent
```

---

## 🧠 Mental Model

Think of an **Employee's Job Status**:
1. **Created**: You hired them, but they haven't started.
2. **Waiting For Activation**: They are at the desk, waiting for a manager to say "Go".
3. **Waiting To Run**: The manager said "Go", but they are grabbing coffee.
4. **Running**: They are actually working.
5. **Waiting For Children**: They delegated work to an intern and are waiting.
6. **Ran To Completion**: Job done!
7. **Faulted**: Something broke (PC caught fire).

You move between these states by sending **Events** (signals).

---

## 🚀 Features

* **⚡ Zero Allocation:** Pure struct state machine.
* **🌳 Hierarchical:** Supports sub-states like "Waiting for Children".
* **🛡️ Generated:** Powered by StateSmith for bug-free, optimized C# code.
* **🧩 Generic:** Use it for AI, Quests, Abilities, or UI flows.

---

## 📊 State Diagram

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

// 1. Initialize
var intent = new IntentState();
intent.Start(); // State: CREATED

// 2. Prepare
intent.DispatchEvent(IntentState.EventId.GET_READY); // State: WAITING_FOR_ACTIVATION

// 3. Activate
intent.DispatchEvent(IntentState.EventId.ACTIVATED); // State: WAITING_TO_RUN

// 4. Run
intent.DispatchEvent(IntentState.EventId.START_RUNNING); // State: RUNNING
Console.WriteLine("AI is doing work...");

// 5. Finish
intent.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY); // State: RAN_TO_COMPLETION
```

### 2. Handling Failures (Faults)

```csharp
if (PathBlocked())
{
    intent.DispatchEvent(IntentState.EventId.UNABLE_TO_COMPLETE);
    // State: FAULTED
}

// Try to fix it
if (FindNewPath())
{
    intent.DispatchEvent(IntentState.EventId.RECOVER_RETRY);
    // State: WAITING_TO_RUN (Ready to try again)
}
else
{
    intent.DispatchEvent(IntentState.EventId.UNABLE_TO_RECOVER);
    // State: CANCELED (Give up)
}
```

### 3. Sub-Tasks (Delegation)

Useful for complex AI (e.g., "Attack" needs to "MoveTo" first).

```csharp
// Current State: RUNNING

// "I need to move first!"
SpawnMoveTask();
intent.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);
// State: WAITING_FOR_CHILDREN_TO_COMPLETE (Self is paused)

// ... frames later ...

if (MoveTask.IsComplete)
{
    intent.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
    // State: RUNNING (Resumes attacking)
}
```

---

## 🔧 API Reference

### `IntentState`
The main struct. Contains the entire state machine logic.

### `EventId` (Enum)
Signals you send to change state.
- `ACTIVATED`, `CANCEL`, `START_RUNNING`
- `COMPLETED_SUCCESSFULLY`, `UNABLE_TO_COMPLETE`
- `CHILD_TASK_CREATED`, `ALL_CHILDREN_COMPLETED`
- `RECOVER_RETRY`

### `StateId` (Enum)
The current status.
- `CREATED`, `WAITING_FOR_ACTIVATION`, `WAITING_TO_RUN`
- `RUNNING`, `WAITING_FOR_CHILDREN_TO_COMPLETE`
- `RAN_TO_COMPLETION`, `FAULTED`, `CANCELED`

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
