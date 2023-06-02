using System;
using UnityEngine;

namespace Framework
{
    public interface IResolver
    {
        T Resolve<T>();
        object Resolve(Type contractType);
        T FindInScene<T>() where T: MonoBehaviour;
    }
}
