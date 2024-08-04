namespace fluttering_avian.state_machine.states;

using global::StateMachine;
using input_manager;
using ViewManager;

public abstract class BaseCoreState : State<StateType>
{
    protected ViewManager ViewManager;
    protected RuntimeData RuntimeData;
    protected InputManager InputManager;

    public override StateType StateType { get; }

    public BaseCoreState(ViewManager viewManager, RuntimeData runtimeData, InputManager inputManager,
        StateType stateType)
    {
        ViewManager = viewManager;
        RuntimeData = runtimeData;
        InputManager = inputManager;
        StateType = stateType;
    }
}