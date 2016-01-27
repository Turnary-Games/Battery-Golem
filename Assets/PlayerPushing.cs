using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPushing : PlayerSubClass {

	public List<PushingPoint> points = new List<PushingPoint>();
	public PushingPoint point;

	/// <summary>
	/// Try to grab/drop a pushing point.
	/// Returns true if grabbed/dropped.
	/// Returns false if nothing happened.
	/// </summary>
	public bool GrabNDrop() {
		if (point) {
			// Drop current point
			point = null;
			movement.outsideForces = Vector3.zero;
			return true;
		} else {
			// Grab a point

			// Remove all invalid points. If for some reason they occur.
			points.RemoveAll(delegate (PushingPoint p) {
				return p == null;
			});

			// Find the closest one
			Closest<PushingPoint> closest = PushingPoint.GetClosest(points, controller.characterCenter, interaction.ignoreYAxis);

			if (closest.valid) {
				point = closest.obj;
				return true;
			}
		}
		return false;
	}

	public void SetMovement() {
		if (point == null) return;

		Vector3 delta = point.playerPos - transform.position;
		delta.y = 0;
		movement.outsideForces = Vector3.ClampMagnitude(delta, 1) * movement.moveSpeed;
	}

	public Vector3 GetAxis() {
		if (point == null) return Vector3.zero;
		return point.playerRot;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Push point") {
			PushingPoint point = other.GetComponent<PushingPoint>();
			if (point != null && !points.Contains(point)) {
				points.Add(point);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Push point") {
			PushingPoint point = other.GetComponent<PushingPoint>();
			if (point != null) {
				int index = points.IndexOf(point);
				if (index >= 0)
					points.RemoveAt(index);
			}
		}
	}

}
