namespace fluttering_avian.state_machine.states;

using character_controller;
using Godot;
using input_manager;

public class GameplaySummeryState : BaseCoreState
{
    private CharacterController character;
    
    public GameplaySummeryState(RuntimeData runtimeData, InputManager inputManager, CharacterController characterController) : base(runtimeData, inputManager,
        StateType.GameplaySummary)
    {
        character = characterController;
    }

    public override void OnEnter()
    {
        // TODO: Embed this in UI, switch state on button press.
        GD.Print($"Score: {RuntimeData.Score}");
        StateMachine.SwitchState(StateType.Idle);
    }
    
    public override void OnExit()
    {
        RuntimeData.ResetScore();
        character.Freeze = true;
        character.Position = Vector3.Zero;
        base.OnExit();
    }
}