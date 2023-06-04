using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Core
{
    public class Binder : IBinder
    {
        private readonly Dictionary<Type, Binding> container;
        
        private IInstantiator instantiator;
        private ConfigSettings configSettings;

        public Binder(Dictionary<Type, Binding> container, IInstantiator instantiator)
        {
            this.instantiator = instantiator;
            this.container = container;
        }

        public ContractBinder<T> Bind<T>()
        {
           EnsureThatDependencyNotRegistered<T>();

           var binding = new Binding();
           
           if (typeof(T).IsClass)
           {
               binding.ContractType = typeof(T);   
               binding.Interfaces = typeof(T).Interfaces();
           }
           
           container.Add(typeof(T), binding);

           return new ContractBinder<T>(binding, instantiator);
        }

        public void BindConfigs()
        {
            if (configSettings == null)
            {
                configSettings = ConfigSettings.Settings;

                if (configSettings == null)
                {
                    throw new NullReferenceException("ConfigProvider is not found. Please create ConfigProvider asset in Resources folder.");
                }
            }
			
            foreach (var config in configSettings.configs)
            {
                EnsureThatDependencyNotRegistered(config.GetType());
                
                var binding = new Binding();
                
                binding.ContractType = config.GetType();
                binding.Interfaces = config.GetType().Interfaces();
                binding.Instance = config;
                
                container.Add(config.GetType(), binding);
            }
        }

        public void Unbind<T>()
        {
            container.Remove(typeof(T));
        }
        
        private void EnsureThatDependencyNotRegistered<T>()
        {
            EnsureThatDependencyNotRegistered(typeof(T));
        }
        
        private void EnsureThatDependencyNotRegistered(Type type)
        {
            if (container.ContainsKey(type))
                throw new NullReferenceException($"Dependency of type {type} is already registered as single.");
        }
    }
}
