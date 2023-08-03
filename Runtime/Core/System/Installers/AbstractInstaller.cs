using UnityEngine;

namespace Framework.Core
{
	public abstract class AbstractInstaller : ScriptableObject
	{
		public abstract void InstallBindings(DiContainer diContainer);
	}
}
