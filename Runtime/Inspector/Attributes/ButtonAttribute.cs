using UnityEngine;

namespace Framework.Inspector
{
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class ButtonAttribute : PropertyAttribute
	{
		public readonly string label;
		public readonly float height = 21f;

		/// <summary>
		/// Custom button on any MonoBehaviour to call Methods for Debug purposes
		/// </summary>
		/// <param name="label">Label shown on the button</param>
		/// <param name="buttonColor">Button color</param>
		/// <param name="height">Button height</param>
		public ButtonAttribute(string label = "", float height = 18f)
		{
			this.label = label;
			this.height = height;
		}

		public ButtonAttribute()
		{
		}
	}
}