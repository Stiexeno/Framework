using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{
	public enum Team { Player, Enemy, NPC }
	public sealed class Character : MonoBehaviour
	{
		[SF] private Team team;
		
		private ICharacterComponent[] components;
		
		public Team Team => team;

		public ICharacterComponent Get<T>() where T : ICharacterComponent
		{
			foreach (var component in components)
			{
				if (component is T)
				{
					return component;
				}
			}

			return null;
		}

		private void Awake()
		{
			components = GetComponents<ICharacterComponent>();
			foreach (var component in components)
			{
				component.Init(this);
			}
		}

		private void Update()
		{
			foreach (var component in components)
			{
				component.Process(Time.deltaTime);
			}
		}
	}
}