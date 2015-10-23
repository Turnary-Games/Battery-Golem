using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomPropertyDrawer(typeof(_EffectItem))]
public class EffectPropertyDrawer : PropertyDrawer {

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		// + 0.5 line
		return base.GetPropertyHeight(property, label) * (GetHeightMultiplier(property, label) + 0.5f);
	}

	public float GetHeightMultiplier(SerializedProperty property, GUIContent label) {
		// Type of effect
		_EffectItem.Type type = (_EffectItem.Type)property.FindPropertyRelative("type").enumValueIndex;

        switch (type) {
			case _EffectItem.Type.resetTransform:
				return 3f;

			case _EffectItem.Type.effect:
				return 2f;

            case _EffectItem.Type.gameObject:
                return 2f;

			default:
				return 1f;
		}
	}

	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		// Type of effect
		_EffectItem.Type type = (_EffectItem.Type)property.FindPropertyRelative("type").enumValueIndex;


		EditorGUI.BeginProperty(position, label, property);
		EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Calculate label rect
		var typeProperty = property.FindPropertyRelative("type");
		var typeContent = new GUIContent("Type");
        var typeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, EditorGUIUtility.labelWidth - 5f, EditorGUIUtility.singleLineHeight);
		var typeLabel = new Rect(typeRect.x, typeRect.y, 55f, typeRect.height);
		var typeEnum = new Rect(typeRect.x + 40f, typeRect.y, typeRect.width - 40f, typeRect.height);

		// Draw label
		EditorGUI.BeginProperty(typeRect, typeContent, typeProperty);
		EditorGUI.LabelField(typeLabel, typeContent);
		EditorGUI.PropertyField(typeEnum, typeProperty, GUIContent.none);
		EditorGUI.EndProperty();

		if (type == _EffectItem.Type.resetTransform) {
			// Calculate relativeto rect
			var relProperty = property.FindPropertyRelative("resetRelativeTo");
			var relContent = new GUIContent("Relative To");
			var relRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2f, EditorGUIUtility.labelWidth - 5f, EditorGUIUtility.singleLineHeight);
			var relLabel = new Rect(relRect.x, relRect.y, 55f, relRect.height);
			var relEnum = new Rect(relRect.x + 40f, relRect.y, relRect.width - 40f, relRect.height);

			// Draw relativeto
			EditorGUI.BeginProperty(relRect, relContent, relProperty);
			EditorGUI.LabelField(relLabel, relContent);
			EditorGUI.PropertyField(relEnum, relProperty, GUIContent.none);
			EditorGUI.EndProperty();
		}

		// Don't make child fields be indented
		position = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;


		if (type == _EffectItem.Type.effect || type == _EffectItem.Type.gameObject) {
            // What
            string objectRef = type == _EffectItem.Type.effect ? "effect" : "toggleGameObject";
            string pickUpRef = type == _EffectItem.Type.effect ? "effectOnPickup" : "toggleOnPickup";
            string dropRef = type == _EffectItem.Type.effect ? "effectOnDrop" : "toggleOnDrop";
            float labelWidth = type == _EffectItem.Type.effect ? 45f : 80f;

            // Calculate rects
            var halfw = position.width / 2f;
			var height = EditorGUIUtility.singleLineHeight;

			// Effect object
			var objProperty = property.FindPropertyRelative(objectRef);
			var objRect = new Rect(position.x, position.y, position.width, height);
			var objLabel = new Rect(objRect.x, objRect.y, labelWidth, objRect.height);
			var objObj = new Rect(objRect.x + objLabel.width, objRect.y, objRect.width - objLabel.width, objRect.height);

			// Pickup
			var onPickupProperty = property.FindPropertyRelative(pickUpRef);
            var onPickupRect = new Rect(position.x, position.y + height, halfw, height);
			var onPickupLabel = new Rect(onPickupRect.x, onPickupRect.y, 60f, onPickupRect.height);
			var onPickupEnum = new Rect(onPickupRect.x + 60f, onPickupRect.y, onPickupRect.width - 60f, onPickupRect.height);

			// Drop
			var onDropProperty = property.FindPropertyRelative(dropRef);
            var onDropRect = new Rect(position.x + halfw, position.y + height, halfw, height);
			var onDropLabel = new Rect(onDropRect.x, onDropRect.y, 55f, onDropRect.height);
			var onDropEnum = new Rect(onDropRect.x + 55f, onDropRect.y, onDropRect.width - 55f, onDropRect.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			EditorGUI.BeginProperty(objRect, new GUIContent(objectRef), objProperty);
			EditorGUI.LabelField(objLabel, type == _EffectItem.Type.effect ? "effect:" : "gameobject:");
			EditorGUI.PropertyField(objObj, objProperty, GUIContent.none);
			EditorGUI.EndProperty();

			EditorGUI.BeginProperty(onPickupRect, new GUIContent(pickUpRef), onPickupProperty);
			EditorGUI.LabelField(onPickupLabel, "onPickup:");
			EditorGUI.PropertyField(onPickupEnum, onPickupProperty, GUIContent.none);
			EditorGUI.EndProperty();

			EditorGUI.BeginProperty(onDropRect, new GUIContent(dropRef), onDropProperty);
			EditorGUI.LabelField(onDropLabel, " onDrop:");
			EditorGUI.PropertyField(onDropEnum, onDropProperty, GUIContent.none);
			EditorGUI.EndProperty();

		} else if (type == _EffectItem.Type.resetTransform) {

			// Calculate rects
			var height = EditorGUIUtility.singleLineHeight;

			// Affected transform
			var transformProperty = property.FindPropertyRelative("resetTransform");
			var transformRect = new Rect(position.x, position.y, position.width, height);
            var transformLabel = new Rect(transformRect.x, transformRect.y, 45f, transformRect.height);
			var transformObj = new Rect(transformRect.x + 45f, transformRect.y, transformRect.width - transformLabel.width, transformRect.height);

			EditorGUI.BeginProperty(transformRect, new GUIContent("resetTransform"), transformProperty);
			EditorGUI.LabelField(transformLabel, "effect:");
			EditorGUI.PropertyField(transformObj, transformProperty, GUIContent.none);
			EditorGUI.EndProperty();

			// Boolean labels
			for (int i=1; i<=2; i++) {

				var resetProperty = property.FindPropertyRelative(i == 1 ? "resetOnPickup" : "resetOnDrop");
                var labelRect = new Rect(position.x, position.y + i * height, 70f, height);
				var boolRect = new Rect(position.x + labelRect.width, labelRect.y, position.width - labelRect.width, labelRect.height);

				// Draw
				EditorGUI.BeginProperty(position, label, resetProperty);
				EditorGUI.LabelField(labelRect, i == 1 ? "onPickup:" : "onDrop:");
				EditorGUI.PropertyField(boolRect, resetProperty, GUIContent.none);
				EditorGUI.EndProperty();
            }
		}

		// Reset indent level
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}

}



