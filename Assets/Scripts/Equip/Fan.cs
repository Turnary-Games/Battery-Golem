using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fan : _Equipable {

	[Header("Settings")]
	
	public _EffectItem[] effects;

	[Space]

	[Tooltip("Top speed the lilypad will reach.")]
	public float speed = 1;
	[Tooltip("Similar to acceleration.")]
	public float power = 1;

	// Reset to lastY when dropped. Is set when picked up
	private float lastY;
	
	void OnElectrify() {
		// Error checking
		if (inventory == null)
			return;

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

	public override void OnPickupBegin(PlayerInventory inventory) {
		base.OnPickupBegin(inventory);

		lastY = transform.position.y;
	}

	public override void OnPickup (PlayerInventory inventory) {
		base.OnPickup (inventory);

		// Disable all effects
		effects.OnPickup ();
	}

	public override void OnDropped (PlayerInventory inventory) {
		base.OnDropped (inventory);

		transform.position = new Vector3(transform.position.x, lastY, transform.position.z);

		// Enable all effects
		effects.OnDrop ();
	}

}
