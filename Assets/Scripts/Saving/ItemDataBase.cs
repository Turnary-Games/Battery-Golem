using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(UniqueId))]
public class ItemDataBase : SingletonBase<ItemDataBase>, ISavable {

	static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	protected override void Awake() {
		if (instance == null)
			instance = this;
	}

	public void OnLoad(Dictionary<string, object> data) {
		if (instance != this && instance != null) return;

		// Skip if first time loading a scene
		if (GameSaveManager.freshLoad) return;

		// Destory all existing items
		foreach (UniqueId id in FindObjectsOfType<UniqueId>()) {
			if (id.uniqueId == "") continue;
			_Item item = id.GetComponent<_Item>();
			if (!item || item.prefab == "") continue;

			DestroyImmediate(item.gameObject);
		}
		
		// Recreate all items for this room
		foreach (string key in data.Keys) {
			if (key.StartsWith("itemdb@")) {
				string id = key.Substring(7);
				ItemData itemData = (ItemData)data[key];

				// Skip if not for this room
				if (itemData.scene != GameSaveManager.currentRoom) continue;

				// Create a clone ^^
				GameObject clone = Instantiate(GetPrefab(itemData.prefab), itemData.position, itemData.rotation) as GameObject;

				_Item script = clone.GetComponent<_Item>();
				script.startPos = itemData.startPos;
				script.startRot = itemData.startRot;
				script.can_reset = true;

				UniqueId unique = clone.GetComponent<UniqueId>();
				unique.uniqueId = id;

				Rigidbody body = clone.GetComponent<Rigidbody>();
				body.velocity = itemData.velocity;
				body.angularVelocity = itemData.angularVelocity;
			}
		}

	}

	public void OnSave(ref Dictionary<string, object> data) {
		if (instance != this && instance != null) return;

		List<string> saved = new List<string>();

		// Save the ones in this room
		foreach (UniqueId id in FindObjectsOfType<UniqueId>()) {
			if (id.uniqueId == "") continue;
			_Item item = id.GetComponent<_Item>();
			// Skip if not item
			if (!item || item.prefab == "") continue;

			// Skip inventory and held items
			if (item is _CoreItem && (item as _CoreItem).inventory) continue;
			if (PlayerController.instance.inventory.equipped == item) continue;

			data["itemdb@" + id.uniqueId] = new ItemData {
				prefab = item.prefab,
				scene = GameSaveManager.currentRoom,
				position = item.transform.position,
				rotation = item.transform.rotation,
				velocity = item.body ? item.body.velocity : Vector3.zero,
				angularVelocity = item.body ? item.body.angularVelocity : Vector3.zero,
				startPos = item.startPos,
				startRot = item.startRot,
			};

			// Remember that we saved this one
			saved.Add(id.uniqueId);
		}

		List<string> killUs = new List<string>();

		foreach (string key in data.Keys) {
			if (key.StartsWith("itemdb@")) {
				string id = key.Substring(7);
				ItemData itemData = (ItemData)data[key];

				if (itemData.scene == GameSaveManager.currentRoom && !saved.Contains(id)) {
					// Is saved to be in this room, but was not here this time, which means it got removed
					// If it jumped to different scene then the itemData.scene should be different
					killUs.Add(key);
				}
			}
		}

		foreach (string key in killUs)
			data.Remove(key);
	}

	public static GameObject GetPrefab(string path) {
		if (prefabs.ContainsKey(path)) {
			return prefabs[path];
		} else {
			GameObject prefab = Resources.Load<GameObject>(path);
			if (prefab == null)
				Debug.LogError("Unable to load prefab at \"" + path + "\"!");
			prefabs[path] = prefab;
			return prefab;
		}
	}

	public static void RemoveFromWorld(string uuid) {
		string myUUID = instance.FetchUniqueID();

		var data = GameSaveManager.roomData[myUUID];
		data.Remove("itemdb@" + uuid);
		GameSaveManager.roomData[myUUID] = data;
	}

	public struct ItemData {
		public string prefab;

		public int scene;
		public Vector3 position;
		public Quaternion rotation;

		public Vector3 velocity;
		public Vector3 angularVelocity;

		public Vector3 startPos;
		public Quaternion startRot;
	}

}
