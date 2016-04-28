using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraController))]
public class CinematicCamera : MonoBehaviour {

	[Range(0,1)]
	public float speed = 1;
	public bool lockCursor;

	private CameraController cam;

	void Awake() {
		cam = GetComponent<CameraController>();
	}

	void Update() {
		Cursor.lockState = CursorLockMode.Confined;
		
		if (Input.GetMouseButton(1)) {
			Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * speed;
			Vector3 euler = transform.localEulerAngles;

			euler += new Vector3(mouseDelta.y, mouseDelta.x, 0);
			euler.x = Mathf.Clamp(euler.x, 0, 90);

			transform.localEulerAngles = euler;

			float axis = -Input.mouseScrollDelta.y;
			cam.distance = Mathf.Clamp(cam.distance + axis, 0, 50);
		}
		
	}
	
}
