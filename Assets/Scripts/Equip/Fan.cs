using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fan : _Equipable {
	
	[Header("Settings")]

	public List<EffectItem> effects = new List<EffectItem> ();

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

	public override void OnPickup (PlayerInventory inventory) {
		base.OnPickup (inventory);

		// Disable all effects
		effects.OnPickup ();
	}

	public override void OnDropped (PlayerInventory inventory) {
		base.OnDropped (inventory);

		// Enable all effects
		effects.OnDrop ();
	}

}
