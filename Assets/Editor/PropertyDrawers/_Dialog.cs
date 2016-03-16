using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(NPCController.Dialog))]
public class _Dialog : PropertyDrawer {

	private ReorderableList list;
	private SerializedProperty propPlayOnce;
	private SerializedProperty propMessages;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		Init(property, label);

		float h = EditorGUI.GetPropertyHeight(propPlayOnce) + list.GetHeight();

		//for (int i=0; i<propMessages.arraySize; i++) {
		//	h += EditorGUI.GetPropertyHeight(propMessages.GetArrayElementAtIndex(i));
		//}

		return h;
	}

	// Draw the int as a list popup
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		Init(property, label);

		EditorGUI.BeginProperty (position, label, property);

		Rect rectPlayOnce = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(propPlayOnce));
		Rect rectMessages = new Rect(position.x, rectPlayOnce.yMax, position.width, list.GetHeight());

		EditorGUI.PropertyField(rectPlayOnce, propPlayOnce);

		property.serializedObject.Update();
		list.DoList(rectMessages);
		property.serializedObject.ApplyModifiedProperties();

		EditorGUI.EndProperty();
	}

	void Init(SerializedProperty property, GUIContent label) {
		propPlayOnce = propPlayOnce ?? property.FindPropertyRelative("playOnce");
		propMessages = propMessages ?? property.FindPropertyRelative("messages");

		if (list == null) {
			list = new ReorderableList(propMessages.serializedObject, propMessages, true, true, true, true);


		}
	}

}
