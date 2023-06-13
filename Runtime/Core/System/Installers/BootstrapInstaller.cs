namespace Framework.Core
{
    public class BootstrapInstaller : AbstractInstaller
    {
        public override void InstallBindings(DiContainer diContainer)
        {
            // Core

            diContainer.Bind<DataManager>();
            diContainer.Bind<FocusManager>().DontDestroyOnLoad();
            diContainer.Bind<SceneManager>().DontDestroyOnLoad();
            diContainer.Bind<CoroutineManager>().DontDestroyOnLoad();

            diContainer.Bind<EventManager>();
            diContainer.Bind<Timer>();
            diContainer.Bind<LocalClock>().NonLazy();
            diContainer.Bind<AssetLoader>();
        }
    }
}
