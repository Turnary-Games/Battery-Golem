using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DynamicSwitch))]
public class e_DynamicSwitch : Editor {

	private DynamicSwitch script;

	void OnEnable() {
		script = target as DynamicSwitch;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		serializedObject.Update();

		if (script.recursive) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("filter"));
		}

		if (script.type == DynamicSwitch.Type.particleSystem) {
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("particleSwitch"));
		}

		serializedObject.ApplyModifiedProperties();
	}

}
