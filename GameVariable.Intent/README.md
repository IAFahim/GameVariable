# 🧠 Variable.Intent

**The Brain of your Game Logic.** 🤖

**Variable.Intent** is a high-performance, **zero-allocation** Hierarchical State Machine (HSM). It organizes your AI, character controllers, and complex game flows into clean, manageable states.

---

## 📦 Installation

```bash
dotnet add package Variable.Intent
```

---

## 🤯 Mental Model

### The "Traffic Light" System 🚦
Imagine a traffic light. It can only be in **one state** at a time (Red, Yellow, or Green).
*   **Red:** Stop. Wait for Green.
*   **Green:** Go. Wait for Yellow.
*   **Yellow:** Slow down. Wait for Red.

**Variable.Intent** works the same way. Your character is either `IDLE`, `RUNNING`, or `JUMPING`. It handles the rules for switching between them so you don't end up `RUNNING` and `IDLE` at the same time (which looks weird).

---

## 👶 ELI5 (Explain Like I'm 5)

Writing `if` statements is like trying to give directions by shouting every turn at once.
*"Turn left, then if there's a dog turn right, but if it's raining stop, but if..."* 😵‍💫

**Variable.Intent** is like drawing a map.
1.  **Start Here.**
2.  **Go to 'Running'.**
3.  **If tired, go to 'Resting'.**

It keeps your code from becoming a bowl of spaghetti. 🍝

---

## 🚀 Usage Guide

### 1. The Setup

`IntentState` is your state machine manager.

```csharp
using Variable.Intent;

// Create the brain
var brain = new IntentState();

// Turn it on!
brain.Start();
// Current State: CREATED
```

### 2. Moving Around (Transitions)

You don't set states directly ("You are now running!").
Instead, you send **Signals** (Events).

```csharp
// Tell the brain to get ready
brain.DispatchEvent(IntentState.EventId.GET_READY);
// Current State: WAITING_FOR_ACTIVATION

// Tell the brain to GO!
brain.DispatchEvent(IntentState.EventId.ACTIVATED);
// Current State: WAITING_TO_RUN

// Actually start the task
brain.DispatchEvent(IntentState.EventId.START_RUNNING);
// Current State: RUNNING
```

### 3. The Loop

In your `Update()` loop, just check `brain.stateId` and do what it says.

```csharp
void Update() {
    switch (brain.stateId) {
        case IntentState.StateId.RUNNING:
            MoveCharacter();
            if (IsAtDestination()) {
                brain.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            }
            break;

        case IntentState.StateId.FAULTED:
            PlaySadSound();
            brain.DispatchEvent(IntentState.EventId.RECOVER_RETRY);
            break;
    }
}
```

---

## 🗺️ The Map (State Diagram)

Here is the full lifecycle of an Intent. It handles everything: starting, running, waiting for sub-tasks, failing, and retrying.

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

## 🔧 API Reference

### `IntentState`
- **`Start()`**: Initializes the machine.
- **`DispatchEvent(eventId)`**: Triggers a transition based on the current state.
- **`stateId`**: The current state (Read-only).

### Key States
- **`CREATED`**: Born, but not ready.
- **`WAITING_TO_RUN`**: Ready, set... (waiting for `START_RUNNING`).
- **`RUNNING`**: GO! Doing the work.
- **`FAULTED`**: Something went wrong. Can we retry?
- **`RAN_TO_COMPLETION`**: Finished successfully.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
