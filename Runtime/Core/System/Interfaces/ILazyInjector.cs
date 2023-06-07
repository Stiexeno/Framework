using System.Collections.Generic;

namespace Framework.Core
{
	public interface ILazyInjector
	{
		void Inject(object instance);
		void Inject(IEnumerable<object> instances);
		void InjectAll();
		internal void QueueToInject(object instance);
	}
}