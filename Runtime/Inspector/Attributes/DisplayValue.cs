using System;
using UnityEngine;

namespace Framework.Inspector
{
    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public class DisplayValue : PropertyAttribute
    {
        public DisplayValue()
        {
            
        }
    }
}
