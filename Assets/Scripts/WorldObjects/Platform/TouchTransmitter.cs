using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class TouchTransmitter : MonoBehaviour {

	[HideInInspector]
	public Rigidbody body;
	public Vector3 raycastOffset;
	public LayerMask raycastMask = 1;
	public float rayLength = 0.3f;

	void Start() {
		body = GetComponent<Rigidbody>();
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.TransformPoint(raycastOffset + Vector3.up * rayLength / 2), -transform.up * rayLength);
	}
	
	void FixedUpdate() {
		RaycastHit hit;
		if (Physics.Raycast(transform.TransformPoint(raycastOffset + Vector3.up * rayLength / 2), -transform.up, out hit, rayLength, raycastMask)) {
			var main = hit.collider.GetMainObject();
			main.SendMessage(TouchMethods.Touch, this, SendMessageOptions.DontRequireReceiver);
		}
	}

}
