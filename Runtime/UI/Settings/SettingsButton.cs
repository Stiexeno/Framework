using Framework.Core;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
	public class SettingsButton : Button
	{
		//Serialized fields
		
		//Private fields

		private IUIManager uiManager;
		
		//Properties

		[Inject]
		private void Construct(IUIManager uiManager)
		{
			this.uiManager = uiManager;
		}
		
		protected override void ClickPerformed()
		{
			uiManager.OpenDialog<SettingsDialog>();
		}
	}
}