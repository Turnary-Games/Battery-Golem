using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System;

public class PlayerInventory : PlayerSubClass {

	[Header("Object references")]
	
	public Transform equippedParent;

	[Header("Inventory fields")]

	public _Item equipped;

	// The size of the inventoryList defines the size of the inventory
	public _CoreItem[] coreItems = new _CoreItem[4];
	
	#region Pickup item (from ground) & Equipping
	// Unequip the equipped item
	public void Unequip() {
		if (equipped == null)
			return;
		
		if (equipped is _CoreItem) {
			MoveToInventory(equipped);
		} else {
			_Item item = equipped;
			interaction.onArmsDown = i => {
				MoveToWorld(item);
				item.OnDropped();
				i.onArmsDown = null;
			};
		}

		equipped = null;
	}

	/// <summary>
	/// Equip an item from the inventory.
	/// </summary>
	public void Equip(int slot) {
		_CoreItem item = coreItems.Get(slot);

		if (item == equipped && item != null) return;

		// If theres already an item equipped
		Unequip();

		// Move it
		if (item != null)
			MoveToEquipped(item);
	}

	/// <summary>
	/// Equip an item from the ground.
	/// </summary>
	public void Equip(_Item item) {
		if (item == null) return;

		// If theres already an item equipped
		Unequip();

		if (item is _CoreItem) {
			var core = item as _CoreItem;
			coreItems[item.targetSlot] = core;
			hud.UnlockItem(GetItemSlot(core));
			MoveToInventory(item);
		} else {
			// Move it
			MoveToEquipped(item);
		}

		item.OnPickup();
	}

    #endregion

    #region Parenting
	public void MoveToEquipped(_Item item) {
		equipped = item;

		item.transform.parent = equippedParent;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;

		if (item is _CoreItem)
			(item as _CoreItem).OnEquip(this);

		if (sound)
			sound.OnItemEquipped();
	}

	public void MoveToInventory(_Item item) {
		item.transform.parent = transform;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;

		if (item is _CoreItem)
			(item as _CoreItem).OnUnequip(this);

		if (sound)
			sound.OnItemUnequipped();
	}

	public void MoveToWorld(_Item item) {
		item.transform.parent = null;

		if (item is _CoreItem)
			(item as _CoreItem).OnUnequip(this);

		if (sound)
			sound.OnItemUnequipped();
	}
	#endregion

	public int GetItemSlot(_CoreItem item) {
		for (int slot = 0; slot < coreItems.Length; slot++) {
			if (coreItems.Get(slot) == item)
				return slot;
		}
		return -1;
	}
}
