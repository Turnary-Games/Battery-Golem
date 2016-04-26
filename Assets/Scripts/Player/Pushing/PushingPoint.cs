using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class PushingPoint : Searchable {
	
	public Rigidbody body;
	public BoxCollider col;

	public bool x;
	public bool z;
	public Vector3 offset;
	public float rotation;

	public Vector3 playerPos { get { return transform.TransformPoint(offset + (col ? col.center - Vector3.up * col.size.y / 2 : Vector3.zero)); } }
	public Vector3 playerRot { get { var vec = VectorHelper.FromDegrees(rotation); return transform.TransformVector(new Vector3(vec.x, 0, vec.y)).normalized; } }

	public override bool valid { get { return isActiveAndEnabled; } }

	[HideInInspector]
	public PushingHighlight highlight;

	protected override void Awake() {
		// Searchable awake function
		base.Awake();

		// Get the parent pushing highlight, if any
		highlight = body.GetComponent<PushingHighlight>();
	}

	void Start() {}

#if UNITY_EDITOR
	void OnDrawGizmos() {
		if (!transform.IsSelected()) return;

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(playerPos, 0.1f);
		Gizmos.DrawRay(playerPos + Vector3.up * 0.1f, Vector3.up);
		Gizmos.DrawRay(playerPos + Vector3.up * 1.1f, playerRot / 2);
		
		//if (col) {
		//	Gizmos.color = body == null ? Color.red : Color.green;
		//	Gizmos.DrawWireCube(transform.TransformPoint(col.center), transform.TransformVector(col.size));
		//}

		if (z) {
			UnityEditor.Handles.color = Color.blue;
			UnityEditor.Handles.ArrowCap(-1, playerPos, Quaternion.LookRotation(Vector3.forward), 0.5f);
		}
		if (x) {
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.ArrowCap(-1, playerPos, Quaternion.LookRotation(Vector3.right), 0.5f);
		}
	}
#endif

	public static Closest<PushingPoint> GetClosest(List<PushingPoint> list, Vector3 point, bool ignoreY = false) {
		return new Closest<PushingPoint>(list, point, ignoreY);
	}

	public override float GetDistance(Vector3 from, bool ignoreY = false) {
		Vector3 to = playerPos;
		if (ignoreY) from.y = to.y = 0;

		return Vector3.Distance(from, to);
	}
}
