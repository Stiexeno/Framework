using UnityEngine;

namespace Framework
{
    public abstract class Config : ScriptableObject
    {
	    [HideInInspector] public string Guid = "UNSET";
        
	    // TODO: Add here BeginChangeCheck and EndChangeCheck
        #if UNITY_EDITOR
        [ContextMenu("Set GUID")]
        public void SetGUID()
        {
	        Guid = System.Guid.NewGuid().ToString();
        }
        #endif
    }   
}