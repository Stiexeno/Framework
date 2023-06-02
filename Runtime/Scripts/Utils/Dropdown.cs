using System;
using UnityEngine;

namespace Framework.Utils
{
	/// <summary>
	/// Basically recreates generic enum with const values from public class
	/// </summary>
	/// <typeparam name="T">Public class which contains public const strings with his values </typeparam>
	[Serializable]
	public class Dropdown<T> where T: class
	{
		/// <summary>
		/// Private field which used to get current value in PropertyDrawer
		/// </summary>
		[SerializeField] private string cachedValue;

		/// <summary>
		/// Main variable which must be used in other classes
		/// </summary>
		public string Value => cachedValue;
	}	
}