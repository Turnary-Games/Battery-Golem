using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPushing : PlayerSubClass {

	[Header("Pushing settings")]
	
	public float breakDist = 1f;
	public float pushSpeed = 2.5f;

	private List<PushingPoint> points = new List<PushingPoint>();
	[HideInInspector]
	public PushingPoint point {
		get { return _point; }
		set { var old = _point; _point = value; if (_point != old) OnPointChange(old); }
	}
	private PushingPoint _point;

	void Update() {
		// Check if outside breaking distance (on the Y axis)
		if (point != null && Mathf.Abs(point.playerPos.y - transform.position.y) > breakDist) {
			point = null;
		}
	}

	/// <summary>
	/// Try to grab/drop a pushing point.
	/// Returns true if grabbed/dropped.
	/// Returns false if nothing happened.
	/// </summary>
	public bool GrabNDrop() {
		if (point) {
			// Drop current point
			point = null;
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

	public Vector3 GetMovement() {
		if (point == null) return Vector3.zero;

		// Movement towards point
		Vector3 delta = point.playerPos - transform.position;
		delta.y = 0;
		delta = Vector3.ClampMagnitude(delta, 1) * movement.moveSpeed;

		// Player movement
		Vector3 axis = movement.GetAxis();
		if (!point.x) axis.x = 0;
		if (!point.z) axis.z = 0;

		delta += axis * pushSpeed;

		// Move box, but keep the y velocity
		point.body.velocity = axis * pushSpeed + Vector3.up * point.body.velocity.y;

		return delta;
	}

	public Vector3 GetAxis() {
		if (point == null) return Vector3.zero;
		return point.playerRot;
	}

	void OnPointChange(PushingPoint old) {
		if (point) movement.anim.SetTrigger("ArmsHolding");
		else movement.anim.SetTrigger("ArmsEmpty");
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
