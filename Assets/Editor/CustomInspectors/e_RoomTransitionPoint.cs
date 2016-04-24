using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(RoomTransitionPoint))]
public class e_RoomTransitionPoint : Editor {

	RoomTransitionPoint script;
	
	void OnEnable() {
		script = target as RoomTransitionPoint;
	}
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		serializedObject.Update();
		var openCredits = serializedObject.FindProperty("openCredits");

		if (script.gotoRoomOnTrigger == 0) {
			EditorGUILayout.PropertyField(openCredits);
		} else {
			openCredits.boolValue = false;
		}
		serializedObject.ApplyModifiedProperties();
	}

}
