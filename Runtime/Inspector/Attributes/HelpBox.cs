using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Inspector
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class HelpBox : PropertyAttribute
	{
        public string description;
		public string messageType;
		
		public HelpBox(string description, string messageType)
		{
			this.description = description;
			this.messageType = messageType;
		}
    }
}
