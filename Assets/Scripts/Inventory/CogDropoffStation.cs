using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CogDropoffStation : _DropoffStation<LostCog> {

	public Gate gate;

	void OnInteractStart(PlayerController player) {
		player.inventory.Dropoff(station:this);
	}

	public override void OnItemAdded(OnItemAddedEvent _event) {
		base.OnItemAdded(_event);
		gate.OnCogDropoff();
	}
}
