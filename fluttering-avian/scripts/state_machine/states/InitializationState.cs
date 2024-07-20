namespace fluttering_avian.state_machine.states;

using Godot;

[GlobalClass]
public partial class InitializationState : BaseCoreState
{
    public override StateType StateType => StateType.Initialization;
}