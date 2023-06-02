using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Utils
{
	[Serializable]
	public class Observable<T> : IObservable<T>
	{
		[SerializeField] private T value;

		public T Value
		{
			get => value;

			set
			{
				if (EqualityComparer<T>.Default.Equals(this.value, value) == false)
				{
					OnValueAboutToChangeDelta?.Invoke(this.value, value);
					this.value = value;
					OnValueChanged?.Invoke(this.value);
				}
			}
		}

		public event Action<T> OnValueChanged;
		public event Action<T,T> OnValueAboutToChangeDelta;

		public Observable(T value)
		{
			this.value = value;
		}

		public static implicit operator T(Observable<T> observable)
		{
			return observable.Value;
		}
		
		public override string ToString()
		{
			return value.ToString();
		}
	}

	public interface IObservable<out T>
	{
		T Value { get; }
		/// <summary>
		/// The argument is the final value.
		/// </summary>
		event Action<T> OnValueChanged;
		/// <summary>
		/// The first argument is the previous value, the second argument is the final value.
		/// </summary>
		event Action<T,T> OnValueAboutToChangeDelta;
	}
}