using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerController))]
public class e_PlayerController : Editor {

	PlayerController script;
	private bool move;
	private Vector2 mouse;
	private RaycastHit lastHit;

	void OnEnable() {
		script = target as PlayerController;
	}

	void Update() {
		if (Event.current.type == EventType.MouseDown)
			Event.current.Use();
	}

	void OnSceneGUI() {
		if (!move) return;

		Event e = Event.current;

		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		switch (e.GetTypeForControl(controlID)) {
			case EventType.MouseDown:
				if (e.button != 0) break;
				GUIUtility.hotControl = controlID;
				e.Use();
				break;

			case EventType.MouseUp:
				if (e.button != 0) break;
				
				Undo.RecordObject(script.transform, "Move player position");
				script.transform.position = lastHit.point;
				move = false;

				GUIUtility.hotControl = 0;
				e.Use();
				break;

			case EventType.MouseMove:
				mouse = Event.current.mousePosition;
				HandleUtility.Repaint();
				break;

			case EventType.KeyDown:
				if (e.keyCode == KeyCode.Escape && move) {
					// Abort
					move = false;
				}
				break;

		}

		if (Camera.current && e.type == EventType.Repaint) {
			Ray ray = HandleUtility.GUIPointToWorldRay(mouse);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Camera.current.farClipPlane, LayerMask.GetMask("Default", "Terrain"))) {
				Handles.ArrowCap(controlID, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), 1);
				Handles.CircleCap(controlID, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), 1);
				
				foreach(var box in hit.collider.GetComponentsInChildren<BoxCollider>()) { 	
					Handles.color = Color.red;

					Vector3 pos = box.transform.position;
					Vector3 half = box.size / 2;

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(half.x, half.y, half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(half.x, half.y, -half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(half.x, half.y, -half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(-half.x, half.y, -half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(-half.x, half.y, -half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(-half.x, half.y, half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(-half.x, half.y, half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(half.x, half.y, half.z)));


					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(half.x, half.y, half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(half.x, -half.y, half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(half.x, half.y, -half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(half.x, -half.y, -half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(-half.x, half.y, half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(-half.x, -half.y, half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(-half.x, half.y, -half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(-half.x, -half.y, -half.z)));


					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(half.x, -half.y, half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(half.x, -half.y, -half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(half.x, -half.y, -half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(-half.x, -half.y, -half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(-half.x, -half.y, -half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(-half.x, -half.y, half.z)));

					Handles.DrawLine(pos + box.transform.TransformVector(box.center + new Vector3(-half.x, -half.y, half.z)),
						pos + box.transform.TransformVector(box.center + new Vector3(half.x, -half.y, half.z)));
				}

				lastHit = hit;
			}
		}
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		GUI.enabled = !move;
		if (GUILayout.Button("Move player")) {
			move = true;
		}
		GUI.enabled = true;
	}

}
