using UnityEngine;

namespace Framework.Core
{
	public static class BinderExtensions
	{
		public static void DontDestroyOnLoad(this AbstractBinder binder)
		{
			binder.binding.DontDestroyOnLoad = true;
			
			if (binder.binding.Instance == null)
				return;
			
			Object.DontDestroyOnLoad(binder.binding.Instance as Object);
		}
	}
}