[CustomPropertyDrawer(typeof(_EffectItem.ResetTransform))]
public class EffectStructPropertyDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		// Calculate rects
		var width = position.width / 3f;
		var labelWidth = width - position.height;
		var boolWidth = position.height;

		var posProperty = property.FindPropertyRelative("position");
		var posContent = new GUIContent("position");
        var posRect = new Rect(position.x, position.y, width, position.height);
        var posLabel = new Rect(posRect.x, posRect.y, labelWidth, posRect.height);
		var posBool = new Rect(posLabel.max.x, posRect.y, boolWidth, posRect.height);

		var rotProperty = property.FindPropertyRelative("rotation");
		var rotContent = new GUIContent("rotation");
		var rotRect = new Rect(position.x + width, position.y, width, position.height);
		var rotLabel = new Rect(rotRect.x, rotRect.y, labelWidth, rotRect.height);
		var rotBool = new Rect(rotLabel.max.x, rotRect.y, boolWidth, rotRect.height);

		var scaProperty = property.FindPropertyRelative("scale");
		var scaContent = new GUIContent("scale");
		var scaRect = new Rect(position.x + width * 2f, position.y, width, position.height);
		var scaLabel = new Rect(scaRect.x, scaRect.y, labelWidth, scaRect.height);
		var scaBool = new Rect(scaLabel.max.x, scaRect.y, boolWidth, scaRect.height);


		// Draw
		EditorGUI.BeginProperty(posRect, posContent, posProperty);
		EditorGUI.LabelField(posLabel, posContent);
		EditorGUI.PropertyField(posBool, posProperty, GUIContent.none);
		EditorGUI.EndProperty();

		EditorGUI.BeginProperty(rotRect, rotContent, rotProperty);
		EditorGUI.LabelField(rotLabel, rotContent);
		EditorGUI.PropertyField(rotBool, rotProperty, GUIContent.none);
		EditorGUI.EndProperty();

		EditorGUI.BeginProperty(scaRect, scaContent, scaProperty);
		EditorGUI.LabelField(scaLabel, scaContent);
		EditorGUI.PropertyField(scaBool, scaProperty, GUIContent.none);
		EditorGUI.EndProperty();

	}

}