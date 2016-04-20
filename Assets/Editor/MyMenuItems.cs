using UnityEngine;
using UnityEditor;
using System.Collections;
using ExtensionMethods;

public static class MyMenuItems {
	[MenuItem("Edit/Cleanup Missing Scripts")]
	static void CleanupMissingScripts() {
		string log = "Removing missing scripts...\n----------------------\n";
		int total = 0;

		for (int i = 0; i < Selection.gameObjects.Length; i++) {
			var gameObject = Selection.gameObjects[i];

			// We must use the GetComponents array to actually detect missing components
			var components = gameObject.GetComponents<Component>();

			// Create a serialized object so that we can edit the component list
			var serializedObject = new SerializedObject(gameObject);
			// Find the component list property
			var prop = serializedObject.FindProperty("m_Component");

			// Track how many components we've removed
			int r = 0;

			// Iterate over all components
			for (int j = 0; j < components.Length; j++) {
				// Check if the ref is null
				if (components[j] == null) {
					// If so, remove from the serialized component array
					prop.DeleteArrayElementAtIndex(j - r);
					// Increment removed count
					r++;
				}
			}

			if (r > 0) {
				log += r + "\tfrom\t/" + gameObject.transform.GetPath() + "\n";
				total += r;
			}

			// Apply our changes to the game object
			serializedObject.ApplyModifiedProperties();
		}

		if (Selection.gameObjects.Length == 0) {
			Debug.LogWarning("No gameobjects selected!");
		} else {
			if (total == 0) {
				log += "Removed 0 missing scripts";
			} else {
				log += "----------------------\nRemoved " + total + " missing scripts";
			}
			Debug.Log(log);
		}
	}
}