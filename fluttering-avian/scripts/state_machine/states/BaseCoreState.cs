namespace fluttering_avian.state_machine.states;

using global::StateMachine;
using input_manager;

public abstract class BaseCoreState : State<StateType>
{
    protected InputManager InputManager;
    protected RuntimeData RuntimeData;
    
    public override StateType StateType { get; }
    
    public BaseCoreState(RuntimeData runtimeData, InputManager inputManager, StateType stateType)
    {
        RuntimeData = runtimeData;
        InputManager = inputManager;
        StateType = stateType;
    }
}