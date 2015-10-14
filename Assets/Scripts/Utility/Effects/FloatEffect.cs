using UnityEngine;
using System.Collections;

public class FloatEffect : _Effect {

	public float waveAmplitude;
	public float waveLength;
	public Space relativeTo;

	private Vector3 start;

	void Start() {
		start = relativeTo == Space.World ? transform.position : transform.localPosition;
	}

	void Update () {
		float y = Mathf.Sin (2f * Mathf.PI * Time.time / waveLength) * waveAmplitude;

		if (relativeTo == Space.World)
			transform.position = start + Vector3.up * y;
		else
			transform.localPosition = start + Vector3.up * y;
	}
}
