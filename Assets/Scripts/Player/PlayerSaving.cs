using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(UniqueId))]
public class PlayerSaving : PlayerSubClass, ISavable {

	int exitID = -1;

	public static void SetExitID(int exitID) {
		PlayerController.instance.saving.exitID = exitID;
	}

	public void OnSave(ref Dictionary<string, object> data) {
		data["player@exitID"] = exitID;

		var itemData = new ItemData {
			prefab = inventory.equipped ? inventory.equipped.prefab : null,
			uuid = inventory.equipped ? inventory.equipped.FetchUniqueID() : null,
		};
		if (itemData.prefab != null)
			print("SAVE EQUIPPED ITEM AS \"" + itemData.prefab + "\"");

		data["player@equipped"] = itemData;

		for (int i=0; i<inventory.coreItems.Length; i++) {
			var item = inventory.coreItems[i];
			data["player@coreItem#" + i] = new ItemData {
				prefab = item ? item.prefab : null,
				uuid = item ? item.FetchUniqueID() : null,
			};
		}
	}

	public void OnLoad(Dictionary<string, object> data) {
		// Goto position
		int exitID = (int)data["player@exitID"];
		
		SpawnPoint exit = SpawnPoint.GetFromID(exitID);
		if (exit) {
			transform.position = exit.transform.position;
			transform.rotation = exit.transform.rotation;
		}

		// Load coreitems
		PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
		ItemData equippedData = (ItemData)data["player@equipped"];

		for (int i = 0; i < inventory.coreItems.Length; i++) {
			ItemData itemData = (ItemData)data["player@coreItem#" + i];
			if (itemData.prefab == null) continue;

			GameObject clone = Instantiate(ItemDataBase.GetPrefab(itemData.prefab)) as GameObject;
			
			UniqueId uuid = clone.GetComponent<UniqueId>();
			uuid.uniqueId = itemData.uuid;

			_CoreItem item = clone.GetComponent<_CoreItem>();

			if (equippedData.uuid == itemData.uuid) {
				// Send to equipped
				inventory.coreItems[item.targetSlot] = item;
				inventory.MoveToEquipped(item);
				item.OnPickup();
				equippedData.prefab = null;
			} else {
				// Send to inventory
				inventory.coreItems[item.targetSlot] = item;
				inventory.MoveToInventory(item);
				item.OnPickup();
			}
		}

		if (equippedData.prefab != null) {
			// Spawn as equipped
			GameObject clone = Instantiate(ItemDataBase.GetPrefab(equippedData.prefab)) as GameObject;
			
			UniqueId uuid = clone.GetComponent<UniqueId>();
			uuid.uniqueId = equippedData.uuid;

			_Item item = clone.GetComponent<_Item>();

			inventory.MoveToEquipped(item);
			item.OnPickup();
		}
		
	}

	struct ItemData {
		public string prefab;
		public string uuid;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Home))
			GameSaveManager.SaveRoom();
		if (Input.GetKeyDown(KeyCode.End))
			LoadingScreen.LoadRoom(GameSaveManager.currentRoom);
	}

}
