using UnityEngine;

namespace Framework.Inspector
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ShowSprite : PropertyAttribute
    {
        public ShowSprite()
        {
        }
    }
}