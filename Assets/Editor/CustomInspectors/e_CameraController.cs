using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CameraController))]
public class e_CameraController : Editor {

	CameraController script;

	void OnEnable() {
		script = target as CameraController;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		GUI.enabled = script.player;
		if (GUILayout.Button("Move into place") && script.player) {
			script.SnapIntoPlace();
		}
		GUI.enabled = true;
	}

}
