using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fan : _Equipable {

	[Tooltip("Top speed the lilypad will reach.")]
	public float speed = 1;
	[Tooltip("Similar to acceleration.")]
	public float power = 1;
	
	void OnElectrify() {

		var listeners = inventory.player.GetListeners();
		
		listeners.ForEach(delegate (_TouchListener listener) {
			var lilypad = listener.GetComponent<Lilypad>();
			if (lilypad != null) {
				// Move objects
				Vector3 move = -transform.forward * speed;
				lilypad.Move(move, power);
			}
		});
	}

	public override void OnEquip (PlayerInventory inventory) {
		base.OnEquip (inventory);
	}

}
