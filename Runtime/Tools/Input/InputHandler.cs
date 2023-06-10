using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.SimpleInput
{
	public abstract class InputHandler : MonoBehaviour
	{
		public Vector2 Direction { get; protected set; }
	}
}