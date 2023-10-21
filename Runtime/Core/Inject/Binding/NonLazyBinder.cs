namespace Framework.Core
{
	public class NonLazyBinder : AbstractBinder
	{
		private IInstantiator instantiator;

		public NonLazyBinder(Binding binding, IInstantiator instantiator) : base(binding)
		{
			this.instantiator = instantiator;
			Instantiate();
		}

		private void Instantiate()
		{
			binding.Instance = instantiator.Instantiate(binding.ContractType);
		}
	}
}