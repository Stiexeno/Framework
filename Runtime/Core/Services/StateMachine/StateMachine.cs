using UnityEngine;

namespace Framework.StateMachine
{
	public abstract class StateMachine : IStateMachine, IProcessable, IFixedProcessable
	{
		private IStatesFactory statesFactory;

		private IState activeState;

		public StateMachine(IStatesFactory statesFactory)
		{
			this.statesFactory = statesFactory;
		}

		public void Process(in float deltaTime) => (activeState as IProcessable)?.Process(deltaTime);
		public void FixedProcess(in float deltaTime) => (activeState as IFixedProcessable)?.FixedProcess(deltaTime);

		public void Enter<TState>() where TState : IState
		{
			IState state = ChangeActiveStateTo<TState>();
			(state as IEnter)?.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : IState, IPayloadedEnter<TPayload>
		{
			IState state = ChangeActiveStateTo<TState>();
			(state as IPayloadedEnter<TPayload>)?.Enter(payload);
		}

		public void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2)
			where TState : IState, IPayloadedEnter<TPayload1, TPayload2>
		{
			IState state = ChangeActiveStateTo<TState>();
			(state as IPayloadedEnter<TPayload1, TPayload2>)?.Enter(payload1, payload2);
		}

		public void Enter<TState, TPayload1, TPayload2, TPayload3>(TPayload1 payload1, TPayload2 payload2, TPayload3 payload3)
			where TState : IState, IPayloadedEnter<TPayload1, TPayload2, TPayload3>
		{
			IState state = ChangeActiveStateTo<TState>();
			(state as IPayloadedEnter<TPayload1, TPayload2, TPayload3>)?.Enter(payload1, payload2, payload3);
		}

		private IState ChangeActiveStateTo<TState>() where TState : IState
		{
			(activeState as IExit)?.Exit();

			activeState = statesFactory.Create<TState>();

			Debug.Log($"<color=green>{GetType().Name}</color> switched to <color=cyan>{typeof(TState).Name}</color>");

			return activeState;
		}
	}	
}