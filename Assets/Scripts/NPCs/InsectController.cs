using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(_ElectricListener))]
public class InsectController : MonoBehaviour {
	
	public Animator anim;
	public List<PushingPoint> points = new List<PushingPoint>();
	public BoxCollider deactivateOnFlip;
	public float slideForce = 5;

	private States state = States.idle;

	void OnCollisionStay(Collision col) {
		var main = col.collider.GetMainObject();
		if (main.tag == "Player" && state == States.idle) {
			// Check if the player is above
			if (main.transform.position.y > transform.TransformVector(deactivateOnFlip.center).y) {
				// Push the player away!
				anim.SetTrigger("Shake");
				col.rigidbody.AddForce((main.transform.position - transform.position).SetY(0).normalized * slideForce, ForceMode.Force);
			}
		}
	}

	void OnInteractStart() {
		if (state == States.idle) {
			if (anim.IsInTransition(0)) return;

			var info = anim.GetCurrentAnimatorStateInfo(0);

			// Make sure it isn't already shaking
			if (info.IsTag("Idle"))
				anim.SetTrigger("Shake");
		}
	}

	void OnElectrifyStart() {
		if (state == States.idle) {
			state = States.flipping;
			anim.SetTrigger("Flip");
		}
	}

	// Invoked by the animation behaviour on the insect
	public void OnInsectFlipped() {
		points.ForEach(go => go.enabled = true);
		if (deactivateOnFlip)
			deactivateOnFlip.enabled = false;

		state = States.flipped;
		GetComponent<_ElectricListener>().acceptInteraction = false;
		// Reset to default
		foreach (var child in GetComponentsInChildren<Transform>())
			child.gameObject.layer = 0;
	}

	enum States {
		idle, flipping, flipped
	}

}
