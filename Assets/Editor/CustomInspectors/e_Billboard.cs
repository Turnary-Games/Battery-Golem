using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Billboard))]
public class e_Billboard : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		serializedObject.Update();

		var cam = serializedObject.FindProperty("cam");
		var useMainCam = serializedObject.FindProperty("useMainCam");
		var execureInEditMode = serializedObject.FindProperty("executeInEditMode");

		if (Camera.main != null)
			cam.objectReferenceValue = Camera.main;

		GUI.enabled = !useMainCam.boolValue;
		EditorGUILayout.PropertyField(cam);
		GUI.enabled = true;

		EditorGUILayout.PropertyField(useMainCam);
		EditorGUILayout.PropertyField(execureInEditMode);

		serializedObject.ApplyModifiedProperties();
	}

}
