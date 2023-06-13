using Animancer;
using Framework.Core;
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

		//Private fields
		
		private IInputManager inputManager;               
		private ICharacterState characterState;               
		private Character character;                      
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
			characterController.Move(moveDirection * (moveSpeed * deltaTime));
			moveDirection.SetY(0);

			if (moveDirection != Vector3.zero)
			{
				characterState.State = State.Move;
				transform.rotation = Quaternion.LookRotation(moveDirection);
			}
		}

		public void Init(Character character)
		{
			this.character = character;
			characterController = GetComponent<CharacterController>();
			characterState = GetComponent<ICharacterState>();
		}
		
		public void Pause(float duration)
		{
		}
	}   
}
