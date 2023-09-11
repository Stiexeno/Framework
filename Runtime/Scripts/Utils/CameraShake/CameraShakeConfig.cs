using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Shake
{
	[CreateAssetMenu(fileName = "CameraShakeConfig", menuName = "Framework/Camera/CameraShakeConfig")]
    public class CameraShakeConfig : ScriptableObject
    {
	    public bool ignoreGlobalCap;
	    
	    public Vector3 strength = new Vector3(0.1f,0.1f, 0.1f);
	    public float duration = 0.2f;
	    public int vibration = 10;

	    public AnimationCurve frequencyCurve = new AnimationCurve(
				new Keyframe(0, 1),
						new Keyframe(0.5f, 0.25f),
						new Keyframe(1, 0));
	    
	    public AnimationCurve shakerCurve = new AnimationCurve(
		    new Keyframe(0, 0),
		    new Keyframe(0.25f, -1f),
		    new Keyframe(0.75f, 1),
		    new Keyframe (1, 0));
	    
	    [Header ("Making this 1,1,1 will randomize it from -1 to 1 on per axis")]
	    public Vector3 strengthRandomizer;
	    
	    [Header ("Power boost will multiply the strength, and randomizer")]  
	    [Header ("You can customize the linearity by this curve")]	  
	    [Header ("In the case when it called with a value between 0 and 1")]
	    [Header ("Configs usually called with powerBoost = 1")]  
	    [Header ("")] 	    
	    public AnimationCurve powerBoostCurve = new AnimationCurve(
		    new Keyframe(0, 0),
		    new Keyframe(0.25f, 0.25f),
		    new Keyframe(0.75f, 0.5f),
		    new Keyframe (1, 1f));
    }
}
