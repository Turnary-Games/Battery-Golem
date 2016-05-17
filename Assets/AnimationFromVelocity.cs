using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimationFromVelocity : MonoBehaviour {

	public string speedParameter = "Speed";
	public float speedScale = 1;
	public RelativeTo relativeTo;
	private Vector3 lastPos;
	private Animator anim;

	void OnValidate() {
		speedScale = Mathf.Max(speedScale, 0.001f);
	}

	void Awake() {
		lastPos = transform.position;
		anim = GetComponent<Animator>();
	}

	void FixedUpdate() {
		Vector3 delta = (transform.position - lastPos) / Time.fixedDeltaTime;
		
		switch(relativeTo) {
			case RelativeTo.magnitude: anim.SetFloat(speedParameter, delta.magnitude / speedScale); break;
			case RelativeTo.xAxis: anim.SetFloat(speedParameter, delta.x / speedScale); break;
			case RelativeTo.yAxis: anim.SetFloat(speedParameter, delta.y / speedScale); break;
			case RelativeTo.zAxis: anim.SetFloat(speedParameter, delta.z / speedScale); break;
		}

		lastPos = transform.position;
	}

	public enum RelativeTo {
		magnitude, xAxis, yAxis, zAxis
	}
}
