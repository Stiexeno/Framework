namespace Framework.Core
{
    public class BootstrapInstaller : AbstractInstaller
    {
        public override void InstallBindings(DiContainer diContainer)
        {
            // Core
            
            diContainer.Bind<FocusManager>().Instantiate(InstantiateArgs.DontDestroyOnLoad);
            diContainer.Bind<SceneManager>().Instantiate(InstantiateArgs.DontDestroyOnLoad);
            diContainer.Bind<CoroutineManager>().Instantiate(InstantiateArgs.DontDestroyOnLoad);
            
            diContainer.Bind<DataManager>();
            diContainer.Bind<EventManager>();
            diContainer.Bind<Timer>();
            diContainer.Bind<LocalClock>().Instantiate();
            diContainer.Bind<UIManager>().FindInScene<UIManager>();
            
            // Services
            
            diContainer.Bind<Wallet>().Instantiate();
        }
    }
}
