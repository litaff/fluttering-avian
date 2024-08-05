namespace fluttering_avian.state_machine.states;

using input_manager;
using view.idle;
using ViewManager;

public class IdleState : BaseCoreState
{
    private IdleStateView view;
    
    public IdleState(ViewManager viewManager, RuntimeData runtimeData, InputManager inputManager) : base(viewManager,
        runtimeData, inputManager, StateType.Idle)
    {

    }

    public override async void OnEnter()
    {
        view = await ViewManager.GetView<IdleStateView>();
        
        view.OnStartButtonPressed += OnStartButtonPressedHandler;
    }

    private void OnStartButtonPressedHandler()
    {
        view.OnStartButtonPressed -= OnStartButtonPressedHandler;
        StateMachine.SwitchState(StateType.Gameplay);
    }
}