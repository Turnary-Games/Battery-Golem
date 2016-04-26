using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundMusic : SingletonBase<BackgroundMusic> {

	private List<AudioSource> audioSources;

	protected override void Awake() {
		base.Awake();
		audioSources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
	}

	public void SetAudioFading(float t) {
		if (audioSources != null)
			audioSources.ForEach(a => a.volume = t);
	}

}
