namespace Framework.Core
{
	public abstract class AbstractBinder
	{
		internal readonly Binding binding;

		public AbstractBinder(Binding binding)
		{
			this.binding = binding;
		}
	}
}