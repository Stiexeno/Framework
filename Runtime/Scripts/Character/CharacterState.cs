using System;
using Framework.SimpleInput;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public enum State { Move, Combat, Action}

	public class CharacterState : MonoBehaviour, ICharacterState
	{
		//Serialized fields

		//Private fields
		
		private IInputManager inputManager;

		//Properties

		public State State { get; set; } = State.Move;

		[Inject]
		private void Construct(IInputManager inputManager)
		{
			this.inputManager = inputManager;
		}

		public void SetState(State state)
		{
			switch (state)
			{
				case State.Move:
					State = State.Move;
					break;
				case State.Combat:
					if (inputManager.Axis == Vector2.zero)
					{
						State = State.Combat;
					}
					
					break;
				case State.Action:
					State = State.Action;
					break;
				
			}
		}
		
		public void Process(in float deltaTime)
		{
		}

		public void Init(Character character)
		{
		}
	}
}