using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AutoMove))]
public class e_AutoMove : Editor {

	AutoMove script;

	void OnEnable() {
		script = target as AutoMove;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		serializedObject.Update();

		if (script.mode == AutoMove.Action.autoMove) {
			var vector = serializedObject.FindProperty("vector");
			var relativeTo = serializedObject.FindProperty("relativeTo");

			EditorGUILayout.Space();
			script.vector = EditorGUILayout.Vector3Field(vector.displayName, script.vector);
			//EditorGUILayout.PropertyField(vector);
			script.relativeTo = (Space)EditorGUILayout.EnumPopup(relativeTo.displayName, script.relativeTo);
			//EditorGUILayout.PropertyField(relativeTo);
		}

		var filter = serializedObject.FindProperty("filter");
		var idMustBe = serializedObject.FindProperty("idMustBe");
		var setIDTo = serializedObject.FindProperty("setIDTo");

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(filter);

		GUI.enabled = script.filter;
		if (!GUI.enabled)
			idMustBe.intValue = -1;
		EditorGUILayout.PropertyField(idMustBe);
		GUI.enabled = script.filter && script.mode == AutoMove.Action.autoMove;
		if (!GUI.enabled)
			setIDTo.intValue = -1;
		EditorGUILayout.PropertyField(setIDTo);
		GUI.enabled = true;

		serializedObject.ApplyModifiedProperties();

		EditorUtility.SetDirty(target);
	}

}
