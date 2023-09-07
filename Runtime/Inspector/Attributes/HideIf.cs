using UnityEngine;
using System;

namespace Framework.Inspector
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class HideIf : PropertyAttribute
    {
        public string PropertyName { get; private set; }
        public object Value        { get; private set; }
        
        public HideIf(string pName,object v)
        {
            PropertyName = pName;
            Value = v;
        }
    }
}