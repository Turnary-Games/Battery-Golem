using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueId))]
public class Lever : MonoBehaviour, ISavable {

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

	public void OnSave(ref Dictionary<string, object> data) {
		data["lever@on"] = on;
		data["lever@idle"] = idle;
	}

	public void OnLoad(Dictionary<string, object> data) {
		on = (bool)data["lever@on"];
		idle = (bool)data["lever@idle"];
	}
}
