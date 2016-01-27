using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class PushingPoint : Searchable {

	public bool x;
	public bool z;
	public Vector3 offset;
	public float rotation;

	public bool playerInside = false;

	public Vector3 playerPos { get { return transform.TransformPoint(offset); } }
	public Vector3 playerRot { get { var vec = VectorHelper.FromDegrees(rotation); return new Vector3(vec.x, 0, vec.y); } }

#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(playerPos, 0.2f);
		Gizmos.DrawRay(playerPos + Vector3.up*0.2f, Vector3.up);
		Gizmos.DrawRay(playerPos + Vector3.up * 1.2f, playerRot);

		BoxCollider col = GetComponent<BoxCollider>();
		if (col) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.TransformPoint(col.center), transform.TransformVector(col.size));
        }

		Vector3 pos = transform.TransformPoint(col.center) - Vector3.up * transform.TransformVector(col.size).y / 2;

		if (z) {
			UnityEditor.Handles.color = Color.blue;
			UnityEditor.Handles.ArrowCap(-1, pos, Quaternion.LookRotation(Vector3.forward), 0.5f);
		}
		if (x) {
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.ArrowCap(-1, pos, Quaternion.LookRotation(Vector3.right), 0.5f);
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
