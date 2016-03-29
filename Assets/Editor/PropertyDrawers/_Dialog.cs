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
		return t.list.GetHeight();
	}

	// Draw the int as a list popup
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		Init(property, label);

		ThisObject t = properties[property.propertyPath];

		property.serializedObject.Update();
		EditorGUI.BeginProperty (position, label, property);

		t.list.DoList(position);

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
				if (!t.list.serializedProperty.isExpanded) return;
				EditorGUI.PropertyField(rect, t.propMessages.GetArrayElementAtIndex(index));
			};

			t.list.elementHeight = t.list.serializedProperty.isExpanded ? (t.list.count == 0 ? EditorGUIUtility.singleLineHeight : 64) : 0;
			//t.list.elementHeightCallback = (int index) => {
			//	return EditorGUI.GetPropertyHeight(t.list.serializedProperty.GetArrayElementAtIndex(index));
			//};

			t.list.drawHeaderCallback = (Rect rect) => {
				//EditorGUI.LabelField(rect, "Dialog");
				Rect rectLabel = new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
				Rect rectPlayOnce = new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2, rect.height);

				EditorGUI.indentLevel++;
				t.list.serializedProperty.isExpanded = EditorGUI.Foldout(rectLabel, t.list.serializedProperty.isExpanded, "Dialog");
				EditorGUI.indentLevel--;

				var playOnceLabel = new GUIContent(t.propPlayOnce.displayName);
				EditorGUI.BeginProperty(rectPlayOnce, playOnceLabel, t.propPlayOnce);
				//EditorGUI.PropertyField(rectPlayOnce, t.propPlayOnce);
				t.propPlayOnce.boolValue = EditorGUI.ToggleLeft(rectPlayOnce, playOnceLabel, t.propPlayOnce.boolValue);
				EditorGUI.EndProperty();

				t.list.draggable =
				t.list.displayAdd =
				t.list.displayRemove = t.list.serializedProperty.isExpanded;

				t.list.elementHeight = t.list.serializedProperty.isExpanded ? (t.list.count == 0 ? EditorGUIUtility.singleLineHeight : 64) : 0;
			};

			t.list.onAddCallback = (ReorderableList list) => {
				var prop = list.serializedProperty;
				int index = prop.arraySize;

				prop.InsertArrayElementAtIndex(index);

				// Reset the values
				var sub = prop.GetArrayElementAtIndex(index);
				sub.FindPropertyRelative("text").stringValue = string.Empty;
				sub.FindPropertyRelative("turnHead").boolValue = false;
			};

			t.list.onRemoveCallback = (ReorderableList list) => {
				if (list.count > 0) {
					list.serializedProperty.DeleteArrayElementAtIndex(list.index >= 0 ? list.index : list.count - 1);
					list.elementHeight = list.serializedProperty.isExpanded ? (list.count == 0 ? EditorGUIUtility.singleLineHeight : 64) : 0;
				}
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
