using Framework.Audio;
using Framework.Core;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
	public class SettingsDialog : Dialog
	{
		[SF] private Toggle musicToggle;
		[SF] private Toggle soundToggle;
		[SF] private Toggle hapticsToggle;

		private IAudioManager audioManager;

		[Inject]
		private void Construct(IAudioManager audioManager)
		{
			this.audioManager = audioManager;
		}

		protected override void OnInit()
		{
			musicToggle.SetState(audioManager.IsMusicEnabled);
			soundToggle.SetState(audioManager.IsSfxEnabled);
			hapticsToggle.SetState(audioManager.IsHapticsEnabled);

			musicToggle.OnToggled += (value) => audioManager.SetMusicEnabled(value);
			soundToggle.OnToggled += (value) => audioManager.SetSfxEnabled(value);
			hapticsToggle.OnToggled += (value) => audioManager.SetHapticsEnabled(value);
		}
	}
}