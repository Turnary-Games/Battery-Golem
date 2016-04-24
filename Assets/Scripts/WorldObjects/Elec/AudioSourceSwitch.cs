using UnityEngine;
using System.Collections;

[RequireComponent(typeof(_ElectricListener))]
public class AudioSourceSwitch : MonoBehaviour {

	public AudioSource audioSource;
	public Mode mode = Mode.playAndStop;
	public bool inverted = false;
	private bool isPaused;
	
	void Start() {
		SetAudioState(inverted);
	}

	void OnElectrifyStart() {
		SetAudioState(!inverted);
	}

	void OnElectrifyEnd() {
		SetAudioState(inverted);
	}

	void SetAudioState(bool on) {
		if (mode == Mode.playAndStop) {

			if (on && !audioSource.isPlaying)
				audioSource.Play();

			if (!on && audioSource.isPlaying)
				audioSource.Stop();
			
		} else if (mode == Mode.pauseAndResume) {

			if (on && !audioSource.isPlaying) {
				if (isPaused) {
					audioSource.UnPause();
					isPaused = false;
				} else {
					audioSource.Play();
				}
			}
				
			if (!on && audioSource.isPlaying) {
				audioSource.Pause();
				isPaused = true;
			}
				
		}
	}

	public enum Mode {
		playAndStop, pauseAndResume
	}

}
