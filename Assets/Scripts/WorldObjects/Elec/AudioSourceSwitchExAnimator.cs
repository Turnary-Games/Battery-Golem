using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_ElectricListener))]
public class AudioSourceSwitchExAnimator : AudioSourceSwitch {

	[Header("Ex")]
	public Animator anim;
	[Space]
	public bool volumeEqualsSpeed;
	public string changeVolumeTag = "Start";
	public bool volumeTagMatch = false;
	[Space]
	public string playTag = "Done";
	public bool playTagMatch = false;
	private bool on = false;
	
	void FixedUpdate() {
		var info = anim.GetCurrentAnimatorStateInfo(0);

		// On/off
		bool playSnd = info.IsTag(playTag);
		if (!playTagMatch) playSnd = !playSnd;

		base.SetAudioState(on && playSnd);

		// Volume
		if (volumeEqualsSpeed) {
			bool changeVolume = info.IsTag(changeVolumeTag);
			if (!volumeTagMatch) changeVolume = !changeVolume;

			audioSource.ForEach(a => {
				if (a) a.volume = changeVolume ? info.speed * info.speedMultiplier : 0;
			});
		}
	}

	public override void SetAudioState(bool on) {
		this.on = on;
	}

}
