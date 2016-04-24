using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtensionMethods;

[CustomEditor(typeof(ItemDataBase))]
public class e_ItemDataBase : Editor {

	UniqueId uuid;

	void OnEnable() {
		uuid = (target as ItemDataBase).GetComponent<UniqueId>();
	}
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (Application.isPlaying && GameSaveManager.roomData != null && GameSaveManager.roomData.ContainsKey(uuid.uniqueId)) {
			var data = GameSaveManager.roomData[uuid.uniqueId];

			foreach (string key in data.Keys) {
				ItemDataBase.ItemData itemData = (ItemDataBase.ItemData)data[key];
				EditorGUILayout.LabelField(itemData.scene + ": \"" + itemData.prefab + "\" " + key.Substring(7));
			}
		}

		EditorUtility.SetDirty(target);
	}

}
