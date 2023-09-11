using UnityEngine;

namespace Framework.Shake
{
	public class ShakeCommand
	{
		private readonly Behaviour activator;
		
		public readonly Vector3 strengthVector;
		public readonly bool ignoreGlobalCap;

		private readonly bool hadActivator;

		private readonly float speed;
		private readonly int vibCount;
		private readonly AnimationCurve freqCurve;
		private readonly AnimationCurve shakeCurve;

		private readonly Vector3 strengthRandomizer;
		
		private int freqPoint;

		private Vector3 activeRandomizer;

		private float mMultiplier;
		
		public float Progress;

		public float Multiplier
		{
			set => mMultiplier = Mathf.Clamp(value, 0, 1);
			private get => mMultiplier;
		}

		public ShakeCommand(Behaviour activator, Vector3 strengthVector, float time, int vibCount, AnimationCurve freqCurve, AnimationCurve shakeCurve, Vector3 strengthRandomizer, bool ignoreGlobalCap = false)
		{
			this.activator = activator;
			this.ignoreGlobalCap = ignoreGlobalCap;

			hadActivator = activator != null;

			this.strengthRandomizer = strengthRandomizer;
			this.strengthVector = strengthVector;
			this.vibCount = vibCount;
			speed = 1 * vibCount / time;

			this.freqCurve = freqCurve;
			this.shakeCurve = shakeCurve;

			RandomizeStrength();
		}

		private void RandomizeStrength()
		{
			activeRandomizer = new Vector3(Random.Range(-1, 2) * strengthRandomizer.x, Random.Range(-1, 2) * strengthRandomizer.y, Random.Range(-1, 2) * strengthRandomizer.z);
		}

		public Vector3 CalculateActiveShake()
		{
			var freqPower = freqCurve.Evaluate((float)freqPoint / vibCount);
			var shakePoint = shakeCurve.Evaluate(Progress);

			var result = (strengthVector + Multiplier * activeRandomizer * freqPower) * (freqPower * shakePoint);

			return result;
		}

		/// <summary>
		/// Process the shake command.
		/// </summary>
		/// <param name="deltaTime"></param>
		/// <param name="result"></param>
		/// <returns>True if the command is completed.</returns>
		public bool ProcessCommand(in float deltaTime, out Vector3 result)
		{
			Progress += deltaTime * speed;

			result = Vector3.zero;

			try
			{
				if (hadActivator && !activator.isActiveAndEnabled)
				{
					//! force complete.
					return true;
				}
			}
			catch
			{
				// ignored
			}

			result = CalculateActiveShake();
			result *= Multiplier;

			if (Progress >= 1)
			{
				RandomizeStrength();
				Progress = 0;
				++freqPoint;
			}

			return vibCount == freqPoint;
		}
	}
}
