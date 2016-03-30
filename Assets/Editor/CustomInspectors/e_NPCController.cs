using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NPCController))]
public class e_NPCController : Editor {

	private ReorderableList list;
	private NPCController script;

	void OnEnable() {
		script = target as NPCController;
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("dialogs"), true, true, true, true);

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			if (!list.serializedProperty.isExpanded) return;

			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.x += 15;
			rect.width -= 15;
			rect.height = EditorGUI.GetPropertyHeight(element) + 2;
			EditorGUI.PropertyField(rect, element);
		};

		list.elementHeightCallback = (int index) => {
			if (!list.serializedProperty.isExpanded) return 0;

			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			float h = EditorGUI.GetPropertyHeight(element) + 6;

			// If it's not the last one, but it's the last playOnce...
			if (element.FindPropertyRelative("messages").isExpanded && ShowLastPlayOnceStuff(index))
				h += 20;

			return h;
		};

		list.drawHeaderCallback = (Rect rect) => {
			//EditorGUI.LabelField(rect, list.serializedProperty.displayName);
			//EditorGUI.LabelField(rect, "Conversations");
			EditorGUI.indentLevel++;
			list.serializedProperty.isExpanded = EditorGUI.Foldout(rect, list.serializedProperty.isExpanded, "Conversations");
			EditorGUI.indentLevel--;

			list.draggable = 
			list.displayAdd = 
			list.displayRemove = list.serializedProperty.isExpanded;
		};
		
		list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			if (!list.serializedProperty.isExpanded) return;
			if (index < 0) return;

			bool playOnce = list.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("playOnce").boolValue;

			rect.y -= 2;
			rect.x += 1;
			rect.width -= 2;
			rect.height = list.elementHeightCallback(index);
			EditorGUI.DrawRect(rect, isFocused
				? (playOnce ? new Color(0.2f, 0.5f, 0.2f, 0.2f) : new Color(0.2f, 0.2f, 0.2f, 0.2f))
				: (playOnce ? new Color(0, 1, 0, 0.1f) : Color.clear));
			
			if (ShowLastPlayOnceStuff(index)) {
				var bot = new Rect(rect.x, rect.yMax-1, rect.width, 1);
				EditorGUI.DrawRect(bot, new Color(0, 0.5f, 0, 0.5f));
			}
		};

		list.onAddCallback = (ReorderableList l) => {
			int index = l.count;
			l.serializedProperty.InsertArrayElementAtIndex(index);
			var sub = l.serializedProperty.GetArrayElementAtIndex(index);

			sub.FindPropertyRelative("playOnce").boolValue = false;
			sub.FindPropertyRelative("messages").ClearArray();
			
			sub.isExpanded = true;
		};
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		script.headWeight = EditorGUILayout.CurveField("Head Weight", script.headWeight, Color.green, new Rect(0, 0, 180, 1));

		EditorGUILayout.Space();

		GUI.enabled = script.headBone != null;
		if (GUILayout.Button("Set Idle Angle to current euler")) {
			script.idleAngle = script.headBone.eulerAngles;
		}
		GUI.enabled = true;

		EditorGUILayout.Space();

		serializedObject.Update();
		list.DoLayoutList();
		ValidateList();
		serializedObject.ApplyModifiedProperties();
	}

	void ValidateList() {
		var prop = list.serializedProperty;

	Start:
		int nonPlayOnceIndex = -1;
		for (int i = 0; i < prop.arraySize; i++) {
			var item = prop.GetArrayElementAtIndex(i);
			var playOnce = item.FindPropertyRelative("playOnce");

			if (!playOnce.boolValue) nonPlayOnceIndex = i;
			else if (nonPlayOnceIndex != -1) {
				// A "playOnce=true" after a "playOnce=false"
				// Move up the "playOnce=true"
				prop.MoveArrayElement(i, nonPlayOnceIndex);
				// Repeat until this never happens
				goto Start;
			}
		}
	}

	bool ShowLastPlayOnceStuff(int index) {
		// Don't if it's the last one
		if (index == list.serializedProperty.arraySize - 1) return false;
		
		var prop = list.serializedProperty;

		for (int i = 0; i < prop.arraySize; i++) {
			var item = prop.GetArrayElementAtIndex(i);
			var playOnce = item.FindPropertyRelative("playOnce");
			
			// It's not even playOnce itself...
			if (i == index && !playOnce.boolValue)
				return false;
			
			// There was a playOnce after you, therefore...
			if (i > index && playOnce.boolValue)
				return false;
		}
		// No playOnce's returned, therefore...
		return true;
	}

}
