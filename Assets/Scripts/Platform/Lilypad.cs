using UnityEngine;
using System.Collections;

public class Lilypad : MonoBehaviour {

	public Rigidbody rbody;
	public Follow follow;

	public void Move(Vector3 move, float power) {
		rbody.velocity = Vector3.Lerp(rbody.velocity, move, Time.fixedDeltaTime * power) ;
	}

	void OnTouchStart(Touch touch) {
		if (IsPlayer(touch)) {
			follow.enabled = true;
			follow.targetObj = (touch.source as PlayerController).transform;
			follow.UpdateOffset();
		}
	}

	void OnTouch(Touch touch) {
		if (IsPlayer(touch)) {
			follow.UpdateOffset();
		}
	}

	void OnTouchEnd(Touch touch) {
		if (IsPlayer(touch)) {
			follow.enabled = false;
		}
	}

	bool IsPlayer(Touch touch) {
		return touch.source as PlayerController != null;
	}

}
