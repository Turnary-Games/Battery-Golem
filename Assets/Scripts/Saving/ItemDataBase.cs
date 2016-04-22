using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueId))]
public class ItemDataBase : SingletonBase<ItemDataBase>, ISavable {
	
	public const string PATH_FAN = "Prefabs/Items/Fan";
	public const string PATH_S1NUT = "Prefabs/Items/Nut";
	public const string PATH_S1COG = "Prefabs/Items/S1_Lost_cog";
	public const string PATH_S3ROD = "Prefabs/Items/Wheel-couple_rod";
	public const string PATH_S3WHEEL = "Prefabs/Items/Wheel-couple-wheel";

	public static GameObject prefabFan;
	public static GameObject prefabS1Nut;
	public static GameObject prefabS1Cog;
	public static GameObject prefabS3Rod;
	public static GameObject prefabS3Wheel;

	protected override void Awake() {
		if (instance == null)
			instance = this;
	}

	public void OnLoad(Dictionary<string, object> data) {
		if (instance != this) return;

		foreach (UniqueId id in FindObjectsOfType<UniqueId>()) {
			if (id.uniqueId == "") continue;
			_Item item = id.GetComponent<_Item>();
			if (!item || item.prefab == null) continue;

			Destroy(item.gameObject);
		}
		
		// Recreate all items for this room
		foreach (string key in data.Keys) {
			print("data[" + key + "]=" + data[key]);
			if (key.StartsWith("itemdb@")) {
				string id = key.Substring(7);
				ItemData item = (ItemData)data[key];

				// Skip if not for this room
				if (item.scene != GameSaveManager.currentRoom) continue;

				// Create a clone ^^
				print("Create item \"" + item.prefab.name + "\" at " + item.position);
				GameObject clone = Instantiate(item.prefab, item.position, item.rotation) as GameObject;

				//_Item script = clone.GetComponent<_Item>();

				UniqueId unique = clone.GetComponent<UniqueId>();
				unique.uniqueId = id;

				Rigidbody body = clone.GetComponent<Rigidbody>();
				body.velocity = item.velocity;
				body.angularVelocity = item.angularVelocity;
			}
		}

	}

	public void OnSave(ref Dictionary<string, object> data) {
		if (instance != this) return;

		// In case an item is removed and should no longer be stored
		// Mark it...
		List<string> killUs = new List<string>();
		foreach (string key in data.Keys) {
			if (key.StartsWith("itemdb@")) {
				ItemData item = (ItemData)data[key];
				if (item.scene == GameSaveManager.currentRoom) {
					killUs.Add(key);
					print("Remove " + key);
				}
			}
		}

		// ...to be removed from the data list
		foreach (string key in killUs)
			data.Remove(key);

		// Save the ones in this room
		foreach (UniqueId id in FindObjectsOfType<UniqueId>()) {
			if (id.uniqueId == "") continue;
			_Item item = id.GetComponent<_Item>();
			// Skip if not item
			if (!item || item.prefab == null) continue;

			// Skip inventory and held items
			if (item is _CoreItem && (item as _CoreItem).inventory) continue;
			if (PlayerController.instance.inventory.equipped == item) continue;


			data["itemdb@" + id.uniqueId] = new ItemData {
				prefab = item.prefab,
				scene =  GameSaveManager.currentRoom,
				position = item.transform.position,
				rotation = item.transform.rotation,
				velocity = item.body ? item.body.velocity : Vector3.zero,
				angularVelocity = item.body ? item.body.angularVelocity : Vector3.zero,
			};

			print("Save \"" + item.name + "\"");
		}
	}

	struct ItemData {
		public GameObject prefab;

		public int scene;
		public Vector3 position;
		public Quaternion rotation;

		public Vector3 velocity;
		public Vector3 angularVelocity;
	}

}
