using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
	public sealed class DiContainer : IInstantiator, IResolver
	{
		public Dictionary<Type, Binding> Container { get; }

		private readonly IResolver resolver;
		private readonly IInstantiator instantiator;
		private readonly IBinder binder;
		private readonly ILazyInjector lazyInjector;

		private readonly List<DiContainer> parents;

		public DiContainer(List<DiContainer> parents = null)
		{
			this.parents = parents ?? new List<DiContainer>();
			Container = new Dictionary<Type, Binding>();
			binder = new Binder(Container, this);
			instantiator = new Instantiator(this);
			resolver = new Resolver(this, this);
			lazyInjector = new LazyInjector(this);
			
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
		internal void BindConfigs() => binder.BindConfigs();

		// Instantiator

		public T Instantiate<T>() => instantiator.Instantiate<T>();
		public object Instantiate(Type bindingConcreteType) => instantiator.Instantiate(bindingConcreteType);
		object IInstantiator.Instantiate(Binding binding)	=> instantiator.Instantiate(binding);

		public T Instantiate<T>(params object[] args) => instantiator.Instantiate<T>(args);
		public GameObject InstantiatePrefab(GameObject original) => instantiator.InstantiatePrefab(original);

		// Resolver

		public T Resolve<T>() => resolver.Resolve<T>();
		public object Resolve(Type contractType) => resolver.Resolve(contractType);
		T IResolver.FindInScene<T>() => resolver.FindInScene<T>();
		
		// Inject

		public void Inject(object instance) => lazyInjector.Inject(instance);
		public void Inject(IEnumerable<object> instances) => lazyInjector.Inject(instances);
		internal void InjectAll() => lazyInjector.InjectAll();
		internal void QueueForInject(object instance) => lazyInjector.QueueToInject(instance);
	}
}