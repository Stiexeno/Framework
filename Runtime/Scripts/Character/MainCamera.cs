using Framework.Core;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public class MainCamera : MonoBehaviour, ICamera
	{
		[SF] protected Transform target;
		[SF] private float smoothTime = 0.3f;
		[SF] private Vector3 extraOffset;

		private Vector3 initialOffset;
		private Vector3 velocity = Vector3.zero;
		
		public Camera Camera { get; set; }

		public void SetSmoothTime(float value)
		{
			smoothTime = value;
		}

		public void SetTarget(Transform target)
		{
			this.target = target;

			initialOffset = transform.position - target.position;
		}

		public void SetOffset(Vector3 offset)
		{
			extraOffset = offset;
		}

		protected virtual void Awake()
		{
			if (target != null)
				initialOffset = transform.position - target.position;
		}

		protected virtual void LateUpdate()
		{
			if (target != null)
			{
				var targetPosition = target.position + initialOffset + extraOffset;
				transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
			}
		}
	}
}