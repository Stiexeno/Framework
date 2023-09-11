using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Shake
{
	public class ShakeHandler : MonoBehaviour, IShakeHandler
	{
		//Serialized fields
		
		[SF] private float shakeAdditionLerpSpeed = 5;
		
		// Private fields
		
		private Vector3 defaultCamPos;
		
		private readonly ShakeBuilder shakeBuilder = new ShakeBuilder();
		
		public void Shake(CameraShakeConfig config, float power, Behaviour activator = null)
		{
			power = config.powerBoostCurve.Evaluate(power);
			
			var shakeCommand = new ShakeCommand(
				activator,
				config.strength * power, 
				config.duration, 
				config.vibration, 
				config.frequencyCurve, 
				config.shakerCurve,
				config.strengthRandomizer * power,
				config.ignoreGlobalCap);
			
			shakeBuilder.AddCommand(shakeCommand);
		}

		public void Shake(CameraShakeConfig config, Vector3 position, Behaviour activator = null)
		{
			if (config == null)
				return;
			
			var distance = Vector3.Distance(transform.position, position);
			
			Shake(config, distance, activator);
		}

		private void Awake()
		{
			defaultCamPos = transform.localPosition;
		}

		private void LateUpdate()
		{
			var shakeAddition = shakeBuilder.Run();
			var localCameraPosition = transform.localPosition;
			transform.localPosition =
				Vector3.Lerp(localCameraPosition,
					defaultCamPos + shakeAddition, 
					Time.deltaTime * shakeAdditionLerpSpeed);
		}
	}
}