namespace Framework.Core
{
	public class SceneInstaller : AbstractInstaller
	{
		public override void InstallBindings(DiContainer diContainer)
		{
			diContainer.Bind<UIManager>().FromScene();
		}
	}	
}