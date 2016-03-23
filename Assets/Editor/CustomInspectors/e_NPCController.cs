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

			return EditorGUI.GetPropertyHeight(element) + 2;
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

			rect.y -= 2;
			rect.x += 1;
			rect.width -= 2;
			rect.height = list.elementHeightCallback(index);
			EditorGUI.DrawRect(rect, isFocused ? new Color(0.2f, 0.2f, 0.2f, 0.2f) : Color.clear);
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

		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

}
