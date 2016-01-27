using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[CustomPropertyDrawer(typeof(CameraDropDown))]
public class _CameraDropDown : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);

		if (property.propertyType == SerializedPropertyType.ObjectReference) {
			
			property.serializedObject.Update();
			property.objectReferenceValue = DrawCameraPopup(position, label, property.objectReferenceValue as Camera, (attribute as CameraDropDown).fullHierarchy);
			property.serializedObject.ApplyModifiedProperties();

		} else {
			EditorGUI.LabelField(position, label.text, "Use CameraDropDown with cameras.");
		}
		
		EditorGUI.EndProperty();
	}

	Camera DrawCameraPopup(Rect position, GUIContent label, Camera currentCamera, bool fullPath) {
		Camera[] cameras = GetCameras();
		string[] cameraNames = new string[cameras.Length];
		int currentIndex = 0;

		// Find the current
		for (int i = 0; i < cameras.Length; i++) {
			Camera camera = cameras[i];

			if (camera == null) {
				cameraNames[i] = "-";
			} else {
				if (fullPath) cameraNames[i] = camera.transform.GetPath();
				else cameraNames[i] = "[" + i + "]\t" + camera.name;
			}
			
			if (camera == currentCamera)
				currentIndex = i;
		}

		// Draw
		int index = EditorGUI.Popup(position, label.text, currentIndex, cameraNames);

		// Get layer from name via index
		return cameras[index];
	}

	Camera[] GetCameras() {
		List<Camera> cameras = new List<Camera>();
		cameras.Add(null); // The "no-scene-selected" option

		foreach (Camera camera in Camera.allCameras) {
			cameras.Add(camera);
		}

		return cameras.ToArray();
	}

}
