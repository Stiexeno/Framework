using Animancer;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	[RequireComponent(typeof(ICharacterState), typeof(ICharacterMovement))]
	public class CharacterMoveAnimation : MonoBehaviour, ICharacterComponent
	{
		//Serialized fields

		[SF] private LinearMixerTransition transition;
		
		//Private fields

		private LinearMixerTransition currentTransition;
		private AnimancerLayer baseLayer;
		private AnimancerComponent animancer;

		private ICharacterMovement characterMovement;

		//Properties

		public void SetTransition(LinearMixerTransition transition)
		{
			currentTransition = transition;
		}

		public void SetDefaultTransition()
		{
			currentTransition = transition;
		}

		public void Process(in float deltaTime)
		{
			baseLayer.Play(currentTransition);

			currentTransition.State.Parameter = characterMovement.Velocity / characterMovement.MaxSpeed;
		}

		public void Init(Character character)
		{
			GetComponent<ICharacterState>();
			characterMovement = GetComponent<ICharacterMovement>();
			animancer = GetComponentInChildren<AnimancerComponent>();

			SetDefaultTransition();

			baseLayer = animancer.Layers[0];

			animancer.Play(currentTransition);
		}
	}
}