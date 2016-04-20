using UnityEngine;
using System.Collections;

[RequireComponent(typeof(_TouchListener))]
public class Lilypad : _Platform {

	private PlayerMovement player;

    // Called by fan
	public void Move(Vector3 move, float power) {
		body.velocity = Vector3.Lerp(body.velocity, move, Time.fixedDeltaTime * power);
	}

	void OnTouchStart(Touch touch) {
		if (IsPlayer(touch)) {
			player = touch.source as PlayerMovement;
		}
	}

	void OnTouch(Touch touch) {
		if (IsPlayer (touch)) {
			player.outsideMotion = body.velocity;
			//player.body.AddForce(body.velocity);
		}
	}

	void OnTouchEnd(Touch touch) {
		if (IsPlayer(touch)) {
			player.outsideMotion = Vector3.zero;
			player = null;
		}
	}

	bool IsPlayer(Touch touch) {
		return touch.source as PlayerMovement != null;
	}

}
