using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public interface IDIState
	{
		internal void EnterState();
		internal void ExitState();
	}	
}