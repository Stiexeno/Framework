using Animancer;
using Framework.SimpleInput;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	[RequireComponent(typeof(CharacterController), typeof(ICharacterState))]
	public class CharacterMovement : MonoBehaviour, ICharacterMovement
	{
		//Serialized fields

		[SF] private float moveSpeed = 20;
		[SF] private float fallSpeed = 2;

		//Private fields
		
		private IInputManager inputManager;               
		private ICharacterState characterState;
		private CharacterController characterController;  
		private AnimancerLayer baseLayer;

		//Properties
		
		public float Velocity => characterController.velocity.magnitude;
		public float MaxSpeed => moveSpeed;
		public bool IsMoving => characterController.velocity.normalized.magnitude > 0.15f;

		[Inject]
		private void Construct(IInputManager inputManager)
		{
			this.inputManager = inputManager;
		}
        
		public void Process(in float deltaTime)
		{
			var moveDirection = new Vector3(inputManager.Axis.x, 0, inputManager.Axis.y);
			characterController.Move((moveDirection + ProcessFalling()) * (moveSpeed * deltaTime));

			if (moveDirection != Vector3.zero)
			{
				characterState.State = State.Move;
				transform.rotation = Quaternion.LookRotation(moveDirection);
			}
		}

		private Vector3 ProcessFalling()
		{
			bool isFalling = !characterController.isGrounded;
			Vector3 fallVelocity = isFalling ? new Vector3(0, -fallSpeed * Time.deltaTime, 0) : Vector3.zero;

			return fallVelocity;
		}

		public void Init(Character character)
		{
			characterController = GetComponent<CharacterController>();
			characterState = GetComponent<ICharacterState>();
		}
		
		public void Pause(float duration)
		{
		}
	}   
}
