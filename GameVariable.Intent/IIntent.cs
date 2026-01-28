namespace GameVariable.Intent;

/// <summary>
///     Defines the contract for an Intent State Machine.
/// </summary>
/// <typeparam name="TState">The enum type representing the states.</typeparam>
/// <typeparam name="TEvent">The enum type representing the events.</typeparam>
public partial interface IIntent<in TState, in TEvent>
{
    /// <summary>
    ///     Starts the state machine, entering the initial state.
    ///     Must be called before dispatching any events.
    /// </summary>
    public void Start();

    /// <summary>
    ///     Dispatches an event to the state machine, potentially triggering a state transition.
    /// </summary>
    /// <param name="eventId">The event to dispatch.</param>
    public void DispatchEvent(TEvent eventId);
}
