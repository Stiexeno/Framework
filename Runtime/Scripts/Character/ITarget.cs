using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public interface ITarget
	{
		public Transform transform { get; }
		public bool IsAlive { get; }
		public int Health { get; set; }

		public void DealDamage(DamageArgs args);
	}

	public struct DamageArgs
	{
		public int damage;
		public ITarget dealer;

		public DamageArgs(int damage, ITarget dealer)
		{
			this.damage = damage;
			this.dealer = dealer;
		}
	}
}