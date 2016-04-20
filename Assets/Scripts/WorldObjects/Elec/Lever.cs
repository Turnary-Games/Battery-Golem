using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	public Animator anim;
	public string parameter = "Flipped";

	[HideInInspector]
	public bool on = false;
	private bool idle = true;

	void OnValidate() {
		if (Application.isPlaying && anim && idle) {
			anim.SetBool(parameter, on);
			idle = false;
		}
	}

	void Start() {
		if (anim != null)
			anim.SetBool(parameter, on);
	}

	void OnInteractStart() {
		on = !on;
		idle = false;
		if (anim != null)
			anim.SetBool(parameter, on);
	}

	// Called by the animation
	void AnimationIdle() {
		idle = true;
	}

}
