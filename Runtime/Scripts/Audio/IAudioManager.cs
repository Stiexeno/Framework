using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Audio
{
	public interface IAudioManager
	{
		public void PlayOneShot(AudioClip clip, float volume = 1f, bool silentMusic = false);
		public void StopMusic();
		public void PlayMusic(AudioClip clip, float volume = 1f);
		public void PlayLoop(AudioClip clip);
		public void StopLoop(AudioClip clip);
		public void SetMusicPitch(float value);
		public void SetSfxEnabled(bool value);
		public void SetMusicEnabled(bool value);
		public void SetHapticsEnabled(bool value);
		public void SetTimeScale(float value, float lerpDuration = 0.1f);
		public void SetVolume(float volume);
		bool IsSfxEnabled { get; }
		bool IsMusicEnabled { get; }
		bool IsHapticsEnabled { get; }
	}
}
