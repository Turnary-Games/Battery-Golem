using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fan : _Equipable {

	public float power;
	
	void OnElectrify() {

		var listeners = inventory.player.GetListeners();
		
		listeners.ForEach(delegate (_TouchListener listener) {
			var lilypad = listener.GetComponent<Lilypad>();
			if (lilypad != null) {
				// Move objects
				Vector3 move = -transform.forward * power * Time.fixedDeltaTime;
				lilypad.transform.Translate(move, Space.World);
				inventory.player.transform.Translate(move, Space.World);
			}
		});
	}

	public override void OnEquip (PlayerInventory inventory) {
		base.OnEquip (inventory);
	}

}
