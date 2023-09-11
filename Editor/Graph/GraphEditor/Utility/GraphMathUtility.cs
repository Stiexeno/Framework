using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public static class GraphMathUtility
	{
		public static Vector2 Round(Vector2 v)
		{
			return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
		}

		public static Rect Round(Rect r)
		{
			return new Rect(Round(r.position), Round(r.size));
		}
		
		public static Vector2 SnapPosition(Vector2 p, float snapStep)
		{
			return SnapPosition(p.x, p.y, snapStep);
		}
		
		public static Vector2 SnapPosition(float x, float y, float snapStep)
		{
			return new Vector2(Snap(x, snapStep), Snap(y, snapStep));
		}

		public static float Snap(float x, float snapStep)
		{
			return Mathf.Round(x / snapStep) * snapStep;
		}
	}
}