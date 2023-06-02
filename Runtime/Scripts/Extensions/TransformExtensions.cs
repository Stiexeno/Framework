using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public static class TransformExtensions
	{
		public static Vector3 SetX(this Vector3 vector, float x)
		{
			return new Vector3(x, vector.y, vector.z);
		}

		public static Vector3 SetY(this Vector3 vector, float y)
		{
			return new Vector3(vector.x, y, vector.z);
		}

		public static Vector3 SetZ(this Vector3 vector, float z)
		{
			return new Vector3(vector.x, vector.y, z);
		}

		public static Vector3 AddX(this Vector3 vector, float x)
		{
			return new Vector3(vector.x + x, vector.y, vector.z);
		}

		public static Vector3 AddY(this Vector3 vector, float y)
		{
			return new Vector3(vector.x, vector.y + y, vector.z);
		}

		public static Vector3 AddZ(this Vector3 vector, float z)
		{
			return new Vector3(vector.x, vector.y, vector.z + z);
		}
		
		public static void Reset(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
		
		public static RectTransform RectTransform(this Transform transform)
		{
			if (transform is RectTransform rectTransform)
			{
				return rectTransform;
			}
	        
			Debug.LogError($"You are trying to convert to RectTransform not UI Element {transform.gameObject.name}");
			return null;
		}
		
		public static void ForceRebuildLayout(this RectTransform rect)
		{
			foreach (RectTransform tr in rect)
			{
				LayoutRebuilder.MarkLayoutForRebuild(tr);
				LayoutRebuilder.ForceRebuildLayoutImmediate(tr);
			}
		}
	}
}