namespace Framework.Core
{
    public interface IContainerFactory
    {
        void CreateContainer(DiContainer overrideContainer = null);
    }
}
