using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundMusic : SingletonBase<BackgroundMusic> {

	private List<Music> audioSources = new List<Music>();

	protected override void Awake() {
		base.Awake();

		foreach (var source in GetComponentsInChildren<AudioSource>())
			audioSources.Add(new Music { source = source, baseVolume = source.volume });
	}

	public void SetAudioFading(float t) {
		if (audioSources != null)
			audioSources.ForEach(a => a.source.volume = t * a.baseVolume);
	}
	
	struct Music {
		public AudioSource source;
		public float baseVolume;
	}

}
