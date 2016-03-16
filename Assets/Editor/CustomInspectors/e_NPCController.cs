using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NPCController))]
public class e_NPCController : Editor {

	private ReorderableList list;

	void OnEnable() {
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("dialogs"), true, true, true, true);

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			//if (!open) return;

			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.x += 15;
			rect.width -= 15;
			rect.height = EditorGUI.GetPropertyHeight(element) + 2;
			EditorGUI.PropertyField(rect, element, new GUIContent("Conversation " + (index + 1)));
			
		};

		list.elementHeightCallback = (int index) => {
			//if (!open) return 0;

			var element = list.serializedProperty.GetArrayElementAtIndex(index);

			return EditorGUI.GetPropertyHeight(element) + 6;
		};

		list.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, list.serializedProperty.displayName);
			//EditorGUI.indentLevel++;
			//open = EditorGUI.Foldout(rect, open, serializedObject.FindProperty("dialog").displayName);
			//EditorGUI.indentLevel--;
		};

		list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			rect.y -= 2;
			rect.x += 1;
			rect.width -= 2;
			rect.height = list.elementHeightCallback(index);
			EditorGUI.DrawRect(rect, isFocused ? new Color(0.2f, 0.2f, 0.2f, 0.2f) : Color.clear);
		};
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

}
