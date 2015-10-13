using UnityEngine;
using System.Collections;

public class Lilypad : MonoBehaviour {

	public Rigidbody rbody;

	private PlayerController player;

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
			player.outsideForces = rbody.velocity;
		}
	}

	void OnTouchEnd(Touch touch) {
		if (IsPlayer(touch)) {
			player = null;
		}
	}

	bool IsPlayer(Touch touch) {
		return touch.source as PlayerController != null;
	}

}
