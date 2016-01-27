using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

	[HideInInspector]
	[CameraDropDown]
	public Camera cam;
	[HideInInspector]
	public bool useMainCam;
	[HideInInspector]
	public bool executeInEditMode;
	
	private Camera _cam { get { return useMainCam && Camera.main != null ? Camera.main : cam; } }

	void Update () {
		if (_cam == null)
			return;

		if (!executeInEditMode && !Application.isPlaying)
			return;

		if (_cam.transform == transform)
			return;

		// Source: http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
		transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward,
			_cam.transform.rotation * Vector3.up);
		
	}
}
