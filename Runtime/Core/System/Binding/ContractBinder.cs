using Object = UnityEngine.Object;

namespace Framework.Core
{
    public class ContractBinder<TContract> : AbstractBinder
    {
        private readonly IInstantiator instantiator;

        public ContractBinder(Binding binding, IInstantiator instantiator) : base(binding)
        {
            this.instantiator = instantiator;
        }

        public TContract FromInstance<TConcrete>(TConcrete instance) where TConcrete : TContract
        {
            binding.Instance = instance;
            binding.ContractType = typeof(TConcrete);
            
            binding.Interfaces = typeof(TConcrete).Interfaces(); 
            
            return (TContract)binding.Instance;
        }
        
        public NonLazyBinder NonLazy()
        {
            return new NonLazyBinder(binding, instantiator);
        }
        
        public TContract FindInScene()
        {
            object targetObject = Object.FindObjectOfType(typeof(TContract), true);
            
            if (targetObject == null)
                Context.Exception($"Object of type {typeof(TContract).Name} not found in scene", 
                    $"Create {typeof(TContract).Name} in scene or add binding to Installer");
                
            return FromInstance((TContract)targetObject);
        }
    }
}
