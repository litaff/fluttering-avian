namespace fluttering_avian;

using Godot;
using state_machine;

public partial class AppManager : Node
{
    [Export]
    private StateMachineHandler stateMachineHandler;
    
    public override void _Ready()
    {
        base._Ready();
        stateMachineHandler.Initialize();
    }
}