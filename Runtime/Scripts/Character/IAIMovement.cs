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
		void AddSpeed(float speed);
		void SetEnabled(bool value);
	}
}