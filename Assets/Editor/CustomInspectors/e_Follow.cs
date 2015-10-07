using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Follow))]
public class e_Follow : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		Follow script = target as Follow;

		EditorGUILayout.Space();

		GUI.enabled = false;
		EditorGUILayout.Vector3Field("Current offset", script.offset);
		GUI.enabled = true;
	}

}
