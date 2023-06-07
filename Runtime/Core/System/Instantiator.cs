using System;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
	public class Instantiator : IInstantiator
	{
		private readonly IResolver resolver;
		private readonly DiContainer diContainer;

		public Instantiator(IResolver resolver)
		{
			this.resolver = resolver;
			this.diContainer = resolver as DiContainer;
		}

		public T Instantiate<T>() => (T)Instantiate(typeof(T));

		public object Instantiate(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(MonoBehaviour))
				? InstantiateMonoBehaviour(objectType)
				: InstantiateNonMonoBehaviour(objectType);
		}
		
		object IInstantiator.Instantiate(Binding binding)
		{
			var instance = Instantiate(binding.ContractType);
			
			if (instance.GetType().IsSubclassOf(typeof(MonoBehaviour)))
			{
				if (binding.DontDestroyOnLoad)
				{
					var gameObject = (instance as MonoBehaviour).gameObject;
					gameObject.transform.SetParent(null);
					Object.DontDestroyOnLoad(gameObject);
				}
			}

			return instance;
		}

		public T Instantiate<T>(params object[] args)
		{
			return (T)InstantiateNonMonoBehaviour(typeof(T), args);
		}

		private object InstantiateNonMonoBehaviour(Type objectType, params object[] constructorArgs)
		{
			var constructors = objectType.GetConstructors();

			if (constructors.Length <= 0)
			{
				return Activator.CreateInstance(objectType);
			}

			var args = constructors[0].GetParameters();
			var argsToInject = new object[args.Length];

			for (int i = 0; i < args.Length; i++)
			{
				var resolvedArg = resolver.Resolve(args[i].ParameterType);

				if (resolvedArg != null)
				{
					argsToInject[i] = resolvedArg;
					continue;
				}

				if (constructorArgs.Length <= i)
				{
					Context.Exception(
						$"Can't inject {args[i].ParameterType.Name} to {objectType.Name}",
						$"Try to refresh Configs or check if you have added {args[i].ParameterType.Name} to the Container");
				}

				argsToInject[i] = constructorArgs[i];
			}

			return Activator.CreateInstance(objectType, argsToInject);
		}

		private object InstantiateMonoBehaviour(Type objectType)
		{
			var gameObject = new GameObject(objectType.Name);

			if (Context.SceneContext != null)
			{
				gameObject.transform.SetParent(Context.SceneContext.transform);
			}

			Component component = gameObject.AddComponent(objectType);

			diContainer.Inject(new List<MonoBehaviour> { component as MonoBehaviour });
			return component;
		}

		public GameObject InstantiatePrefab(GameObject original)
		{
			GameObject clone = UnityEngine.Object.Instantiate(original);
			var components = clone.GetComponentsInChildren<MonoBehaviour>();

			diContainer.Inject(components);

			return clone;
		}
	}
}