using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerPushing))]
public class e_PlayerPushing : Editor {

	PlayerPushing script;

	void OnEnable() {
		script = target as PlayerPushing;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		if (script.movement != null) {
			GUI.enabled = false;
			EditorGUILayout.FloatField("movement.moveSpeed", script.movement.moveSpeed);
			GUI.enabled = true;
		}
	}

}
