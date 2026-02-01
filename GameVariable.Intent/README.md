# 🧠 GameVariable.Intent

**The Traffic Light.** 🚦

**GameVariable.Intent** is a high-performance, zero-allocation State Machine for managing Tasks, Quests, and AI behaviors. It ensures your game logic doesn't end up in an invalid state.

---

## 🧠 Mental Model: The Traffic Light

A Traffic Light can only be Green, Yellow, or Red.
It cannot be "Purple".
It cannot go from "Green" directly to "Red" (usually).

**GameVariable.Intent** enforces these rules for your tasks.
*   **Created:** The light is installed.
*   **Running:** The light is Green.
*   **Completed:** The task is done.
*   **Faulted:** The bulb broke.

---

## 👶 ELI5: "I can't complete a quest I haven't started!"

If you try to call `CompleteQuest()` on a quest the player doesn't have, your game might crash or bug out.

**GameVariable.Intent** stops this.
1.  Quest is `CREATED`.
2.  You try to `COMPLETE`.
3.  Intent says: "Illegal Move! You must `START` first."

It keeps your logic clean and bug-free.

---

## 📦 Installation

```bash
dotnet add package GameVariable.Intent
```

---

## 🚀 Usage Guide

### 1. The Happy Path

The standard lifecycle of a task.

```csharp
using GameVariable.Intent;

// 1. Create
var task = new IntentState();
task.Start(); // Sets state to CREATED

// 2. Activate (Optional "Warning" phase)
task.DispatchEvent(IntentState.EventId.ACTIVATE); // -> PENDING

// 3. Start Execution
task.DispatchEvent(IntentState.EventId.START); // -> RUNNING

// 4. Do work...

// 5. Complete
task.DispatchEvent(IntentState.EventId.COMPLETE); // -> COMPLETED
```

### 2. Handling Failure & Recovery

Things go wrong. The Intent system handles retries.

```csharp
// Task is RUNNING
try
{
    DoHardWork();
}
catch
{
    // Transition to FAULTED
    task.DispatchEvent(IntentState.EventId.FAIL);
}

// Later... try again?
if (task.stateId == IntentState.StateId.FAULTED)
{
    // Transition back to PENDING (Ready to start)
    task.DispatchEvent(IntentState.EventId.RECOVER);
}
```

### 3. Child Processes (Blocking)

Sometimes a task needs to wait for a sub-task.

```csharp
// Task is RUNNING

// 1. Spawn a sub-task
task.DispatchEvent(IntentState.EventId.SPAWN_CHILD);
// State is now BLOCKED (Cannot Complete, Cannot Fail)

// ... Sub-task runs ...

// 2. Sub-task finishes
task.DispatchEvent(IntentState.EventId.RESUME);
// State is back to RUNNING
```

---

## 📊 State Diagram

```
[CREATED] --(Activate)--> [PENDING] --(Start)--> [RUNNING] --(Complete)--> [COMPLETED]
                              ^                       |
                              |--(Recover)-- [FAULTED]<-(Fail)-|
```

(See `IntentState.puml` for the full, rigorous diagram.)

---

## 🔧 API Reference

### States
- `CREATED`: Fresh instance.
- `PENDING`: Ready to run.
- `RUNNING`: Doing work.
- `BLOCKED`: Waiting on child.
- `COMPLETED`: Success.
- `FAULTED`: Failure.
- `CANCELLED`: Aborted.

### Events
- `ACTIVATE`: Created -> Pending.
- `START`: Pending -> Running.
- `COMPLETE`: Running -> Completed.
- `FAIL`: Running -> Faulted.
- `RECOVER`: Faulted -> Pending.
- `CANCEL`: Any -> Cancelled.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
