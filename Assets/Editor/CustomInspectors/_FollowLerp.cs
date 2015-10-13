using UnityEngine;
using UnityEditor;
using System.Collections;

/*
[CustomEditor(typeof(FollowLerp))]
public class _FollowLerp : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		FollowLerp script = target as FollowLerp;

		EditorGUILayout.Space ();
		EditorGUILayout.HelpBox (
			"The offset is set at the start if the target is valid, " +
			"but you can update it manually here if you want.",
			MessageType.Info, true);

		GUI.enabled = false;
		EditorGUILayout.Vector3Field ("Current offset", script.offset);

		GUI.enabled = !script.IsInvalid ();
		if (GUILayout.Button ("Update Offset")) {
			script.UpdateOffset();
		}

		GUI.enabled = true;

		if (!Application.isPlaying) {
			EditorGUILayout.HelpBox(
				"This offset will likely be overridden when the game starts.",
				MessageType.Warning, true);
		}
	}

}

*/