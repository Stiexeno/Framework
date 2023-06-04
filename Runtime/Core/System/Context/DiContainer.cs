using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
	public class DiContainer : IInstantiator, IResolver
	{
		public Dictionary<Type, Binding> Container { get; }

		private readonly IResolver resolver;
		private readonly IInstantiator instantiator;
		private readonly IBinder binder;

		private readonly List<DiContainer> parents;

		public DiContainer(List<DiContainer> parents = null)
		{
			this.parents = parents ?? new List<DiContainer>();
			Container = new Dictionary<Type, Binding>();
			binder = new Binder(Container, this);
			instantiator = new Instantiator(this);
			resolver = new Resolver(this, this);
			
			foreach (var parent in this.parents)
			{
				foreach ((Type type, Binding binding) in parent.Container)
				{
					Container.Add(type, binding);
				}
			}

			Unbind<IInstantiator>();
			Unbind<IResolver>();
			Bind<IResolver>().FromInstance(this);
			Bind<IInstantiator>().FromInstance(this);
		}

		public DiContainer CreateSubContainer() => new DiContainer(new List<DiContainer>(parents) { this });

		// Binder

		public ContractBinder<TContract> Bind<TContract>() => binder.Bind<TContract>();
		public void Unbind<TContract>() => binder.Unbind<TContract>();
		public void BindConfigs() => binder.BindConfigs();

		// Instantiator

		public T Instantiate<T>() => instantiator.Instantiate<T>();
		public object Instantiate(Type bindingConcreteType) => instantiator.Instantiate(bindingConcreteType);
		public T Instantiate<T>(params object[] args) => instantiator.Instantiate<T>(args);
		public GameObject InstantiatePrefab(GameObject original) => instantiator.InstantiatePrefab(original);

		public void InjectToSceneGameObjects() => instantiator.InjectToSceneGameObjects();

		// Resolver

		public T Resolve<T>() => resolver.Resolve<T>();
		public object Resolve(Type contractType) => resolver.Resolve(contractType);
		public T FindInScene<T>() where T : MonoBehaviour => resolver.FindInScene<T>();
	}
}