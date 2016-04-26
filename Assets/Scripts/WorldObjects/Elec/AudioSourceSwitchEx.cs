using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_ElectricListener))]
public class AudioSourceSwitchEx : AudioSourceSwitch {
	
	[Header("Ex")]
	public Animator anim;
	public string animTag = "Done";
	public bool tagMatch = false;
	private bool on = false;
	
	void FixedUpdate() {
		bool matches = anim.GetCurrentAnimatorStateInfo(0).IsTag(animTag);
		if (!tagMatch) matches = !matches;

		base.SetAudioState(on && matches);
	}

	public override void SetAudioState(bool on) {
		this.on = on;
	}

}
