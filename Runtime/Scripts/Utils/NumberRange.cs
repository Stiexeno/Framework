using System;
using UnityEngine;

namespace Framework.Utils
{
	[Serializable]
	public struct IntMinMax
	{
		public int min;
		public int max;
		
		public IntMinMax(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
		
		/// <summary>
		/// Returns Random between min and max (both inclusive)
		/// </summary>
		/// <returns></returns>
		public int Random()
		{
			return UnityEngine.Random.Range(min, max);
		}

		public int Lerp(float time)
		{
			return (int)Mathf.Lerp(min, max, time);
		}
	}
	
	[Serializable]
	public struct FloatMinMax
	{
		public float min;
		public float max;
		
		public FloatMinMax(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
		
		/// <summary>
		/// Returns Random between min and max (both inclusive)
		/// </summary>
		/// <returns></returns>
		public float Random()
		{
			return UnityEngine.Random.Range(min, max);
		}
		
		public float Lerp(float time)
		{
			return Mathf.Lerp(min, max, time);
		}
	}
}