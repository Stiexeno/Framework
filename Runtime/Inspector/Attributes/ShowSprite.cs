using UnityEngine;

namespace Framework.Inspector
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ShowSprite : PropertyAttribute
    {
        // ReSharper disable once EmptyConstructor
        public ShowSprite()
        {
        }
    }
}