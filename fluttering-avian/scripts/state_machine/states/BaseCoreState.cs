namespace fluttering_avian.state_machine.states;

using global::StateMachine;
using Godot;

public abstract partial class BaseCoreState : Resource, IState<StateType>
{
    public IStateMachine<StateType> StateMachine { get; set; }
    public abstract StateType StateType { get; }
    
    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }
}