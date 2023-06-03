using Framework.Core;
using UnityEngine;

public abstract class AbstractInstaller : ScriptableObject
{
	public abstract void InstallBindings(DiContainer diContainer);
}