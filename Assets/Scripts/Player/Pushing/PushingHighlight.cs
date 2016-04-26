using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PushingHighlight : MonoBehaviour {

	public Renderer[] renderers;
	public AudioSource audioSource;
	public float moveThreshold = 1;
	
	private Vector3 lastPos;

	void OnValidate() {
		moveThreshold = Mathf.Max(moveThreshold, 0);
	}

	void Awake() {
		lastPos = transform.position;
	}

	public void SetHighlightActive(bool state) {
		foreach (Renderer ren in renderers) {
			if (ren)
				ren.enabled = state;
		}
	}

	void FixedUpdate() {
		if (!audioSource) return;

		Vector3 delta = transform.position - lastPos;
		bool on = delta.magnitude * 50f >= moveThreshold;
		
		if (on && !audioSource.isPlaying) audioSource.Play();
		if (!on && audioSource.isPlaying) audioSource.Stop();

		lastPos = transform.position;
	}

}
