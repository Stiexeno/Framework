using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework
{
	public static class ListExtensions
	{
		// Dictionary 

		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue valueToAdd)
		{
			if (dictionary.TryGetValue(key, out var value))
				return value;

			value = valueToAdd;
			dictionary[key] = value;

			return value;
		}

		public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
			}
			else
			{
				dictionary.Add(key, value);
			}
		}

		// List

		public static T RandomResult<T>(this List<T> list)
		{
			var count = list.Count;

			if (count == 0)
				return default;

			return list[Random.Range(0, count)];
		}

		public static int RandomIndex<T>(this List<T> list)
		{
			return Random.Range(0, list.Count);
		}

		public static void Shuffle<T>(this List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				T temp = list[i];
				int randomIndex = Random.Range(i, list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		public static T First<T>(this List<T> list)
		{
			if (list.Count == 0)
				return default;

			return list[0];
		}

		public static T Last<T>(this List<T> list)
		{
			if (list.Count == 0)
				return default;

			return list[list.Count - 1];
		}

		public static T Cycle<T>(this List<T> list, int index)
		{
			if (index >= list.Count)
				return list[0];

			if (index < 0)
				return list.Last();

			return list[index];
		}

		public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
		{
			T item = list[oldIndex];
			list.Insert(newIndex, item);
			list.Remove(item);
		}

		public static bool IsEmpty<T>(this List<T> list)
		{
			return list == null || list.Count <= 0;
		}

		public static bool ContainDetails<T>(this List<T> list, Predicate<T> match)
		{
			foreach (T value in list)
			{
				if (match(value))
				{
					return true;
				}
			}

			return false;
		}

		public static void RemoveIf<T>(this List<T> list, Predicate<T> match)
		{
			for (var i = 0; i < list.Count; i++)
			{
				T value = list[i];

				if (match(value))
				{
					list.Remove(value);
					i = 0;
				}
			}
		}

		// Array

		public static T IndexOrLast<T>(this T[] array, int index)
		{
			if (index >= array.Length)
				return array.Last();

			if (index < 0)
				return array.First();

			return array[index];
		}

		public static T First<T>(this T[] array)
		{
			return array[0];
		}

		public static T Last<T>(this T[] array)
		{
			return array[^1];
		}

		public static T RandomResult<T>(this T[] array)
		{
			var count = array.Length;

			if (count == 0)
				return default;

			return array[Random.Range(0, count)];
		}

		public static int RandomIndex<T>(this T[] array)
		{
			return Random.Range(0, array.Length);
		}

		public static bool Contains<T>(this T[] array, T value)
		{
			if (array == null)
			{
				Debug.LogError("Target collection is null");
				return false;
			}

			if (value == null)
			{
				Debug.LogError("Trying to check if contains null value");
				return false;
			}

			foreach (var v in array)
			{
				if (v != null && v.Equals(value))
				{
					return true;
				}
			}

			return false;
		}

		public static T[] Add<T>(this T[] thisArray, T value)
		{
			List<T> nList = thisArray.ToList();
			nList.Add(value);
			return nList.ToArray();
		}

		public static T[] Remove<T>(this T[] thisArray, T value)
		{
			List<T> nList = thisArray.ToList();
			nList.Remove(value);
			return nList.ToArray();
		}

		public static T[] Remove<T>(this T[] thisArray, int index)
		{
			List<T> nList = thisArray.ToList();
			nList.RemoveAt(index);
			return nList.ToArray();
		}

		public static void RemoveRange<T>(this List<T> thisList, List<T> toRemove)
		{
			foreach (var t in toRemove)
			{
				if (thisList.Contains(t))
					thisList.Remove(t);
			}
		}

		public static List<T> ToList<T>(this T[] thisArray)
		{
			List<T> list = new List<T>();

			foreach (var e in thisArray)
			{
				list.Add(e);
			}

			return list;
		}

		public static T Cycle<T>(this T[] array, int index)
		{
			if (index >= array.Length)
				return array[0];

			if (index <= 0)
				return array.Last();

			return array[index];
		}

		public static bool IsEmpty<T>(this T[] array)
		{
			return array == null || array.Length <= 0;
		}

		public static bool ContainDetails<T>(this T[] array, Predicate<T> match)
		{
			foreach (T value in array)
			{
				if (match(value))
				{
					return true;
				}
			}

			return false;
		}
	}
}