namespace fluttering_avian.state_machine.states;

using Godot;

[GlobalClass]
public partial class IdleState : BaseCoreState
{
    public override StateType StateType => StateType.Idle;
}