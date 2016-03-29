using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TurnKinematic : MonoBehaviour {

	private Rigidbody body;
	[SerializeThis]
	private bool done = false;

	void Start() {
		body = GetComponent<Rigidbody>();
	}

	void OnCollisionStay(Collision col) {
		if (col.collider.tag == "Terrain" && !done) {
			float product = Vector3.Dot(transform.up, Vector3.down);

			if (Mathf.Approximately(Mathf.Abs(product),1)) {
				done = true;

				foreach(PushingPoint point in GetComponentsInChildren<PushingPoint>(true)) {
					point.gameObject.SetActive(true);
				}

				body.velocity = Vector3.zero;
				body.isKinematic = true;
				body.constraints += (int)RigidbodyConstraints.FreezeRotationZ + (int)RigidbodyConstraints.FreezePositionY;
				transform.rotation = Quaternion.identity;
			}
		}
	}

}
