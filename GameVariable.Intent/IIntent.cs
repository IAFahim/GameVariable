namespace GameVariable.Intent;

public interface IIntent<in TState, in TEvent>
{
    public void Start();
    public void DispatchEvent(TEvent eventId);
    public string EventIdToString(TEvent eventId);
    public string StateIdToString(TState id);
}