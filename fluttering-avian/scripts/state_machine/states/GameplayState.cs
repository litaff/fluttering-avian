namespace fluttering_avian.state_machine.states;

using character_controller;
using input_manager;
using obstacles;

public class GameplayState : BaseCoreState
{
    private readonly CharacterController character;
    private readonly ObstacleManager obstacleManager;

    public GameplayState(RuntimeData runtimeData, InputManager inputManager, CharacterController characterController,
        ObstacleManager obstacleManager) : base(runtimeData, inputManager, StateType.Gameplay)
    {
        character = characterController;
        this.obstacleManager = obstacleManager;
    }

    public override void OnEnter()
    {
        InputManager.OnJumpPressed += OnFirstInputHandler;
        base.OnEnter();
    }

    public override void OnExit()
    {
        InputManager.OnJumpPressed -= OnFirstInputHandler;
        InputManager.OnJumpPressed -= OnInputHandler;
        obstacleManager.StopSpawning();
        base.OnExit();
    }

    private void OnFirstInputHandler()
    {
        InputManager.OnJumpPressed -= OnFirstInputHandler;
        
        character.Freeze = false;
        obstacleManager.OnCharacterCollision += OnCharacterCollisionHandler;
        obstacleManager.OnCharacterExit += OnCharacterExitHandler;
        obstacleManager.StartSpawning();
        character.RequestJump();
        InputManager.OnJumpPressed += OnInputHandler;
    }

    private void OnCharacterExitHandler()
    {
        RuntimeData.AddScore(1);   
    }

    private void OnCharacterCollisionHandler()
    {
        StateMachine.SwitchState(StateType.GameplaySummary);
    }

    private void OnInputHandler()
    {
        character.RequestJump();
    }
}