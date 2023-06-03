using System;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Utils
{
	[Serializable]
	public struct AnimationArgs
	{
		public float duration;
		public AnimationCurve curve;
	}	
}