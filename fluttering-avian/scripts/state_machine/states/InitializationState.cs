namespace fluttering_avian.state_machine.states;

using input_manager;

public class InitializationState : BaseCoreState
{
    public InitializationState(RuntimeData runtimeData, InputManager inputManager) : base(runtimeData, inputManager,
        StateType.Initialization)
    {
    }

    public override void OnEnter()
    {
        StateMachine.SwitchState(StateType.Idle);
    }
}