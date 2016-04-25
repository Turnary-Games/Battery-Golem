using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class ConveyorBelt : MonoBehaviour {

	public Vector3 motion;

	void OnCollisionEnter(Collision col) {
		if (col.collider.attachedRigidbody != null) {
			col.collider.attachedRigidbody.sleepThreshold = 0;
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.collider.attachedRigidbody != null) {

		}
	}

	void OnCollisionStay(Collision col) {
		if (col.collider.attachedRigidbody != null) {
			Rigidbody body = col.collider.attachedRigidbody;
			//Vector3[] points = new Vector3[col.contacts.Length];
			//for (int i = 0; i < col.contacts.Length; i++) {
			//	points[i] = col.contacts[i].point;
			//}

			//body.AddForceAtPosition(motion, VectorHelper.Average(points), ForceMode.Acceleration);
			//if (motion.MaxValue() > 0.1f)
			//	print("Add force to " + body.name);
			Vector3 vel = body.velocity;

			if (motion.x != 0) vel.x = motion.x;
			if (motion.y != 0) vel.y = motion.y;
			if (motion.z != 0) vel.z = motion.z;

			body.velocity = vel;
		}
	}

}
