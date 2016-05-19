using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerPushing : PlayerSubClass {

	[Header("Pushing settings")]
	
	public float breakDist = 1f;
	public float pushSpeed = 2.5f;
	
	[HideInInspector]
	public List<PushingPoint> points = new List<PushingPoint>();
	public PushingPoint point {
		get { return _point; }
		set { var old = _point; _point = value; if (_point != old) OnPointChange(old); }
	}
	private PushingPoint _point;
	private PushingHighlight lastHover;

	public bool hasPoint { get { return point != null; } }

	void Update() {
		// Check if outside breaking distance (on the Y axis)
		if (!IsPointInRange(point)) {
			point = null;
		}

		// Visualization
		var closest = GetClosestPoint();
		PushingHighlight hover = IsPointInRange(closest) ? closest.highlight : null;

		if (hover != lastHover) {
			// Hover changed
			if (hover) hover.SetHighlightActive(true);
			if (lastHover) lastHover.SetHighlightActive(false);
		}
		lastHover = hover;
	}

	public bool IsPointInRange(PushingPoint point) {
		if (point == null) return false;
		return (point.playerPos - transform.position).magnitude < breakDist;
	}

	/// <summary>
	/// Try to grab a pushing point.
	/// </summary>
	public void TryToGrab() {

		// Error checking
		if (hasPoint) return;
		if (!movement.grounded) return;

		// Find the closest one
		PushingPoint closest = GetClosestPoint();

		if (IsPointInRange(closest))
			point = closest;
	}

	PushingPoint GetClosestPoint() {
		if (hasPoint) return null;
		if (inventory && inventory.equipped) return null;
		if (interaction && interaction.hover) return null;

		// Remove all invalid points. If for some reason they occur.
		points.RemoveAll(p => p == null);
		// Find the closest one
		Closest<PushingPoint> closest = PushingPoint.GetClosest(points, transform.position, interaction.ignoreYAxis);

		return closest.obj;
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
