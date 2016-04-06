using UnityEngine;
using UnityEditor;
using System.Collections;
using ExtensionMethods;

[CustomEditor(typeof(RigidbodyCarrier))]
public class e_RigidbodyCarrier : Editor {

	RigidbodyCarrier script;

	void OnEnable() {
		script = target as RigidbodyCarrier;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		GUI.enabled = false;

		string bodies = "";
		if (script.bodies != null && script.bodies.Count > 0) {
			foreach (var body in script.bodies) {
				bodies += body.transform.GetPath() + "\n";
			}
		}

		if (bodies.Length == 0) bodies = "(none)";

		EditorGUILayout.LabelField("bodies", bodies);

		GUI.enabled = true;
		
	}

}
