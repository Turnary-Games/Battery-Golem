using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[CustomPropertyDrawer(typeof(SceneDropDown))]
public class _SceneDropDown : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);

		if (property.propertyType == SerializedPropertyType.String) {
			property.stringValue = DrawScenePopup(position, label, property.stringValue);
		} else if (property.propertyType == SerializedPropertyType.Integer) {
			property.intValue = DrawScenePopup(position, label, property.intValue);
		} else {
			EditorGUI.LabelField(position, label.text, "Use SceneDropDown with strings or integers.");
		}
		
		EditorGUI.EndProperty();
		
	}

	string DrawScenePopup(Rect position, GUIContent label, string currentScene) {
		string[] scenes = GetScenes();
		string[] sceneNames = new string[scenes.Length];
		int currentIndex = 0;

		// Find the current
		for (int i = 0; i < scenes.Length; i++) {
			sceneNames[i] = (i > 0 ? "[" + (i - 1) + "]\t" : "") + scenes[i];

			if (scenes[i] == currentScene)
				currentIndex = i;
		}

		// Draw
		int index = EditorGUI.Popup(position, label.text, currentIndex, sceneNames);

		// Get layer from name via index
		return scenes[index];
	}

	int DrawScenePopup(Rect position, GUIContent label, int currentScene) {
		string[] scenes = GetScenes();
		string[] sceneNames = new string[scenes.Length];
		int currentIndex = 0;

		// Find the current
		for (int i = 0; i < scenes.Length; i++) {
			sceneNames[i] = (i > 0 ? "[" + (i - 1) + "]\t" : "") + scenes[i];

			if (i-1 == currentScene)
				currentIndex = i;
		}

		// Draw
		int index = EditorGUI.Popup(position, label.text, currentIndex, sceneNames);

		// Get layer from name via index
		return index-1;
	}

	string[] GetScenes() {
		List<string> scenes = new List<string>();
		scenes.Add("-"); // The "no-scene-selected" option

		for (int id = 0; id < EditorBuildSettings.scenes.Length; id++) {
			var scene = EditorBuildSettings.scenes[id];

			if (scene.enabled) {
				string name = System.IO.Path.GetFileNameWithoutExtension(scene.path);

				scenes.Add(name);
			}
		}

		return scenes.ToArray();
	}

}
