using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
    public interface IProcessable
    {
        void Process(in float deltaTime);
    }
}
