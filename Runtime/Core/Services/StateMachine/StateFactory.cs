using UnityEngine;

namespace Framework.StateMachine
{
	public class StateFactory : IStatesFactory
	{
		private readonly InstantiatorProvider instantiatorProvider;

		public StateFactory(InstantiatorProvider instantiatorProvider)
		{
			this.instantiatorProvider = instantiatorProvider;
		}

		public T Create<T>() where T : IState => instantiatorProvider.instantiator.Instantiate<T>();
	}
}