namespace fluttering_avian.state_machine.states;

using character_controller;
using Godot;
using GodotTask;
using input_manager;
using view.gameplay_summary;
using ViewManager;

public class GameplaySummeryState : BaseCoreState
{
    private CharacterController character;
    private GameplaySummaryStateView view;

    public GameplaySummeryState(ViewManager viewManager, RuntimeData runtimeData, InputManager inputManager,
        CharacterController characterController) : base(viewManager, runtimeData, inputManager,
        StateType.GameplaySummary)
    {
        character = characterController;
    }

    public override async void OnEnter()
    {
        await GodotTask.WaitUntil(() => !RuntimeData.ObstacleSpawned);
        view = await ViewManager.GetView<GameplaySummaryStateView>();
        view.UpdateView(RuntimeData.Score);
        view.OnRestartButtonPressed += OnRestartButtonPressedHandler;
    }

    public override void OnExit()
    {
        RuntimeData.ResetScore();
        character.Freeze = true;
        PhysicsServer3D.BodySetState(character.GetRid(), PhysicsServer3D.BodyState.Transform, Vector3.Zero);
        base.OnExit();
    }

    private void OnRestartButtonPressedHandler()
    {
        view.OnRestartButtonPressed -= OnRestartButtonPressedHandler;
        StateMachine.SwitchState(StateType.Gameplay);
    }
}