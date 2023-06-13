using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Character
{

	public sealed class Character : MonoBehaviour
	{
		private ICharacterComponent[] components;

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