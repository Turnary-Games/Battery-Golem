using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System;

public class PlayerInventory : PlayerSubClass {

	[Header("Object references")]
	
	public Transform equippedParent;

	[Header("Inventory fields")]

	public _Equipable equipped;

	// The size of the inventoryList defines the size of the inventory
	public _Equipable[] coreItems = new _Equipable[4];

	void Update() {
		if (hud.isOpen) {
			for (int slot = 1; slot <= 4; slot++) {
				if (Input.GetButtonDown("Slot " + slot)) {
					inventory.Equip(slot - 1);
				}
			}
		}
	}

	#region Pickup item (from ground) & Equipping
	// Unequip the equipped item
	public void Unequip() {
		if (equipped == null)
			return;
		
		if (equipped.fixedInInv) {
			MoveToInventory(equipped);
		} else {
			equipped.OnDropped();
			MoveToWorld(equipped);
		}

		equipped = null;
	}

	/// <summary>
	/// Equip an item from the inventory.
	/// </summary>
	public void Equip(int slot) {
		// If theres already an item equipped
		Unequip();

		// Move it
		_Equipable item = coreItems.Get(slot);
		if (item != null)
			MoveToEquipped(item);
	}

	/// <summary>
	/// Equip an item from the ground.
	/// </summary>
	public void Equip(_Equipable item) {
		if (item == null) return;

		// If theres already an item equipped
		Unequip();

		// Move it
		item.OnPickup();
		MoveToEquipped(item);
	}

    #endregion

    #region Dropoff at station
    // Dropoff at dropoff-station
    public void Dropoff<Item>(_DropoffStation<Item> station) where Item : _DropoffItem {
		if (equipped != null && !equipped.fixedInInv) {
			_Equipable item = equipped;
			Unequip();
			station.AddItem(item as Item);
		}
    }
    #endregion

    #region Parenting
	void MoveToEquipped(_Equipable item) {
		equipped = item;

		if (item.targetSlot >= 0) coreItems[item.targetSlot] = item;
		if (!item.unlocked) { item.unlocked = true;  hud.UnlockItem(GetItemSlot(item)); }

		item.transform.parent = equippedParent;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
		item.OnEquip(this);
	}

	void MoveToInventory(_Equipable item) {
		item.transform.parent = transform;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
		item.OnUnequip(this);
	}

	void MoveToWorld(_Equipable item) {
		item.transform.parent = null;
		item.OnUnequip(this);
	}
	#endregion

	public int GetItemSlot(_Equipable item) {
		for (int slot = 0; slot < coreItems.Length; slot++) {
			if (coreItems.Get(slot) == item)
				return slot;
		}
		return -1;
	}
}
