using Framework.Utils;
using UnityEngine;
using UnityEngine.AI;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	[RequireComponent(typeof(IAIMovement))]
	public class AIPatrol : MonoBehaviour, ICharacterComponent
	{
		//Serialized fields

		[SF] private float minWanderRange;
		[SF] private float maxWanderRange;

		//Private fields

		private float timelapsed;
		
		private ICharacterState characterState;
		private IAIMovement aIMovement;

		//Properties
		
		private Vector3 GetRandomPoint()
		{
			var wanderRange = Random.Range(minWanderRange, maxWanderRange);
			var randomDirection = transform.GetRandomPointAround(wanderRange);
			NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRange, 1);
			Vector3 finalPosition = hit.position;

			return finalPosition;
		}
		
		public void Process(in float deltaTime)
		{
			if (characterState.State == State.Move)
			{
				timelapsed += deltaTime;
				
				if (timelapsed >= 3)
				{
					timelapsed = 0;
					var randomPoint = GetRandomPoint();
					aIMovement.SetDestination(randomPoint);
				}
			}
		}

		public void Init(Character character)
		{
			characterState = GetComponent<ICharacterState>();
			aIMovement = GetComponent<IAIMovement>();
		}
	}
}