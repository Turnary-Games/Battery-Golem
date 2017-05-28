using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class ConveyorBelt : MonoBehaviour {

	public Vector3 motion;

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		var rot = Quaternion.LookRotation(transform.TransformVector(motion));

		UnityEditor.Handles.color = new Color(1, .6f, 0);
		UnityEditor.Handles.ArrowHandleCap(-1, transform.position, rot, 2f, EventType.ignore);
		UnityEditor.Handles.color = new Color(1, .3f,0);
		UnityEditor.Handles.ArrowHandleCap(-1, transform.position, rot, 2.2f, EventType.ignore);
		UnityEditor.Handles.color = new Color(1, 0, 0);
		UnityEditor.Handles.ArrowHandleCap(-1, transform.position, rot, 2.4f, EventType.ignore);
	}
#endif

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

			// Add force to it
			Vector3[] points = new Vector3[col.contacts.Length];
			for (int i = 0; i < col.contacts.Length; i++) {
				points[i] = col.contacts[i].point;
			}

			body.AddForceAtPosition(transform.TransformVector(motion), VectorHelper.Average(points), ForceMode.Acceleration);

			// Change it's velocity directly
			//Vector3 vel = body.velocity;

			//if (motion.x != 0) vel.x = motion.x;
			//if (motion.y != 0) vel.y = motion.y;
			//if (motion.z != 0) vel.z = motion.z;

			//body.velocity = vel;
		}
	}

}
