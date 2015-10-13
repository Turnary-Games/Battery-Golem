using UnityEngine;
using System.Collections;

public class FloatEffect : _Effect {

	public float waveAmplitude;
	public float waveLength;

	private Vector3 start;

	void Start() {
		start = transform.position;
	}

	void Update () {
		float y = Mathf.Sin (2f * Mathf.PI * Time.time / waveLength) * waveAmplitude;

		transform.position = start + Vector3.up * y;
	}
}
