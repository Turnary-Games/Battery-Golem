using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPushing : PlayerSubClass {

	[Header("Pushing settings")]
	
	public float breakDist = 1f;
	public float pushSpeed = 2.5f;

	[DoNotSerialize]
	public List<PushingPoint> points = new List<PushingPoint>();
	public PushingPoint point {
		get { return _point; }
		set { var old = _point; _point = value; if (_point != old) OnPointChange(old); }
	}
	private PushingPoint _point;

	public bool hasPoint { get { return point != null; } }
	public bool pointBreak { get { return hasPoint ? Mathf.Abs(point.playerPos.y - transform.position.y) > breakDist : false; } }

	void Update() {
		// Check if outside breaking distance (on the Y axis)
		if (pointBreak) {
			point = null;
		}
	}

	/// <summary>
	/// Try to grab a pushing point.
	/// </summary>
	public void TryToGrab() {

		// Error checking
		if (hasPoint) return;
		if (!movement.grounded) return;

		// Remove all invalid points. If for some reason they occur.
		points.RemoveAll(p => p == null);

		// Find the closest one
		Closest<PushingPoint> closest = PushingPoint.GetClosest(points, controller.characterCenter, interaction.ignoreYAxis);

		if (closest.valid)
			point = closest.obj;
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
		if (!hasPoint) return Vector3.zero;
		return point.playerRot;
	}

	void OnPointChange(PushingPoint old) {
		// Toggle isKinematic
		if (old)
			old.body.isKinematic = true;
		if (hasPoint)
			point.body.isKinematic = false;
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
