using System;
using Framework.Utils;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public class CharacterVitals : MonoBehaviour , ICharacterVitals
	{
		//Serialized fields

		[SF] private Team team;
		[SF] private int maxHealth;
		[SF] private int minHealth;
		[SF] private ProgressBar healthbar;
		[SF, CanBeNull] private ParticleSystem hitVFX;

		//Private fields

		//Properties

		public Team Team => team;
		public bool IsAlive => Health > 0;
		public int Health { get; set; }
		public int MinHealth => minHealth;
		public int MaxHealth => maxHealth;

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
			
			var targetHealth = Mathf.Clamp(Health - args.damage, minHealth, maxHealth);
			Health = targetHealth;
			healthbar.Value = (float)Health / maxHealth;

			if (hitVFX != null)
			{
				hitVFX.Play();
			}
			
			if (Health <= 0)
			{
				healthbar.SetEnabled(false);
			}
			
			OnDamageTaken?.Invoke(args);
		}
	}
}