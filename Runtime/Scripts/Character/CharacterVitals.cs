using System;
using Framework.Utils;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public class CharacterVitals : MonoBehaviour , ICharacterVitals
	{
		//Serialized fields

		[SF] private int maxHealth;
		[SF] private ProgressBar healthbar;
		[SF, CanBeNull] private ParticleSystem hitVFX;

		//Private fields

		//Properties
		
		public bool IsAlive => Health > 0;
		public int Health { get; set; }
		
		public event Action<DamageArgs> OnDamageTaken;
		
		public void Process(in float deltaTime)
		{
		}

		public void Init(Character character)
		{
			Health = maxHealth;
		}
		
		public void DealDamage(DamageArgs args)
		{
			healthbar.SetEnabled(true);
			
			Health -= args.damage;
			healthbar.Value = (float)Health / maxHealth;

			hitVFX?.Play();	

			if (Health <= 0)
			{
				healthbar.SetEnabled(false);
			}
			
			OnDamageTaken?.Invoke(args);
		}
	}
}