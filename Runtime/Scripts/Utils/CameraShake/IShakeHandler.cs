using Framework.Shake;
using UnityEngine;

namespace Framework
{
	public interface IShakeHandler
	{
		public void Shake(CameraShakeConfig config, float power, Behaviour activator = null);
		public void Shake(CameraShakeConfig config, Vector3 position, Behaviour activator = null);
	}
}