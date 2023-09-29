using UnityEngine;

namespace Framework.Editor
{
	public static class EditorRectUtils
	{
		public static Rect SetWidth(this Rect r, float w)
		{
			r.width = w;
			return r;
		}

		public static Rect SetWidthHeight(this Rect r, Vector2 v)
		{
			r.width = v.x;
			r.height = v.y;
			return r;
		}

		public static Rect SetWidthHeight(this Rect r, float w, float h)
		{
			r.width = w;
			r.height = h;
			return r;
		}

		public static Rect AddWidth(this Rect r, float w)
		{
			r.width += w;
			return r;
		}

		public static Rect AddHeight(this Rect r, float h)
		{
			r.height += h;
			return r;
		}

		public static Rect SetHeight(this Rect r, float h)
		{
			r.height = h;
			return r;
		}

		public static Rect AddXY(this Rect r, Vector2 xy)
		{
			r.x += xy.x;
			r.y += xy.y;
			return r;
		}

		public static Rect AddXY(this Rect r, float x, float y)
		{
			r.x += x;
			r.y += y;
			return r;
		}

		public static Rect AddX(this Rect r, float x)
		{
			r.x += x;
			return r;
		}

		public static Rect AddY(this Rect r, float y)
		{
			r.y += y;
			return r;
		}

		public static Rect SetY(this Rect r, float y)
		{
			r.y = y;
			return r;
		}

		public static Rect SetX(this Rect r, float x)
		{
			r.x = x;
			return r;
		}

		public static Rect SetXMin(this Rect r, float x)
		{
			r.xMin = x;
			return r;
		}

		public static Rect SetXMax(this Rect r, float x)
		{
			r.xMax = x;
			return r;
		}

		public static Rect SetYMin(this Rect r, float y)
		{
			r.yMin = y;
			return r;
		}

		public static Rect SetYMax(this Rect r, float y)
		{
			r.yMax = y;
			return r;
		}

		public static Rect AddXMin(this Rect r, float x)
		{
			r.xMin += x;
			return r;
		}

		public static Rect AddXMax(this Rect r, float x)
		{
			r.xMax += x;
			return r;
		}

		public static Rect AddYMin(this Rect r, float y)
		{
			r.yMin += y;
			return r;
		}

		public static Rect AddYMax(this Rect r, float y)
		{
			r.yMax += y;
			return r;
		}

		public static Rect Adjust(this Rect r, float x, float y, float w, float h)
		{
			r.x += x;
			r.y += y;
			r.width += w;
			r.height += h;
			return r;
		}

		public static Rect ToRect(this Vector2 v, float w, float h)
		{
			return new Rect(v.x, v.y, w, h);
		}

		public static Rect ZeroXY(this Rect r)
		{
			return new Rect(0, 0, r.width, r.height);
		}

		public static Vector2 ToVector2(this Rect r)
		{
			return new Vector2(r.width, r.height);
		}

		public static Rect ClampToRect(this Rect rect, Rect targetRect, float padding)
		{
			// Calculate the clamped position for the smallerRect.
			//float clampedX = Mathf.Clamp(rect.x, targetRect.x, targetRect.x + targetRect.width - rect.width);
			//float clampedY = Mathf.Clamp(rect.y, targetRect.y, targetRect.y + targetRect.height - rect.height);

			float clampedX = Mathf.Clamp(rect.x, 0, targetRect.width - rect.width - padding);
			float clampedY = Mathf.Clamp(rect.y, 0, targetRect.height - rect.height - padding);
			
			return new Rect(clampedX, clampedY, rect.width, rect.height);
		}

		public static Rect Expand(this Rect rect, float value)
		{
			return new Rect(rect.x - value, rect.y - value, rect.width + value * 2, rect.height + value * 2);
		}
		
		public static Rect Expand(this Rect rect, float x, float y)
		{
			return new Rect(rect.x - x, rect.y - y, rect.width + x * 2, rect.height + y * 2);
		}

		#if UNITY_EDITOR
		public static Rect AddLine(this Rect r, int count = 1)
		{
			return AddY(r, count * (UnityEditor.EditorGUIUtility.singleLineHeight + UnityEditor.EditorGUIUtility.standardVerticalSpacing));
		}

		public static Rect SetLineHeight(this Rect r)
		{
			return SetHeight(r, UnityEditor.EditorGUIUtility.singleLineHeight);
		}
#endif
	}
}