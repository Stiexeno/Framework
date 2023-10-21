using System;
using Framework;
using Framework.Core;
using UnityEngine;

public class ResourceBinder : AbstractBinder
{
	// Serialized fields
	
	// Private fields
	
	private IInstantiator instantiator;
	
	// Properties
	
	//ResourceBinder
	
	public ResourceBinder(Binding binding, string resoucePath, Type type, IInstantiator instantiator) : base(binding)
	{
		this.instantiator = instantiator;
		Instantiate(resoucePath, type);
	}
	
	private void Instantiate(string resoucePath, Type type)
	{
		var targetObject = Resources.Load(resoucePath, type);
        
		if (targetObject == null)
			Context.Exception($"Object of type {type.Name} not found in resources folder", 
			$"Create {type.Name} in resources folder or check if path is valid");
        
		binding.Instance = instantiator.InstantiatePrefab(targetObject).GetComponent(type);
	}
}