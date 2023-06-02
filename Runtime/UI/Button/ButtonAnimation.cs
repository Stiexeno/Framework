using System;
using Framework.UI;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public abstract class ButtonAnimation : MonoBehaviour
{
	private Button button;

	protected Button Button
	{
		get
		{
			if (button == null)
			{
				button = GetComponent<Button>();
			}

			return button;
		}
	}
	
	public abstract void Init();

	public virtual void PressAnimation(Action callback)
	{
		
	}

	public virtual void ReleaseAnimation(Action callback)
	{
		
	}

	public virtual void SetInteractable(bool interactable)
	{
		
	}

	private void OnValidate()
	{
		hideFlags = HideFlags.HideInInspector;
	}
}
