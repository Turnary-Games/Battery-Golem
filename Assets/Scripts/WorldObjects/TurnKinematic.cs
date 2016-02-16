using UnityEngine;
using System.Collections;

public class TurnKinematic : MonoBehaviour {

	public Rigidbody body;
	[Range(0,180)]
	public float angleError = 2;

	private bool done = false;

	void OnCollisionStay(Collision col) {
		if (col.collider.tag == "Terrain" && !done) {
			float delta = Mathf.DeltaAngle(transform.eulerAngles.z, 0);
			if (Mathf.Abs(delta) <= angleError) {
				done = true;

				foreach(PushingPoint point in GetComponentsInChildren<PushingPoint>(true)) {
					point.gameObject.SetActive(true);
				}

				body.velocity = Vector3.zero;
				body.isKinematic = true;
				body.constraints += (int)RigidbodyConstraints.FreezeRotationZ + (int)RigidbodyConstraints.FreezePositionY;
				transform.eulerAngles = Vector3.zero;
			}
		}
	}

}
