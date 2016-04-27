using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(RoomTransitionPoint))]
public class e_RoomTransitionPoint : Editor {

	RoomTransitionPoint script;

	void OnEnable() {
		script = target as RoomTransitionPoint;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		serializedObject.Update();
		
		var filter = serializedObject.FindProperty("filter");
		var idMustBe = serializedObject.FindProperty("idMustBe");

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(filter);

		GUI.enabled = script.filter;
		if (!GUI.enabled)
			idMustBe.intValue = -1;
		EditorGUILayout.PropertyField(idMustBe);
		GUI.enabled = true;

		serializedObject.ApplyModifiedProperties();

		EditorUtility.SetDirty(target);
	}

}
