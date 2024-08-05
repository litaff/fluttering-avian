namespace fluttering_avian.state_machine;

using global::StateMachine;
using Logger;

public class StateMachine : StateMachine<StateType>
{
    public StateMachine(ILogger logger) : base(logger)
    {
    }

    public StateMachine(ILogger logger, params IState<StateType>[] states) : base(logger, states)
    {
    }
}