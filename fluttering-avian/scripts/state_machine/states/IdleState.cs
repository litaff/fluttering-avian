namespace fluttering_avian.state_machine.states;

using input_manager;

public class IdleState : BaseCoreState
{
    public IdleState(RuntimeData runtimeData, InputManager inputManager) : base(runtimeData, inputManager,
        StateType.Idle)
    {
    }

    public override void OnEnter()
    {
        // TODO: Attach this to UI button.
        StateMachine.SwitchState(StateType.Gameplay);
    }
}