using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

namespace Framework.Audio
{
	public class AudioManager : MonoBehaviour, IAudioManager
	{
		// Private fields
	
		private const int POOL_SIZE = 10;
		
		private float musicVolume = 1f;
		private bool silencingMusic = false;
		
		private AudioSource musicSource;
		private AudioData data;

		private List<AudioSource> sources = new List<AudioSource>();
		
		// Properties

		public bool IsSfxEnabled => data.isSfxEnabled;
		public bool IsMusicEnabled => data.isMusicEnabled;
		public bool IsHapticsEnabled => data.isHapticsEnabled;
		
		// AudioManager
		
		[Inject]
		private void Construct(IDataManager dataManager)
		{
			data = dataManager.GetOrCreateData<AudioData>();

			for (int i = 0; i < POOL_SIZE; i++)
			{
				var source = new GameObject();
				source.transform.SetParent(transform);
				source.name = "Source";
				sources.Add(source.AddComponent<AudioSource>());
			}

			var mSource = new GameObject();
			mSource.transform.SetParent(transform);
			mSource.name = "MusicSource";

			musicSource = mSource.AddComponent<AudioSource>();
			musicSource.loop = true;

			SetMusicEnabled(data.isMusicEnabled);
			SetSfxEnabled(data.isSfxEnabled);
		}

		public void StopMusic()
		{
			musicSource.Stop();
		}

		public void PlayMusic(AudioClip clip, float volume = 1f)
		{
			musicVolume = volume;
			musicSource.clip = clip;

			if (data.isMusicEnabled)
				musicSource.volume = volume;

			musicSource.Play();
		}

		public void PlayLoop(AudioClip clip)
		{
			foreach (AudioSource src in sources)
			{
				if (src.isPlaying == false)
				{
					src.clip = clip;
					src.loop = true;
					src.Play();
					break;
				}
			}
		}

		public void StopLoop(AudioClip clip)
		{
			foreach (AudioSource src in sources)
			{
				if (src.isPlaying && src.clip == clip)
				{
					src.Stop();
					break;
				}
			}
		}

		private IEnumerator MusicSilentCoroutine(float effectDuration, Action onSilent)
		{
			silencingMusic = true;

			var duration = 0.2f;

			float t = 0f;

			var currentVolume = musicSource.volume;
			var targetVolume = currentVolume / 2f;

			while (t < 1)
			{
				musicSource.volume = Mathf.Lerp(currentVolume, targetVolume, t);
				t += Time.deltaTime / duration;
				yield return null;
			}

			onSilent?.Invoke();

			yield return new WaitForSeconds(effectDuration);

			t = 0;

			while (t < 1)
			{
				musicSource.volume = Mathf.Lerp(targetVolume, currentVolume, t);
				t += Time.deltaTime / duration;
				yield return null;
			}

			silencingMusic = false;
		}

		public void PlayOneShot(AudioClip clip, float volume = 1, bool silentMusic = false)
		{
			if (silentMusic && silencingMusic == false)
			{
				StartCoroutine(MusicSilentCoroutine(clip.length, play));
			}
			else
			{
				play();
			}

			void play()
			{
				foreach (AudioSource src in sources)
				{
					if (src.isPlaying == false)
					{
						src.PlayOneShot(clip, volume);
						break;
					}
				}
			}
		}

		public void SetSfxEnabled(bool value)
		{
			data.isSfxEnabled = value;

			foreach (var src in sources)
			{
				src.volume = value ? 1 : 0;
			}
		}
		
		public void SetHapticsEnabled(bool value)
		{
			data.isHapticsEnabled = value;
		}

		public void SetTimeScale(float value, float lerpDuration = 0.1f)
		{
			StartCoroutine(LerpTimeScale(value, lerpDuration));
		}

		private IEnumerator LerpTimeScale(float value, float lerpDuration)
		{
			float duration = lerpDuration;
			var initPitch = musicSource.pitch;

			while (duration > 0)
			{
				duration -= Time.deltaTime;

				SetMusicPitch(Mathf.Lerp(initPitch, value, lerpDuration / duration));

				var initPitches = new List<float>();

				foreach (var src in sources)
				{
					initPitches.Add(src.pitch);
				}

				int i = 0;
				foreach (var src in sources)
				{
					src.pitch = Mathf.Lerp(initPitches[i], value, lerpDuration / duration);
					i++;
				}

				yield return null;
			}
		}

		public void SetVolume(float volume)
		{
			AudioListener.volume = volume;
			data.volume = volume;
		}

		public void SetMusicEnabled(bool value)
		{
			data.isMusicEnabled = value;

			musicSource.volume = value ? musicVolume : 0;
		}

		public void SetMusicPitch(float value)
		{
			musicSource.pitch = value;
		}
	}
}