namespace GameVariable.Intent;

/// <summary>
///     Defines the contract for an Intent State Machine.
/// </summary>
/// <remarks>
///     <para>
///         The Intent system provides a hierarchical state machine for managing game behaviors,
///         quest systems, and task workflows. This interface defines the core contract that all
///         Intent state machines must implement.
///     </para>
///     <para>
///         <strong>Thread Safety:</strong> Intent state machines are NOT thread-safe and are
///         designed for single-threaded Unity game loops. External synchronization is required
///         for multi-threaded scenarios.
///     </para>
///     <para>
///         <strong>Performance:</strong> Implemented as a struct for zero-allocation performance.
///         Ideal for use with Unity's Burst Compiler and Jobs system.
///     </para>
/// </remarks>
/// <typeparam name="TState">
///     The enum type representing the states. Must be an enum with contiguous values starting at 0.
/// </typeparam>
/// <typeparam name="TEvent">
///     The enum type representing the events. Must be an enum with contiguous values starting at 0.
/// </typeparam>
/// <example>
///     Basic usage:
///     <code><![CDATA[
///     var intentState = new IntentState();
///     intentState.Start();
///     intentState.DispatchEvent(IntentState.EventId.ACTIVATE);
///     ]]></code>
/// </example>
public partial interface IIntent<in TState, in TEvent>
{
    /// <summary>
    ///     Starts the state machine, entering the initial state.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method must be called before dispatching any events. It initializes the
    ///         state machine and transitions to the initial state (typically CREATED).
    ///     </para>
    ///     <para>
    ///         <strong>Preconditions:</strong>
    ///         <list type="bullet">
    ///             <item>The state machine must be freshly constructed or reset.</item>
    ///             <item>No events should be dispatched before calling Start().</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <strong>Postconditions:</strong>
    ///         <list type="bullet">
    ///             <item>The state machine is in its initial state.</item>
    ///             <item>Events can now be dispatched.</item>
    ///         </list>
    ///     </para>
    /// </remarks>
    /// <example>
    ///     Initialize a new Intent state machine:
    ///     <code><![CDATA[
    ///     var intentState = new IntentState();
    ///     intentState.Start();
    ///     Debug.Assert(intentState.stateId == IntentState.StateId.CREATED);
    ///     ]]></code>
    /// </example>
    public void Start();

    /// <summary>
    ///     Dispatches an event to the state machine, potentially triggering a state transition.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Events are processed synchronously. The state machine will evaluate the current
    ///         state and transition according to the event's defined behavior. Not all events
    ///         cause state transitions - some may be ignored depending on the current state.
    ///     </para>
    ///     <para>
    ///         <strong>Preconditions:</strong>
    ///         <list type="bullet">
    ///             <item><see cref="Start()"/> must have been called prior to dispatching events.</item>
    ///             <item>The eventId must be a valid enum value.</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         <strong>Postconditions:</strong>
    ///         <list type="bullet">
    ///             <item>The state may have changed depending on the event and current state.</item>
    ///             <item>State entry/exit behaviors may have been executed.</item>
    ///         </list>
    ///     </para>
    /// </remarks>
    /// <param name="eventId">
    ///     The event to dispatch. Must be a valid value from the EventId enum.
    /// </param>
    /// <example>
    ///     Dispatch a sequence of events:
    ///     <code><![CDATA[
    ///     var intentState = new IntentState();
    ///     intentState.Start();
    ///
    ///     // Activate the intent
    ///     intentState.DispatchEvent(IntentState.EventId.ACTIVATE);
    ///
    ///     // Start execution
    ///     intentState.DispatchEvent(IntentState.EventId.START);
    ///
    ///     // Mark as completed
    ///     intentState.DispatchEvent(IntentState.EventId.COMPLETE);
    ///     ]]></code>
    /// </example>
    public void DispatchEvent(TEvent eventId);
}
