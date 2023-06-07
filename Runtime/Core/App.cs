using DG.Tweening;
using UnityEngine;

namespace Framework.Core
{
	internal static class App
	{
		private const int TARGET_FRAME_RATE = 60;
		private const int POOR_HARDWARE_MEMORY_SIZE = 2048;
		private const int POOR_HARDWARE_GRAPHICS_MEMORY_SIZE = 768;

		private static readonly string[] POOR_HARDWARE_CPU =
		{
			"Adreno (TM) 308", "Mali"
		};

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void BootUp()
		{
			HardwareCheck();
			
			#if UNITY_ANIMANCER
			AnimancerLayer.SetMaxStateDepth(20);
			#endif
			
			DOTween.SetTweensCapacity(100, 50);
		}

		private static void HardwareCheck()
		{
			Application.targetFrameRate = TARGET_FRAME_RATE;

			var poorCpu = false;
			string graphicsDeviceName = SystemInfo.graphicsDeviceName;

			for (int i = 0; i < POOR_HARDWARE_CPU.Length; i++)
			{
				if (graphicsDeviceName.Equals(POOR_HARDWARE_CPU[i]) || graphicsDeviceName.Contains(POOR_HARDWARE_CPU[i]))
				{
					poorCpu = true;
					break;
				}
			}

			if (SystemInfo.systemMemorySize <= POOR_HARDWARE_MEMORY_SIZE ||
			    SystemInfo.graphicsMemorySize <= POOR_HARDWARE_GRAPHICS_MEMORY_SIZE ||
			    poorCpu)
			{
				QualitySettings.SetQualityLevel(1, true);
			}
		}
	}
}