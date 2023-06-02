using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framwework.Inspector
{
	public static class RequiredExcludedTypes
	{
		public static readonly Type[] excludedTypes =
		{
			typeof(Image),
			typeof(EventSystem),
			typeof(Text),
			typeof(ScrollRect),
		};
	}
}