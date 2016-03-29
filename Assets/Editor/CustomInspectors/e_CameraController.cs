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
			script.transform.position = script.player.transform.position + script.offset - script.transform.forward * script.distance;
		}
		GUI.enabled = true;
	}

}
