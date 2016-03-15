using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NPCController))]
public class e_NPCController : Editor {

	private ReorderableList list;
	private List<ReorderableList> sub;
	//public bool open = true;

	void OnEnable() {
		var dialogs = serializedObject.FindProperty("dialogs");
		list = new ReorderableList(serializedObject, dialogs, true, true, true, true);
		

		//list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
		//	//if (!open) return;

		//	var element = list.serializedProperty.GetArrayElementAtIndex(index);
		//	rect.y += 2;
		//	rect.height = EditorGUI.GetPropertyHeight(element) + 2;
		//	EditorGUI.PropertyField(rect, element, new GUIContent("Dialog " + (index + 1)));


		//	serializedObject.Update();
		//	list.DoLayoutList();
		//	serializedObject.ApplyModifiedProperties();
		//};

		//list.elementHeightCallback = (int index) => {
		//	//if (!open) return 0;

		//	var element = list.serializedProperty.GetArrayElementAtIndex(index);
		//	return EditorGUI.GetPropertyHeight(element) + 4;
		//};

		//list.drawHeaderCallback = (Rect rect) => {
		//	EditorGUI.LabelField(rect, serializedObject.FindProperty("dialog").displayName);
		//	//EditorGUI.indentLevel++;
		//	//open = EditorGUI.Foldout(rect, open, serializedObject.FindProperty("dialog").displayName);
		//	//EditorGUI.indentLevel--;
		//};
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

}
