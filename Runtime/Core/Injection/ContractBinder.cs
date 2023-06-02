using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public enum InstantiateArgs {None, DontDestroyOnLoad}
    public class ContractBinder<TContract>
    {
        private readonly Binding binding;
        private readonly IInstantiator instantiator;

        public ContractBinder(Binding binding, IInstantiator instantiator)
        {
            this.instantiator = instantiator;
            this.binding = binding;
        }

        public TContract FromInstance<TConcrete>(TConcrete instance) where TConcrete : TContract
        {
            binding.Instance = instance;
            binding.ContractType = typeof(TConcrete);
            
            binding.Interfaces = typeof(TConcrete).Interfaces(); 
            
            return (TContract)binding.Instance;
        }

        public TContract Instantiate(InstantiateArgs args = default)
        {
            var newInstance = instantiator.Instantiate<TContract>();

            if (newInstance is MonoBehaviour monoBehaviour)
            {
                if (args == InstantiateArgs.DontDestroyOnLoad)
                {
                    Object.DontDestroyOnLoad(monoBehaviour);
                }
            }
            
            return FromInstance(newInstance);
        }
        
        //TODO: Find a better solution to get rid of <TConcrete>
        public TContract FindInScene<TConcrete>() where TConcrete : Object, TContract
        {
            var targetObject = Object.FindObjectOfType<TConcrete>(true);
            
            if (targetObject == null)
                throw new NullReferenceException($"Object of type {typeof(TContract)} not found in scene");
                
            return FromInstance(targetObject);
        }
    }
}
