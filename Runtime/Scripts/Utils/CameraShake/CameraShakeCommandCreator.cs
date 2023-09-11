using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace Framework.Shake
{
	/// <summary>
	/// Use it to shake camera with a-synchronized query or from an animation event.
	/// <para>To use it with animation event, Call ShakeCamera with CameraShakeConfig object.</para>
	/// </summary>
	public class CameraShakeCommandCreator : MonoBehaviour
	{
		[Serializable]
		private class CameraShakeRequest : ICloneable
		{
			[FormerlySerializedAs ("time")] public float delay;

			public CameraShakeConfig config;

			public object Clone()
			{
				return new CameraShakeRequest { delay = delay, config = config };
			}
		}

		[Header("You dont need to order the requests by delay.")]
		[SerializeField] private CameraShakeRequest[] shakeRequests = Array.Empty<CameraShakeRequest>();
		private CameraShakeRequest[] bakedShakes;
		
		private float startTime;
		private bool isInitialized;
		
		private IShakeHandler handler;

		[Inject]
		private void Construct(IShakeHandler handler)
		{
			this.handler = handler;
		}

		private void OnEnable ()
		{
			//! this can be a pooled object. reset each delay when it gets enabled.
			startTime = Time.time;

			if (isInitialized)
			{
				//! if the baking is done before, no need to reallocate it.
				for (int i = 0, length = bakedShakes.Length; i < length; i++)
				{
					bakedShakes[i].delay = shakeRequests[i].delay;
					bakedShakes[i].config = shakeRequests[i].config;
				}
			} else
			{
				bakedShakes = shakeRequests.
					Select (x=>(CameraShakeRequest)x.Clone ()).
					OrderBy(x => x.delay).
					ToArray();

				isInitialized = bakedShakes != null || bakedShakes.Length > 0;
			}
		}
		
		private void Update()
		{
			var camInstance = handler;
			
			if (!isInitialized || camInstance == null)
			{
				return;
			}

			var time = Time.time - startTime;

			var target = bakedShakes.FirstOrDefault(x => x.delay >= 0);

			if (target == null)
			{
				//! no tasks.
				return;
			}
			
			if (target.delay > time)
			{
				return;
			}

			target.delay = -1;

			camInstance.Shake(target.config, transform.position, this);
		}

		[Preserve]
		public void ShakeCamera(CameraShakeConfig configAsset)
		{
			var camInstance = handler;
						
			if (camInstance == null)
			{
				return;
			}
			
			camInstance.Shake(configAsset, transform.position);
		}
	}
}