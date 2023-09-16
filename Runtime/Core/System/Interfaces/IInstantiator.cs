using System;
using UnityEngine;

namespace Framework
{
    public interface IInstantiator
    {
        T Instantiate<T>();
        object Instantiate(Type objectType);
        internal object Instantiate(Binding binding);
        GameObject InstantiatePrefab(GameObject original);
        T InstantiatePrefab<T>(T original) where T : class;
        T Instantiate<T>(params object[] args);
    }
}
