namespace fluttering_avian.state_machine.states;

using character_controller;
using input_manager;
using obstacles;
using view.gameplay;
using ViewManager;

public class GameplayState : BaseCoreState
{
    private readonly CharacterController character;
    private readonly ObstacleManager obstacleManager;
    private GameplayStateView view;

    public GameplayState(ViewManager viewManager, RuntimeData runtimeData, InputManager inputManager,
        CharacterController characterController, ObstacleManager obstacleManager) : base(viewManager, runtimeData,
        inputManager, StateType.Gameplay)
    {
        character = characterController;
        this.obstacleManager = obstacleManager;
    }

    public override async void OnEnter()
    {
        view = await ViewManager.GetView<GameplayStateView>();
        InputManager.OnJumpPressed += OnFirstInputHandler;
        base.OnEnter();
    }

    public override async void OnExit()
    {
        InputManager.OnJumpPressed -= OnFirstInputHandler;
        InputManager.OnJumpPressed -= OnInputHandler;
        obstacleManager.OnCharacterCollision -= OnCharacterCollisionHandler;
        obstacleManager.OnCharacterExit -= OnCharacterExitHandler;
        await obstacleManager.StopSpawning().ContinueWith(_ => RuntimeData.ObstacleSpawned = false);
        base.OnExit();
    }

    private void OnFirstInputHandler()
    {
        InputManager.OnJumpPressed -= OnFirstInputHandler;
        
        character.Freeze = false;
        obstacleManager.OnCharacterCollision += OnCharacterCollisionHandler;
        obstacleManager.OnCharacterExit += OnCharacterExitHandler;
        RuntimeData.ObstacleSpawned = true;
        obstacleManager.StartSpawning();
        character.RequestJump();
        InputManager.OnJumpPressed += OnInputHandler;
    }

    private void OnCharacterExitHandler()
    {
        RuntimeData.AddScore(1);
        view.UpdateScore(RuntimeData.Score);
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