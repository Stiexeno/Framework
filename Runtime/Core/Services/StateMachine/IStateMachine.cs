namespace Framework.StateMachine
{
	public interface IStateMachine
	{
		void Enter<TState>() where TState : IState;
        
		void Enter<TState, TPayload>(TPayload payload) where TState : IState, IPayloadedEnter<TPayload>;

		void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2)
			where TState : IState, IPayloadedEnter<TPayload1, TPayload2>;
        
		void Enter<TState, TPayload1, TPayload2, TPayload3>(TPayload1 payload1, TPayload2 payload2, TPayload3 payload3)
			where TState : IState, IPayloadedEnter<TPayload1, TPayload2, TPayload3>;
	}
}