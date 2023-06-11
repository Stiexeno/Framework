using System;

namespace Framework.Character
{
	public interface ICharacterVitals : ICharacterComponent, ITarget
	{
		public event Action<DamageArgs> OnDamageTaken;
	}   
}
