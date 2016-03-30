using UnityEngine;
using System.Collections;

public class KeepUpright : MonoBehaviour {

	public Vector3 worldRotation;

	void LateUpdate () {
		transform.eulerAngles = worldRotation;
	}
}
