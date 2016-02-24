using UnityEngine;
using System.Collections;
using System;

public class Checkpoint : MonoBehaviour {

	public static Checkpoint last;

	public Renderer flag;

	void Start() {
		flag.enabled = last == this;
	}

	void OnTriggerEnter(Collider other) {
		if (LevelSerializer.IsDeserializing) return;

		GameObject main = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

		if (main.tag == "Player") {
			if (last != this) {
				// Save checkpoint
				flag.enabled = true;
				LevelSerializer.Checkpoint();
				last = this;
				print("Saved!");
			}
		}
	}
}
