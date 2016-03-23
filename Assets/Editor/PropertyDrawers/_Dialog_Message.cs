using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(NPCController.Dialog.Message))]
public class _Dialog_Message : PropertyDrawer {

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		var propText = property.FindPropertyRelative("text");
		var propTurnHead = property.FindPropertyRelative("turnHead");

		return EditorGUI.GetPropertyHeight(propText) + EditorGUI.GetPropertyHeight(propTurnHead);
	}

	// Draw the int as a list popup
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		var propTurnHead = property.FindPropertyRelative("turnHead");
		var propText = property.FindPropertyRelative("text");

		Rect rectTurnHead = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(propTurnHead));
		Rect rectText = new Rect(position.x, rectTurnHead.yMax - EditorGUIUtility.singleLineHeight + 3, position.width, EditorGUI.GetPropertyHeight(propText));

		EditorGUI.BeginProperty(rectTurnHead, new GUIContent(propTurnHead.displayName), propTurnHead);
		EditorGUI.PropertyField(rectTurnHead, propTurnHead);
		//propTurnHead.boolValue = EditorGUI.ToggleLeft(rectTurnHead, propTurnHead.displayName, propTurnHead.boolValue);
		EditorGUI.EndProperty();
		EditorGUI.PropertyField(rectText, propText, GUIContent.none);
	}
}
