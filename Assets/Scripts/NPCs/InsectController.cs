﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_ElectricListener))]
public class InsectController : MonoBehaviour {
	
	public Animator anim;
	public List<PushingPoint> points = new List<PushingPoint>();
	public BoxCollider deactivateOnFlip;

	private States state = States.idle;

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
	}

	enum States {
		idle, flipping, flipped
	}

}
