using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class BehaviourTreePreferences : ScriptableObject
{
	public Texture2D sequencerIcon;
	public Texture2D selectorIcon;
	public Texture2D waitIcon;
	
	public Texture2D closeIcon;
	public Texture2D dotIcon;
	
	public Texture2D saveIcon;
	public Texture2D loadIcon;
	public Texture2D formatIcon;
	
	public List<string> savedGraphs = new List<string>();
	
	private static BehaviourTreePreferences instance;

	public static BehaviourTreePreferences Instance
	{
		get
		{
			if (instance == null)
			{
				instance = LoadDefaultPreferences();
			}
			return instance;
		}

		set
		{
			instance = value;
		}
	}
	
	public void SaveGraph(string path)
	{
		if (savedGraphs.Contains(path))
		{
			return;
		}
		
		savedGraphs.Add(path);
	}

	public List<string> GetSavedGraphs()
	{
		for (int i = savedGraphs.Count - 1; i >= 0; i--)
		{
			if (AssetDatabase.LoadAssetAtPath(savedGraphs[i], typeof(BehaviourTree)) == null)
			{
				savedGraphs.RemoveAt(i);
			}
		}

		return savedGraphs;
	}

	public void RemoveSavedGraph(string path)
	{
		savedGraphs.Remove(path);
	}

	private static BehaviourTreePreferences LoadDefaultPreferences()
	{
		var prefs = AssetDatabase.LoadAssetAtPath("Packages/com.framework.dependency-injection/Editor/Graph/BehaviourTree/Preferences/BehaviourTreePreferences.asset", typeof(BehaviourTreePreferences)) as BehaviourTreePreferences;

		if (prefs == null)
		{
			Debug.LogWarning("Failed to load BehaviourTreePreferences");
			prefs = CreateInstance<BehaviourTreePreferences>();
		}

		return prefs;
	}
}
