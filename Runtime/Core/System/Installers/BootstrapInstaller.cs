namespace Framework.Core
{
    public class BootstrapInstaller : AbstractInstaller
    {
        public override void InstallBindings(DiContainer diContainer)
        {
            // Core

            diContainer.Bind<DataManager>();
            diContainer.Bind<FocusManager>().DontDestroyOnLoad();
            diContainer.Bind<SceneManager>();
            diContainer.Bind<CoroutineManager>();
            
            diContainer.Bind<EventManager>();
            diContainer.Bind<Timer>();
            diContainer.Bind<LocalClock>().NonLazy();
            diContainer.Bind<UIManager>().FindInScene();
            diContainer.Bind<Assets>();
            
            // Services

            diContainer.Bind<Wallet>().NonLazy();
        }
    }
}
