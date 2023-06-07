namespace Framework.Core
{
	public static class DIStateMachine
	{
		public static IDIState CurrentState { get; private set; }
		
		public static void TransitionToState(IDIState state)
		{
			if (CurrentState != null)
			{
				CurrentState.ExitState();
			}
			
			CurrentState = state;
			CurrentState.EnterState();
		}
	}	
}