using UnityEngine;

namespace Framework.Inspector
{
    [System.AttributeUsage(System.AttributeTargets.All)]
    public class IfPlaying : PropertyAttribute
    {
        public readonly bool value = true;
        
        /// <summary>
        /// Attribute to toggle showing during play or editor
        /// </summary>
        /// <param name="value">true means will show only during gameplay, false only out of gameplay</param>
        public IfPlaying(bool value = true)
        {
            this.value = value;
        }

        public IfPlaying()
        {
            
        }
    }
}