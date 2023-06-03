using System.Collections.Generic;
using UnityEditor;

namespace Framework.Editor
{
	internal class AssetPostProcessor : AssetPostprocessor
	{
		private static SortedDelegate<string[], string[], string[], string[]> s_OnPostProcessHandler =
			new SortedDelegate<string[], string[], string[], string[]>();

		public static SortedDelegate<string[], string[], string[], string[]> OnPostProcess => s_OnPostProcessHandler;

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			if (s_OnPostProcessHandler != null)
			{
				s_OnPostProcessHandler.Invoke(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
			}
		}
	}

	internal class SortedDelegate<T1, T2, T3, T4>
	{
		struct BufferedValues
		{
			public T1 arg1;
			public T2 arg2;
			public T3 arg3;
			public T4 arg4;
		}

		List<BufferedValues> m_Buffer;
		bool m_IsInvoking;
		private List<(int, Delegate)> m_RegisterQueue = new List<(int, Delegate)>();

		public delegate void Delegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		private SortedList<int, Delegate> m_SortedInvocationList = new SortedList<int, Delegate>();

		public void Unregister(Delegate toUnregister)
		{
			IList<int> keys = m_SortedInvocationList.Keys;
			for (int i = 0; i < keys.Count; ++i)
			{
				m_SortedInvocationList[keys[i]] -= toUnregister;
				if (m_SortedInvocationList[keys[i]] == null)
				{
					m_SortedInvocationList.Remove(keys[i]);
					break;
				}
			}

			if (m_IsInvoking)
			{
				for (int i = m_RegisterQueue.Count - 1; i >= 0; --i)
				{
					if (m_RegisterQueue[i].Item2 == toUnregister)
					{
						m_RegisterQueue.RemoveAt(i);
						break;
					}
				}
			}
		}

		public void Register(Delegate toRegister, int order)
		{
			if (m_IsInvoking)
			{
				m_RegisterQueue.Add((order, toRegister));
				return;
			}

			Unregister(toRegister);
			if (m_SortedInvocationList.ContainsKey(order))
				m_SortedInvocationList[order] += toRegister;
			else
				m_SortedInvocationList.Add(order, toRegister);
			InvokeBuffer_Internal();
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (m_IsInvoking)
				return;

			m_IsInvoking = true;
			foreach (var invocationList in m_SortedInvocationList)
				invocationList.Value?.Invoke(arg1, arg2, arg3, arg4);

			if (m_RegisterQueue.Count > 0)
			{
				m_IsInvoking = false;
				foreach (var toRegister in m_RegisterQueue)
					Register(toRegister.Item2, toRegister.Item1);
				m_RegisterQueue.Clear();
				m_IsInvoking = true;
			}

			InvokeBuffer_Internal();
			m_IsInvoking = false;
		}

		void InvokeBuffer_Internal()
		{
			if (m_Buffer != null)
			{
				foreach (var b in m_Buffer)
					Invoke(b.arg1, b.arg2, b.arg3, b.arg4);
				m_Buffer = null;
			}
		}

		public void BufferInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (m_SortedInvocationList.Count == 0 || m_IsInvoking)
			{
				if (m_Buffer == null)
					m_Buffer = new List<BufferedValues>();
				m_Buffer.Add(new BufferedValues { arg1 = arg1, arg2 = arg2, arg3 = arg3, arg4 = arg4 });
			}
			else
			{
				Invoke(arg1, arg2, arg3, arg4);
			}
		}

		public static SortedDelegate<T1, T2, T3, T4> operator +(SortedDelegate<T1, T2, T3, T4> self, Delegate delegateToAdd)
		{
			int lastInOrder = self.m_SortedInvocationList.Keys[self.m_SortedInvocationList.Count - 1];
			self.Register(delegateToAdd, lastInOrder + 1);
			return self;
		}

		public static SortedDelegate<T1, T2, T3, T4> operator -(SortedDelegate<T1, T2, T3, T4> self, Delegate delegateToRemove)
		{
			self.Unregister(delegateToRemove);
			return self;
		}

		public static bool operator ==(SortedDelegate<T1, T2, T3, T4> obj1, SortedDelegate<T1, T2, T3, T4> obj2)
		{
			bool aNull = ReferenceEquals(obj1, null);
			bool bNull = ReferenceEquals(obj2, null);

			if (aNull && bNull)
				return true;
			if (!aNull && bNull)
				return obj1.m_SortedInvocationList.Count == 0;
			if (aNull && !bNull)
				return obj2.m_SortedInvocationList.Count == 0;
			if (ReferenceEquals(obj1, obj2))
				return true;
			return obj1.Equals(obj2);
		}

		public static bool operator !=(SortedDelegate<T1, T2, T3, T4> lhs, SortedDelegate<T1, T2, T3, T4> rhs)
		{
			return !(lhs == rhs);
		}

		protected bool Equals(SortedDelegate<T1, T2, T3, T4> other)
		{
			return Equals(m_SortedInvocationList, other.m_SortedInvocationList);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((SortedDelegate<T1, T2, T3, T4>)obj);
		}

		public override int GetHashCode()
		{
			return (m_SortedInvocationList != null ? m_SortedInvocationList.GetHashCode() : 0);
		}
	}
}