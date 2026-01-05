# Combo System Architecture

```mermaid
classDiagram
    %% Data Layer (Structs)
    class ComboGraph {
        <<Ref Struct>>
        +ReadOnlySpan~ComboNode~ Nodes
        +ReadOnlySpan~ComboEdge~ Edges
        +int NodeCount
        +int EdgeCount
    }
    
    class ComboNode {
        <<Struct>>
        +int ActionID
        +int EdgeStartIndex
        +int EdgeCount
    }
    
    class ComboEdge {
        <<Struct>>
        +int InputTrigger
        +int TargetNodeIndex
    }
    
    class ComboState {
        <<Struct>>
        +int CurrentNodeIndex
        +bool IsActionBusy
    }
    
    class InputRingBuffer {
        <<Struct>>
        +int Input0..7
        +int Head
        +int Tail
        +int Count
    }

    %% Logic Layer (Static)
    class ComboLogic {
        <<Static>>
        +TryAdvanceState(ref state, ref buffer, nodes, edges, out actionID)
        +SignalActionFinished(ref state)
    }

    %% Relationships
    ComboGraph *-- ComboNode : Contains
    ComboGraph *-- ComboEdge : Contains
    
    ComboNode ..> ComboEdge : References via Index (CSR)
    ComboEdge ..> ComboNode : References via Index
    ComboState ..> ComboNode : Tracks Current Node Index
    
    %% Logic Usage
    ComboLogic ..> ComboState : Mutates
    ComboLogic ..> InputRingBuffer : Consumes
    ComboLogic ..> ComboGraph : Reads Structure

```mermaid
sequenceDiagram
    participant GameLoop
    participant Logic as ComboLogic
    participant State as ComboState
    participant Buffer as InputRingBuffer
    participant Graph as ComboGraph

    GameLoop->>Logic: TryAdvanceState(ref state, ref buffer...)
    
    Logic->>State: Check IsActionBusy
    alt is Busy
        Logic-->>GameLoop: Return False
    else is Idle
        Logic->>Buffer: Peek Next Input
        Logic->>Graph: Get Current Node & Edges
        
        loop Check Edges
            Logic->>Logic: Match Input vs Edge.Trigger
        end
        
        alt Match Found
            Logic->>State: Update CurrentNodeIndex
            Logic->>State: Set IsActionBusy = true
            Logic->>Buffer: Consume Input
            Logic-->>GameLoop: Return True (New ActionID)
        else No Match
            Logic-->>GameLoop: Return False
        end
    end
``````
