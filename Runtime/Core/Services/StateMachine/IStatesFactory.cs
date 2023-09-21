namespace Framework.StateMachine
{
	public interface IStatesFactory
	{
		T Create<T>() where T : IState;
	}
}