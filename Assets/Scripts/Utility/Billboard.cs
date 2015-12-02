using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

	public Camera cam;
	public bool executeInEditMode;
	
	void Update () {
		if (cam == null)
			return;

		if (!executeInEditMode && !Application.isPlaying)
			return;

		if (cam.transform == transform)
			return;

		// Source: http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
		transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
			cam.transform.rotation * Vector3.up);
		
	}
}
