using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	[Tooltip("The speed of which the object follows")]
	public float speed = 1;
	public float distance = 5f;
	public Vector3 offset;
	
	[HideInInspector]
	public Transform player;

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		PlayerController pc = FindObjectOfType<PlayerController>();
		if (pc == null) return;
		player = pc.transform;

		Gizmos.color = Color.red;
		Vector3 p = player.position + offset - transform.forward * distance;

		Camera cam = GetComponent<Camera>();
		if (cam) {
			Vector3 s1, f1, s2, f2, s3, f3, s4, f4;
			DrawRay(p, cam, new Vector3(0, 0, 0), out s1, out f1);
			DrawRay(p, cam, new Vector3(0, 1, 0), out s2, out f2);
			DrawRay(p, cam, new Vector3(1, 1, 0), out s3, out f3);
			DrawRay(p, cam, new Vector3(1, 0, 0), out s4, out f4);

			Gizmos.DrawLine(s1, s2);
			Gizmos.DrawLine(s2, s3);
			Gizmos.DrawLine(s3, s4);
			Gizmos.DrawLine(s4, s1);

			Gizmos.DrawLine(f1, f2);
			Gizmos.DrawLine(f2, f3);
			Gizmos.DrawLine(f3, f4);
			Gizmos.DrawLine(f4, f1);
		}

		Gizmos.DrawWireCube(player.position, new Vector3(1, 0, 1));
		Gizmos.DrawRay(player.position, offset);
		Gizmos.DrawRay(player.position + offset, -transform.forward * distance);
		
	}

	void DrawRay(Vector3 p, Camera cam, Vector3 viewport, out Vector3 start, out Vector3 finish) {
		Ray ray = cam.ViewportPointToRay(viewport);
		start = p + ray.origin - transform.position;
		finish = start + ray.direction * (distance - cam.nearClipPlane);
        Gizmos.DrawLine(start, finish);
	}
#endif

	void Start() {
		if (PlayerController.instance) {
			player = PlayerController.instance.transform;
			SnapIntoPlace();
		}
	}

	public void SnapIntoPlace() {
		transform.position = player.transform.position + offset - transform.forward * distance;
	}

	// Update is called once per frame
	void LateUpdate () {
		player = player ?? FindObjectOfType<PlayerController>().transform;
		if (player == null) return;

		transform.position = Vector3.Lerp(transform.position, player.position + offset - transform.forward * distance, speed * Time.deltaTime);
	}
}
