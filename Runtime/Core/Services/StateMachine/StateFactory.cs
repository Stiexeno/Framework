namespace Framework.StateMachine
{
	public class StateFactory : IStatesFactory
	{
		private readonly IInstantiator _instantiator;

		public StateFactory(IInstantiator instantiator)
		{
			_instantiator = instantiator;
		}

		public T Create<T>() where T : IState => _instantiator.Instantiate<T>();
	}
}