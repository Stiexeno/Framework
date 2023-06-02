using System;
using Framework.Core;

namespace Framework.Audio
{
	[Serializable]
	public class AudioData : IData
	{
		public float volume = 1;
		public bool isSfxEnabled = true;
		public bool isMusicEnabled = true;
		public bool isHapticsEnabled = true;
	}
}