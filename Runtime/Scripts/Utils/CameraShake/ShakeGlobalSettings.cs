using Framework.Core;
using UnityEngine;

namespace Framework.Shake
{
	public class ShakeGlobalSettings : ResourcesGlobalSetting<ShakeGlobalSettings>
	{
		[SerializeField] private float maximumShakeMagnitude = 5;

		public static float MaximumShakeMagnitude => Active.maximumShakeMagnitude;
	}
}
