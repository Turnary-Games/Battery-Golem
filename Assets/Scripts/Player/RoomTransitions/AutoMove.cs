using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class AutoMove : MonoBehaviour {

	public Action mode;
	[HideInInspector]
	public Vector3 vector;
	[HideInInspector]
	public Space relativeTo = Space.Self;
	
	[HideInInspector]
	public bool filter = true;
	[HideInInspector]
	public int idMustBe = -1;
	[HideInInspector]
	public int setIDTo = -1;

	Vector3 target { get { return relativeTo == Space.Self ? transform.TransformPoint(vector) : vector; } }

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		if (mode == Action.autoMove) { 
			Gizmos.DrawRay(transform.position, Vector3.up);
			Gizmos.DrawLine(transform.position + Vector3.up, target + Vector3.up);
			Gizmos.DrawRay(target, Vector3.up);
			Gizmos.DrawWireCube(target, new Vector3(1, 0, 1));
			Gizmos.DrawLine(target + new Vector3(0.5f, 0, 0.5f), target - new Vector3(0.5f, 0, 0.5f));
			Gizmos.DrawLine(target + new Vector3(-0.5f, 0, 0.5f), target - new Vector3(-0.5f, 0, 0.5f));
		}
		Gizmos.DrawWireCube(transform.position, new Vector3(1, 0, 1));
		Gizmos.DrawLine(transform.position + new Vector3(0.5f, 0, 0.5f), transform.position - new Vector3(0.5f, 0, 0.5f));
		Gizmos.DrawLine(transform.position + new Vector3(-0.5f, 0, 0.5f), transform.position - new Vector3(-0.5f, 0, 0.5f));
	}
#endif

	void OnTriggerEnter(Collider other) {
		GameObject main = other.GetMainObject();

		if (main.tag == "Player") {
			if (filter && PlayerController.instance.movement.autoMoveID != idMustBe) return;

			if (mode == Action.autoMove) {
				PlayerController.instance.movement.autoMoveTowards = target;
				if (filter) PlayerController.instance.movement.autoMoveID = setIDTo;
			} else if (mode == Action.returnControls) {
				PlayerController.instance.movement.autoMoveTowards = null;
				PlayerController.instance.movement.autoMoveID = -1;
			}
		}
	}

	public enum Action {
		autoMove, returnControls
	}

}
