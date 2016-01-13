using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fan : _Equipable {

	[Header("Fan fields")]

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

		var listeners = inventory.controller.GetListeners();
		
		listeners.ForEach(delegate (_TouchListener listener) {
			var lilypad = listener.GetComponent<Lilypad>();
			if (lilypad != null) {
				// Move objects
				Vector3 move = -transform.forward * speed;
				lilypad.Move(move, power);
			}
		});
	}

}
