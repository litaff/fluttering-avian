namespace fluttering_avian.state_machine.states;

using input_manager;
using ViewManager;

public class InitializationState : BaseCoreState
{
    public InitializationState(ViewManager viewManager, RuntimeData runtimeData, InputManager inputManager) : base(
        viewManager, runtimeData, inputManager, StateType.Initialization)
    {
    }

    public override void OnEnter()
    {
        StateMachine.SwitchState(StateType.Idle);
    }
}