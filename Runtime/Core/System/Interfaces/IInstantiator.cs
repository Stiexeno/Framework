using System;
using UnityEngine;

namespace Framework
{
    public interface IInstantiator
    {
        T Instantiate<T>();
        object Instantiate(Type objectType);
        void InjectToSceneGameObjects();
        GameObject InstantiatePrefab(GameObject original);
        T Instantiate<T>(params object[] args);
    }
}