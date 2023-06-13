using UnityEngine;
using UnityEngine.AI;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class AIMovement : MonoBehaviour, IAIMovement
	{
		//Serialized fields
		
		//Private fields
		
		private NavMeshAgent agent;

		//Properties
        
		public float Velocity => agent.velocity.magnitude;
		public float MaxSpeed { get; private set; }
		public bool IsMoving => agent.velocity.normalized.magnitude > 0.15f;

		public void SetDestination(Vector3 position)
		{
			if (agent.isStopped)
			{
				agent.isStopped = false;
			}
			
			agent.destination = position;
		}

		public bool ReachedDestination()
		{
			return agent.remainingDistance <= agent.stoppingDistance;
		}

		public void Stop()
		{
			agent.isStopped = true;
		}
		
		public void SetSpeed(float speed)
		{
			agent.speed = speed;
		}

		public void AddSpeed(float speed)
		{
			agent.speed += speed;
		}

		public void SetEnabled(bool value)
		{
			agent.enabled = value;
			enabled = value;
		}
		
		public void Process(in float deltaTime)
		{

		}

		public void Init(Character character)
		{
			agent = GetComponent<NavMeshAgent>();
			MaxSpeed = agent.speed;
		}
        
		public void Pause(float duration)
		{
            
		}
	}   
}
