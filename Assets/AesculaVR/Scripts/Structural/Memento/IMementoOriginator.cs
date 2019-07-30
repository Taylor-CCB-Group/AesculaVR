/// <summary>
/// IMementoOriginator are objects that create mementos that can be used to return the object back to a previous state.
/// </summary>
public interface IMementoOriginator
{
    /// <summary>
    /// Saves the current state of the object to an memento
    /// </summary>
    /// <returns></returns>
    IMemento SaveMemento();
    /// <summary>
    /// Restores this object back to the state stored in the memento
    /// </summary>
    /// <param name="memento">The state we want to return this object to</param>
    void RestoreMemento(IMemento memento);
}
