using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomPropertyDrawer(typeof(UniqueIdentifierAttribute))]
public class UniqueIdentifierDrawer : PropertyDrawer {
	public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
		UniqueId script = prop.serializedObject.targetObject as UniqueId;
		bool isPrefabOriginal = PrefabUtility.GetPrefabParent(script.gameObject) == null && PrefabUtility.GetPrefabObject(script.transform) != null;

		if (isPrefabOriginal && prop.stringValue == "") {
			return EditorGUIUtility.singleLineHeight * 3.5f;
		}

		return EditorGUIUtility.singleLineHeight * 2;
	}

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {

		UniqueId script = prop.serializedObject.targetObject as UniqueId;
		bool isPrefabOriginal = PrefabUtility.GetPrefabParent(script.gameObject) == null && PrefabUtility.GetPrefabObject(script.transform) != null;

		Rect text = new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight);
		Rect button = new Rect(position.x, position.y + position.height - EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
		Rect remove = new Rect(button.x, button.y, EditorGUIUtility.labelWidth, button.height);
		Rect reset = new Rect(button.x + remove.width, button.y, button.width - remove.width, button.height);

		if (isPrefabOriginal && prop.stringValue == "") {
			// Is prefab.. Dont assign unique id's to prefabs

			// Place a label so it can't be edited by accident

			EditorGUI.HelpBox(text, "Care before setting an unique ID on a prefab!", MessageType.Warning);
			if (GUI.Button(button, "Set a unique ID anyway!")) {
				Guid guid = Guid.NewGuid();
				prop.stringValue = guid.ToString();
			}

			//prop.stringValue = "";
		} else if (isPrefabOriginal) {
			// Generate a unique ID, defaults to an empty string if nothing has been serialized yet

			if (GUI.Button(reset, "Set a new unique ID!")) {
				Guid guid = Guid.NewGuid();
				prop.stringValue = guid.ToString();
			}
			if (GUI.Button(remove, "Remove ID")) {
				prop.stringValue = "";
			}

			// Place a label so it can't be edited by accident
			EditorGUI.BeginProperty(text, label, prop);
			DrawLabelField(text, prop, label);
			EditorGUI.EndProperty();
		} else {
			// Generate a unique ID, defaults to an empty string if nothing has been serialized yet

			if (GUI.Button(button, "Set a new unique ID!") || prop.stringValue == "") {
				Guid guid = Guid.NewGuid();
				prop.stringValue = guid.ToString();
			}

			// Place a label so it can't be edited by accident
			EditorGUI.BeginProperty(text, label, prop);
			DrawLabelField(text, prop, label);
			EditorGUI.EndProperty();
		}
	}

	void DrawLabelField(Rect position, SerializedProperty prop, GUIContent label) {
		EditorGUI.LabelField(position, label, new GUIContent(prop.stringValue));
	}
}