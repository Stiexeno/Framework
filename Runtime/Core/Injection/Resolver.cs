using System;
using UnityEngine;

namespace Framework.Core
{
    public class Resolver : IResolver
    {
        private readonly DiContainer diContainer;
        private readonly IInstantiator instantiator;

        public Resolver(DiContainer diContainer, IInstantiator instantiator)
        {
            this.instantiator = instantiator;
            this.diContainer = diContainer;
        }

        public T Resolve<T>() => (T)Resolve(typeof(T));

        public object Resolve(Type objectType)
        {
            var container = diContainer.Container;
            
            foreach (var cachedBinding in container)
            {
                if (cachedBinding.Value.ContractType == null)
                    continue;
                
                if (cachedBinding.Value.Interfaces == null)
                    continue;

                foreach (var bindingInterface in cachedBinding.Value.Interfaces)
                {
                    if (objectType.IsAssignableFrom(bindingInterface))
                    {
                        return internalResolve(cachedBinding.Value);
                    }
                }
            }

            if (container.ContainsKey(objectType) == false)
                return null;

            if (container[objectType].ContractType == null)
                return null;
            
            return internalResolve(container[objectType]);

            object internalResolve(Binding binding)
            {
                if (binding.Instance != null)
                    return binding.Instance;

                binding.Instance = instantiator.Instantiate(binding.ContractType);
                return binding.Instance;
            }
        }

        public T FindInScene<T>() where T: MonoBehaviour
        {
            var targetObject = UnityEngine.Object.FindObjectOfType<T>(true);

            if (targetObject == null)
                throw new NullReferenceException($"Object of type {typeof(T)} not found in scene");
                
            return targetObject;
        }
    }
}
