using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
	public class DisableButtonAnimation : ButtonAnimation
	{
		// Serialized fields

		[SF] private Color buttonColor = Color.white;
		[SF] private Color textColor = Color.white;

		// Private fields

		private Color cachedTextColor = Color.clear;
		
		// Properties

		// DisableButtonAnimation

		public override void SetInteractable(bool interactable)
		{
			if (cachedTextColor == Color.clear)
			{
				cachedTextColor = Button.Text.color;
			}
			
			Button.Image.color = interactable ? Color.white : buttonColor;
			Button.Text.color = interactable ? Color.white : textColor;
		}

		public override void Init()
		{
		}
	}	
}