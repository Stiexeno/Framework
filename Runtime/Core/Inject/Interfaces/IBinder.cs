namespace Framework.Core
{
    public interface IBinder
    {
        ContractBinder<TContract> Bind<TContract>();
        void Unbind<TContract>();
        void BindConfigs();
    }
}
