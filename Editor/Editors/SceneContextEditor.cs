using Framework.Core;
using UnityEditor;
using UnityEngine;

namespace Framework.ContextEditor
{
    [CustomEditor(typeof(SceneContext))]
    public class SceneContextEditor : UnityEditor.Editor
    {
	    private SceneContext sceneContext;

	    public override void OnInspectorGUI()
        {
	        DrawDefaultInspector();

	        if (Application.isPlaying)
	        {
		        GUILayout.Label($"Registered Services: {sceneContext.DiContainer.Container.Count}", EditorStyles.boldLabel);
		        
		        EditorGUILayout.BeginVertical("HelpBox");

		        foreach (var container in sceneContext.DiContainer.Container)
		        {
			        if (container.Value.Instance is ConfigBase)
				        continue;
			        
			        var interfaces = "";

			        if (container.Value.Interfaces != null)
			        {
				        foreach (var iInterface in container.Value.Interfaces)
				        {
					        if (iInterface == null)
						        continue;
				        
					        var splitInterfaces = iInterface.ToString().Split('.');
					        interfaces += $"{splitInterfaces[^1]} ";
				        }   
			        }

			        var type = container.Value.ContractType.ToString().Replace("Framework.", "").Replace("Core.", "");

			        var style = new GUIStyle();
			        style.richText = true;
			        style.fontSize = 12;
			        style.normal.textColor = new Color(0.74f, 0.74f, 0.74f);
			        style.margin = new RectOffset(10, 10, 5, 5);
			        GUILayout.Label($"• <b>{type}</b> {interfaces}", style);
		        }
		        
		        EditorGUILayout.EndVertical();
	        }
        }
	    
        private void OnEnable()
        {
            sceneContext = (SceneContext) target;
        }
    }
}
