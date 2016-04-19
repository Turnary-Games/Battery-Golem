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

	[SerializeThis]
	private States state = States.idle;

	[SerializeThis]
	private Vector3 startPos;
	[SerializeThis]
	private Quaternion startRot;
	[SerializeThis]
	private Quaternion startAnimRot;
	[SerializeThis]
	private int startLayer;

	void Start() {
		startPos = transform.position;
		startRot = transform.rotation;
		startAnimRot = anim.transform.localRotation;

		startLayer = gameObject.layer;
	}

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

	void OnTriggerEnter(Collider other) {
		GameObject main = other.GetMainObject();

		if (main.tag == "Water") {
			Reset();
		}
	}

	void Reset() {
		// Transform
		transform.position = startPos;
		transform.rotation = startRot;
		anim.transform.localRotation = startAnimRot;
		// State
		state = States.idle;
		// Animator
		anim.Rebind();
		// Interaction listener
		GetComponent<_ElectricListener>().acceptInteraction = true;
		// Collider
		if (deactivateOnFlip)
			deactivateOnFlip.enabled = true;
		// Pushing points
		points.ForEach(go => go.enabled = false);
		if (PlayerController.instance && points.Contains(PlayerController.instance.pushing.point))
			PlayerController.instance.pushing.point = null;
		// Layer
		foreach (var child in GetComponentsInChildren<Transform>())
			child.gameObject.layer = startLayer;
	}

	enum States {
		idle, flipping, flipped
	}

}
