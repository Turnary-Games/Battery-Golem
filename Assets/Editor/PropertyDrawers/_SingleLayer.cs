using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SingleLayer))]
public class _SingleLayer : PropertyDrawer {


	// Draw the int as a list popup
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);

		if (property.propertyType == SerializedPropertyType.Integer) {
			property.intValue = DrawLayerPopup(position,label,property.intValue);
		} else {
			EditorGUI.LabelField(position,label.text, "Use SingleLayer with int.");
		}

		EditorGUI.EndProperty();
	}

	int DrawLayerPopup(Rect position, GUIContent label, int currentLayer) {
		int[] layers = LayerHelper.GetAllLayers();
		string[] layerNames = new string[layers.Length];
		int currentIndex = 0;
		
		// Fill names array
		for (int i=0; i<layers.Length; i++) {
			layerNames[i] = LayerMask.LayerToName(layers[i]);
			
			if (layers[i] == currentLayer)
				currentIndex = i;
		}
		
		// Draw
		int index = EditorGUI.Popup(position,label.text,currentIndex,layerNames);
		
		// Get layer from name via index
		return LayerMask.NameToLayer(layerNames[index]);
	}

}
