using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
    public interface IWindowView
    {
        public void OnGUI();
    }
}