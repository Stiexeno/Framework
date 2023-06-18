using UnityEngine;

namespace Framework
{
    public abstract class Config : ScriptableObject
    {
	     [HideInInspector] public string guid = "UNSET";
        
	    // TODO: Add here BeginChangeCheck and EndChangeCheck
        #if UNITY_EDITOR
        [ContextMenu("Set GUID")]
        public void SetGUID()
        {
	        guid = System.Guid.NewGuid().ToString();
        }
        #endif
    }   
}