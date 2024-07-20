namespace fluttering_avian.state_machine;

using Godot;
using logger;
using states;

[GlobalClass]
public partial class StateMachineHandler : Resource
{
    [Export]
    private InitializationState initializationState;
    [Export]
    private IdleState idleState;
    [Export]
    private GameplayState gameplayState;
    [Export]
    private GameplaySummeryState gameplaySummeryState;
    
    private StateMachine stateMachine;

    public void Initialize()
    {
        stateMachine = new StateMachine(new GodotLogger(), initializationState, idleState, gameplayState,
            gameplaySummeryState);
        stateMachine.Run(StateType.Initialization);
    }
}