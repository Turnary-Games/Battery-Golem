using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(NPCController.Dialog))]
public class _Dialog : PropertyDrawer {

	private Dictionary<string, ThisObject> properties = new Dictionary<string, ThisObject>();

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		Init(property, label);
		
		ThisObject t = properties[property.propertyPath];

		float h = EditorGUI.GetPropertyHeight(t.propPlayOnce) + t.list.GetHeight();

		return h;
	}

	// Draw the int as a list popup
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		Init(property, label);

		ThisObject t = properties[property.propertyPath];

		property.serializedObject.Update();
		EditorGUI.BeginProperty (position, label, property);

		Rect rectPlayOnce = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(t.propPlayOnce));
		Rect rectMessages = new Rect(position.x, rectPlayOnce.yMax, position.width, t.list.GetHeight());

		EditorGUI.PropertyField(rectPlayOnce, t.propPlayOnce);

		t.list.DoList(rectMessages);

		EditorGUI.EndProperty();
		property.serializedObject.ApplyModifiedProperties();
	}

	void Init(SerializedProperty property, GUIContent label) {
		string path = property.propertyPath;

		if (!properties.ContainsKey(path)) {
			ThisObject t = new ThisObject() {
				propPlayOnce = property.FindPropertyRelative("playOnce"),
				propMessages = property.FindPropertyRelative("messages"),
				list = null
			};
			
			t.list = new ReorderableList(t.propMessages.serializedObject, t.propMessages, true, true, true, true);

			t.list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				EditorGUI.PropertyField(rect, t.propMessages.GetArrayElementAtIndex(index));
			};

			t.list.elementHeightCallback = (int index) => {
				return EditorGUI.GetPropertyHeight(t.list.serializedProperty.GetArrayElementAtIndex(index));
			};

			t.list.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(rect, property.displayName + " (index:" + property.FindPropertyRelative("index").intValue + ")");
			};

			properties[path] = t;
		}
	}

	struct ThisObject {
		public SerializedProperty propPlayOnce;
		public SerializedProperty propMessages;
		public ReorderableList list;
	}

}
