using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Pool
{
	public class ParticlePool : MonoBehaviour
	{
		[SF] private int prewarmSize = 3;
		[SF] private ParticleSystem source;

		private readonly List<ParticleSystem> pool = new List<ParticleSystem>();

		private void Awake()
		{
			if (string.IsNullOrEmpty(source.gameObject.scene.name) == false) // NOTE: way of checking if it's prefab or a scene object
			{
				source.gameObject.SetActive(false);
			}

			for (int i = 0; i < prewarmSize; i++)
			{
				var newParticleSys = Instantiate(source, transform);
				newParticleSys.gameObject.SetActive(false);
				pool.Add(newParticleSys);
			}
		}
		
		public ParticleSystem Play(Vector3 position)
		{
			var particleSys = Play();
			particleSys.transform.position = position;

			return particleSys;
		}

		public ParticleSystem Play()
		{
			foreach (var particleSys in pool)
			{
				if (particleSys.gameObject.activeSelf == false || particleSys.isPlaying == false)
				{
					particleSys.gameObject.SetActive(true);
					particleSys.Play();

					return particleSys;
				}
			}

			var newParticleSys = Instantiate(source, transform);
			pool.Add(newParticleSys);

			newParticleSys.Play();
			newParticleSys.gameObject.SetActive(true);

			return newParticleSys;
		}
	}
}