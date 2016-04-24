using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringOptionsAttribute), true)]
public class _StringOptionsAttribute : PropertyDrawer {
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		if (property.propertyType == SerializedPropertyType.String) {
			EditorGUI.BeginProperty(position, label, property);

			var options = (attribute as StringOptionsAttribute).options;
			string[] display = new string[options.Length];
			if (options == null) return;

			int selected = -1;
			for (int i = 0; i < options.Length; i++) {
				if (options[i] == property.stringValue) {
					selected = i;
					break;
				}
			}

			for (int i = 0; i < options.Length; i++) {
				// Instead of forward slash /, use the division symbol ∕
				// so it doesnt create foldouts in the popup
				display[i] = options[i].Replace('/', '∕');
			}

			selected = EditorGUI.Popup(position, label.text, selected, display);
			if (selected != -1)
				property.stringValue = options[selected];
			else
				property.stringValue = "";

			EditorGUI.EndProperty();
		} else {
			EditorGUI.HelpBox(position, "StringOptions are only compatible with strings!", MessageType.Error);
		}
	}

}
