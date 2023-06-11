using UnityEngine;

namespace Framework.Character
{
	public interface ICharacterMovement : ICharacterComponent
	{
		float Velocity { get; }
		float MaxSpeed { get; }
		bool IsMoving { get; }
		void Pause(float duration);
	}   
}
