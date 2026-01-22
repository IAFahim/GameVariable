# 🧠 GameVariable.Intent

**The "Brain" of the operation.** 🤖

**GameVariable.Intent** is a high-performance Hierarchical State Machine (HSM). It creates the "Life Cycle" of a task, an AI agent, or a game state.

---

## 🧠 Mental Model

### The Life Cycle 🧬
Every task follows a universal pattern:
1.  **Created:** "I exist."
2.  **Waiting:** "Waiting for permission."
3.  **Running:** "Doing the thing."
4.  **Completed:** "I did it!" (or **Faulted:** "I failed.")

**GameVariable.Intent** enforces this flow so you don't have to invent "bool isStarted" flags.

### Hierarchy (The Parent/Child) 👪
- **Parent:** "Go kill the dragon."
- **Child 1:** "Find Dragon." (Complete)
- **Child 2:** "Attack Dragon." (Running...)
  - **Grandchild:** "Swing Sword."

The Parent waits for the Child. If the Child fails, the Parent knows.

---

## 👶 ELI5 (Explain Like I'm 5)

Without GameVariable.Intent:
> **You:** "State = Attacking."
> **You:** "Wait, he died while attacking. Now State = Dead. But Attack code is still running!"
> **You:** "NullReferenceException." 💥

With GameVariable.Intent:
> **You:** `intent.Dispatch(Event.CANCEL);`
> **Computer:** "I was Attacking. I moved to Canceled state. I cleaned up my sword. I am now safe."

---

## 📦 Installation

```bash
dotnet add package Variable.Intent
```

---

## 🎮 Usage Guide

### 1. The Standard Flow

```csharp
using GameVariable.Intent;

var task = new IntentState();
task.Start(); // State: CREATED

// "Hey, get ready."
task.DispatchEvent(IntentState.EventId.GET_READY); // State: WAITING_FOR_ACTIVATION

// "Go!"
task.DispatchEvent(IntentState.EventId.ACTIVATED); // State: WAITING_TO_RUN
task.DispatchEvent(IntentState.EventId.START_RUNNING); // State: RUNNING

// "I'm done!"
task.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY); // State: RAN_TO_COMPLETION
```

### 2. The Logic Loop

Connect your logic to the state.

```csharp
void Update()
{
    switch (task.stateId)
    {
        case IntentState.StateId.RUNNING:
            // Do the actual work
            MoveToTarget();

            if (Arrived)
                task.DispatchEvent(IntentState.EventId.COMPLETED_SUCCESSFULLY);
            break;

        case IntentState.StateId.FAULTED:
            // Handle error
            PlaySadSound();
            break;
    }
}
```

### 3. Sub-Tasks (Hierarchy)

Handling complex sequences.

```csharp
// In RUNNING state...
if (NeedToReload)
{
    // Pause this task, wait for children
    task.DispatchEvent(IntentState.EventId.CHILD_TASK_CREATED);

    StartReloadSubTask();
}

// When sub-task finishes...
task.DispatchEvent(IntentState.EventId.ALL_CHILDREN_COMPLETED);
// Back to RUNNING!
```

---

## 🔧 API Reference

### `IntentState`
- `.stateId`: The current state enum.
- `.DispatchEvent(id)`: Triggers a transition.
- `.Start()`: Initializes the machine.

### Events
- `ACTIVATED`, `CANCEL`, `START_RUNNING`
- `COMPLETED_SUCCESSFULLY`, `UNABLE_TO_COMPLETE`
- `CHILD_TASK_CREATED`, `ALL_CHILDREN_COMPLETED`

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
