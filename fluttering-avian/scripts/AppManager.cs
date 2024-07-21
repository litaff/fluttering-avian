namespace fluttering_avian;

using character_controller;
using Godot;
using input_manager;
using logger;
using Logger;
using obstacles;
using state_machine;
using state_machine.states;

public partial class AppManager : Node
{
    [Export]
    private ObstacleManager obstacleManager;
    [Export]
    private CharacterController characterController;
    
    private ILogger logger;
    private InputManager inputManager;
    private RuntimeData runtimeData;

    #region State Machine

    private StateMachine stateMachine;
    private InitializationState initializationState;
    private IdleState idleState;
    private GameplayState gameplayState;
    private GameplaySummeryState gameplaySummeryState;

    #endregion
    
    public override void _Ready()
    {
        base._Ready();
        
        logger = new GodotLogger();
        inputManager = new InputManager();
        runtimeData = new RuntimeData();

        InitializeStateMachine();

        stateMachine.Run(StateType.Initialization);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        inputManager.HandleInputEvent(@event);
    }

    private void InitializeStateMachine()
    {
        initializationState = new InitializationState(runtimeData, inputManager);
        idleState = new IdleState(runtimeData, inputManager);
        gameplayState = new GameplayState(runtimeData, inputManager, characterController, obstacleManager);
        gameplaySummeryState = new GameplaySummeryState(runtimeData, inputManager, characterController);
        stateMachine = new StateMachine(logger, initializationState, idleState, gameplayState,
            gameplaySummeryState);
    }
}