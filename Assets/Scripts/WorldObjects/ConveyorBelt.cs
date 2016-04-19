using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class ConveyorBelt : MonoBehaviour {

	public Vector3 motion;

	void OnCollisionStay(Collision col) {
		if (col.collider.attachedRigidbody != null) {
			Rigidbody body = col.collider.attachedRigidbody;
			//Vector3[] points = new Vector3[col.contacts.Length];

			//for (int i = 0; i < points.Length; i++) {
			//body.AddForceAtPosition(force / points.Length, points[i]);
			//body.AddForce(force, ForceMode.Acceleration);
			//}

			//body.velocity = Vector3.zero;
			//body.AddForceAtPosition(motion, VectorHelper.Average(points), ForceMode.VelocityChange);

			Vector3 vel = body.velocity;

			if (motion.x != 0) vel.x = motion.x;
			if (motion.y != 0) vel.y = motion.y;
			if (motion.z != 0) vel.z = motion.z;

			body.velocity = vel;
		}
	}

}
