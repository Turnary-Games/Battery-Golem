using UnityEngine;
using System.Collections;

public class RotateEffect : _Effect {

	public Vector3 velocity;
	public Space relativeTo;

	void Update() {
		transform.Rotate(velocity * Time.deltaTime, relativeTo);
	}

}

