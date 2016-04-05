using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerPushing))]
public class e_PlayerPushing : Editor {

	PlayerPushing script;

	void OnEnable() {
		script = target as PlayerPushing;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		if (script.movement != null) {
			GUI.enabled = false;
			EditorGUILayout.FloatField("movement.moveSpeed", script.movement.moveSpeed);

			string points = "";
			script.points.ForEach(p => {
				points += p.name + (p == script.point ? " (current)" : "") + "\n";
			});
			
			if (points.Length == 0) points = "(none)";

			EditorGUILayout.LabelField("points", points);


			// Find the closest one
			Closest<PushingPoint> closest = PushingPoint.GetClosest(script.points, script.controller.characterCenter, script.interaction.ignoreYAxis);

			if (closest.valid) {
				EditorGUILayout.LabelField("closest", closest.obj.name);
			}

			GUI.enabled = true;
		}
	}

}
