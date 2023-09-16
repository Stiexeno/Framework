using System;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class CollisionTrigger : MonoBehaviour
{
	[SF] protected LayerMask collisionLayers = 0;
	
	public event Action<Collider> TriggerEnter;
	public event Action<Collider> TriggerExit;
	public event Action<Collider> TriggerStay;

	private void OnTriggerEnter(Collider other)
	{
		if (((1 << other.gameObject.layer) & collisionLayers) != 0)
		{
			OnEnter(other);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (((1 << other.gameObject.layer) & collisionLayers) != 0)
		{
			OnStay(other);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (((1 << other.gameObject.layer) & collisionLayers) != 0)
		{
			OnExit(other);
		}
	}

	protected virtual void OnEnter(Collider collider)
	{
		TriggerEnter?.Invoke(collider);
	}
        
	protected virtual void OnExit(Collider collider)
	{
		TriggerExit?.Invoke(collider);
	}
        
	protected virtual void OnStay(Collider collider)
	{
		TriggerStay?.Invoke(collider);
	}
}
