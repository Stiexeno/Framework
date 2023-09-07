using System;
using UnityEngine;

namespace Framework.Inspector
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DisplayValue : PropertyAttribute
    {
        // ReSharper disable once EmptyConstructor
        public DisplayValue()
        {
            
        }
    }
}
