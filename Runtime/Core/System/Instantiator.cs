using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework
{
	public class Instantiator : IInstantiator
	{
		private readonly IResolver resolver;

		public Instantiator(IResolver resolver)
		{
			this.resolver = resolver;
		}
		
		public T Instantiate<T>() => (T)Instantiate(typeof(T));

		public object Instantiate(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(MonoBehaviour))
				? InstantiateMonoBehaviour(objectType)
				: InstantiateNonMonoBehaviour(objectType);
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
				try
				{
					var resolvedArg = resolver.Resolve(args[i].ParameterType);
				
					if (resolvedArg != null)
					{
						argsToInject[i] = resolvedArg;
						continue;
					}
				
					argsToInject[i] = constructorArgs[i];
				}
				catch (Exception e)
				{
					Debug.LogError($"{objectType} => {args[i].ParameterType} => {constructorArgs[i]} => {e}");
				}
			}
			
			return Activator.CreateInstance(objectType, argsToInject);
		}
		
		private object InstantiateMonoBehaviour(Type objectType)
		{
			var gameObject = new GameObject(objectType.Name);
			Component component = gameObject.AddComponent(objectType);

			InjectInto(new List<MonoBehaviour> {component as MonoBehaviour});
			return component;
		}

		private void InjectInto(IEnumerable<MonoBehaviour> components)
		{
			foreach (var component in components)
			{
				var injectMethods = component
					.GetType()
					.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
					.Where(HasInjectMethods)
					.ToList();

				foreach (MethodInfo injectMethod in injectMethods)
				{
					var args = injectMethod.GetParameters();
					var argsToInject = new object[args.Length];
					
					for (int i = 0; i < args.Length; i++)
						argsToInject[i] = resolver.Resolve(args[i].ParameterType);
					
					injectMethod.Invoke(component, argsToInject);
				}
			}
		}
		
		public GameObject InstantiatePrefab(GameObject original)
		{
			GameObject clone = UnityEngine.Object.Instantiate(original);
			var components = clone.GetComponentsInChildren<MonoBehaviour>();

			InjectInto(components);

			return clone;
		}
		
		public void InjectToSceneGameObjects()
		{
			var sceneGameObjects = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true);
			
			InjectInto(sceneGameObjects);
		}

		// Helpers
		
		private static bool HasInjectMethods(MethodInfo methodInfo) =>
			methodInfo.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0;
	}
}