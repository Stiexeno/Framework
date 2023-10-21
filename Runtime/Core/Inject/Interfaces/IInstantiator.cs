using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public interface IInstantiator
    {
        T Instantiate<T>();
        object Instantiate(Type objectType);
        internal object Instantiate(Binding binding);
        GameObject InstantiatePrefab(GameObject original);
        public GameObject InstantiatePrefab(Object prefab);
        T Instantiate<T>(params object[] args);
    }
}
