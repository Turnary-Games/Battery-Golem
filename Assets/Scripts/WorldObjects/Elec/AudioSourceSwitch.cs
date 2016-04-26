using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_ElectricListener))]
public class AudioSourceSwitch : MonoBehaviour {

	public List<AudioSource> audioSource = new List<AudioSource>();
	public Mode mode = Mode.playAndStop;
	public bool inverted = false;
	private bool isPaused;
	
	public AudioSource first { get { return audioSource.Find(x => x != null); } }

	void Start() {
		SetAudioState(inverted);
	}

	void OnElectrifyStart() {
		SetAudioState(!inverted);
	}

	void OnElectrifyEnd() {
		SetAudioState(inverted);
	}

	public virtual void SetAudioState(bool on) {
		if (first == null) return;
		
		if (mode == Mode.playAndStop) {

			if (on && !first.isPlaying)
				audioSource.ForEach(x => x.Play());

			if (!on && first.isPlaying)
				audioSource.ForEach(x => x.Stop());
			
		} else if (mode == Mode.pauseAndResume) {

			if (on && !first.isPlaying) {
				if (isPaused) {
					audioSource.ForEach(x => x.UnPause());
					isPaused = false;
				} else {
					audioSource.ForEach(x => x.Play());
				}
			}
				
			if (!on && first.isPlaying) {
				audioSource.ForEach(x => x.Pause());
				isPaused = true;
			}
				
		}
	}

	public enum Mode {
		playAndStop, pauseAndResume
	}

}
