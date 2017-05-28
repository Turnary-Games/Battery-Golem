using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtensionMethods;

[CustomEditor(typeof(NPCDialogBox))]
public class e_NPCDialogBox : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		serializedObject.Update();

		var min = serializedObject.FindProperty("randomPitchMin");
		var max = serializedObject.FindProperty("randomPitchMax");

		float minValue = min.floatValue;
		float maxValue = max.floatValue;

		Rect pitchRect = EditorGUILayout.GetControlRect();
		EditorGUI.BeginProperty(pitchRect, new GUIContent(min.displayName), min);
		EditorGUI.BeginProperty(pitchRect, new GUIContent(max.displayName), max);
		EditorGUI.MinMaxSlider(pitchRect, new GUIContent("Random Pitch"), ref minValue, ref maxValue, -3, 3);
		EditorGUI.EndProperty();
		EditorGUI.EndProperty();
		
		Rect wut = EditorGUILayout.GetControlRect();

		Rect labelRect = new Rect(wut.x, wut.y, EditorGUIUtility.labelWidth, wut.height);
		Rect buttonRect = new Rect(labelRect.xMax, wut.y, wut.width - labelRect.width, wut.height);

		if (GUI.Button(buttonRect, "Reset min/max")) {
			minValue = 1;
			maxValue = 1;
		}
		GUI.enabled = false;
		EditorGUI.LabelField(labelRect, "min=" + Round(minValue) + "; max=" + Round(maxValue));
		GUI.enabled = true;

		min.floatValue = minValue;
		max.floatValue = maxValue;


		serializedObject.ApplyModifiedProperties();
	}

	float Round(float v) {
		return Mathf.Round(v * 100f) * 0.01f;
	}

}
