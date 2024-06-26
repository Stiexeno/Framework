using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Utils;
using UnityEngine;

namespace Framework.Core
{
	public class LazyInjector : ILazyInjector
	{
		private readonly DiContainer diContainer;
		private readonly HashSet<object> instancesToInject = new HashSet<object>();

		public LazyInjector(DiContainer diContainer)
		{
			this.diContainer = diContainer;
		}

		public void Inject(object instance)
		{
			var existingBinding = diContainer.Container.FirstOrDefault(x => x.Key == instance.GetType()).Value;

			if (existingBinding != null && existingBinding.Initialized)
				return;

			//var injectMethods = instance
			//	.GetType()
			//	.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
			//	.Where(HasInjectMethods)
			//	.ToList();

			var injectableMethods = InjectExtensions.GetInjectableMethods(instance.GetType());

			foreach (MethodInfo injectMethod in injectableMethods)
			{
				var args = injectMethod.GetParameters();
				var argsToInject = new object[args.Length];

				for (int i = 0; i < args.Length; i++)
				{
					argsToInject[i] = diContainer.Resolve(args[i].ParameterType);

					if (argsToInject[i] == null)
					{
						var parameter = $"{args[i].ParameterType}".SetColor(new Color(0.64f, 0.87f, 0.18f));
						Context.Exception($"Failed to inject {parameter} into {instance.GetType()}", $"Make sure that you added binding to installer");
					}
				}
				
					
				injectMethod.Invoke(instance, argsToInject);
			}
		}
		
		public void Inject(IEnumerable<object> instances)
		{
			foreach (var instance in instances)
			{
				Inject(instance);
			}
		}
		
		private void LazyInject(object instance)
		{
			if (instancesToInject.Remove(instance))
			{ 
				Inject(instance);
			}
		}

		void ILazyInjector.QueueToInject(object instance)
		{
			instancesToInject.Add(instance);
		}

		public void InjectAll()
		{
			var tempList = new List<object>();

			while (instancesToInject.Count > 0)
			{
				tempList.Clear();
				tempList.AddRange(instancesToInject);

				foreach (var instance in tempList)
				{
					LazyInject(instance);
				}
			}
		}
	}
}