using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public interface IFixedProcessable
	{
		void FixedProcess(in float deltaTime);
	}
}