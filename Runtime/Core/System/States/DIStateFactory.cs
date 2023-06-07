using System;

namespace Framework.Core
{
	public enum DIState
	{
		Binding,
		Creation,
		Resolving,
		Injection
	}
	
	public class DIStateFactory : IDIStateFactory
	{
		public IDIState CreateState(DIState state)
		{
			switch (state)
			{
				case DIState.Binding:
					return new BindingState();
				case DIState.Creation:
					break;
				case DIState.Resolving:
					break;
				case DIState.Injection:
					break;
			}

			throw new ArgumentException("Invalid state");
		}
	}
}