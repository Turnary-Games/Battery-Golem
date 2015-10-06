using UnityEngine;
using System.Collections;

public class Fan : _Equipable {

	/* TODO: Check if the player is standing on a lilypad,
	 * if so, then move the lilypad and player OnElectrify.
	 * Maybe also activate a particle system for visual feedback. 
	 */

	void OnElectrify() {
		print (Time.time);
	}

	public override void OnEquip () {
		base.OnEquip ();
	}

}
