using Framework.Character;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public interface IAIMovement : ICharacterMovement
	{
		void SetDestination(Vector3 position);
		void Stop();
		void SetSpeed(float speed);
		bool ReachedDestination();
	}
}