using UnityEngine;
using System.Collections;

public class KeepUpright : MonoBehaviour {

	public Vector3 parentOffset;

	void LateUpdate () {
		transform.rotation = Quaternion.identity;

		if (transform.parent != null)
			transform.position = transform.parent.position + parentOffset;
	}
}
