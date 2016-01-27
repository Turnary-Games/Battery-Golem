using UnityEngine;
using System.Collections;

[RequireComponent(typeof(_TouchListener))]
public class Lilypad : _Platform {

	private PlayerController player;

    // Called by fan
	public void Move(Vector3 move, float power) {
		rbody.velocity = Vector3.Lerp(rbody.velocity, move, Time.fixedDeltaTime * power);
	}

	void OnTouchStart(Touch touch) {
		if (IsPlayer(touch)) {
			player = touch.source as PlayerController;
		}
	}

	void OnTouch(Touch touch) {
		if (IsPlayer (touch)) {
			player.movement.outsideForces = rbody.velocity;
		}
	}

	void OnTouchEnd(Touch touch) {
		if (IsPlayer(touch)) {
			player.movement.outsideForces = Vector3.zero;
			player = null;
		}
	}

	bool IsPlayer(Touch touch) {
		return touch.source as PlayerController != null;
	}

}
