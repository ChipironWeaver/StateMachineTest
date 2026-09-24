using UnityEngine.AI;

public abstract class State
{
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
    
    protected FSMAI fsmai;

    protected State(FSMAI fsmai)
    {
        this.fsmai = fsmai;
        Enter();
    }
}