using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_ElectricListener))]
public class AudioSourceSwitchExDropoff : AudioSourceSwitch {

	[Header("Ex")]
	public BasicItemDropoff station;
	public bool waitForItem = true;

	public override void SetAudioState(bool on) {
		bool active = station.activated;
		if (!waitForItem) active = !active;
		base.SetAudioState(on && active);
	}

}
