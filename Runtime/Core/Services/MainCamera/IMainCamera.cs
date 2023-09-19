using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public interface ICamera
	{
		public Camera Camera { get; set; }
		void SetTarget(Transform target);
	}	
}