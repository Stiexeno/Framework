using Framework;

public enum State { Success, Failure, Running }
public abstract class Node : GraphBehaviour
{
    public State state = State.Running;
    
    public bool started = false;
    public string guid;

    public State Update()
    {
        if (started == false)
        {
            OnStart();
            started = true;
        }
        
        state = OnUpdate();
        
        if (state != State.Running)
        {
            OnStop();
            started = false;
        }

        return state;
    }
    
    protected abstract void OnStart();
    protected abstract State OnUpdate();
    protected abstract void OnStop();
}
