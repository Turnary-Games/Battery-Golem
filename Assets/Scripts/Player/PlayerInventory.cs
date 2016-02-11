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
	
	#region Pickup item (from ground) & Equipping
	// Unequip the equipped item
	public void Unequip() {
		if (equipped == null)
			return;
		
		if (equipped.fitsInInv) {
			MoveToInventory(equipped);
		} else {
			_Equipable item = equipped;
			MoveToWorld(equipped);
			item.OnDropped();
		}

		// Tell the animator
		movement.anim.SetTrigger("ArmsEmpty");

		equipped = null;
	}

	/// <summary>
	/// Equip an item from the inventory.
	/// </summary>
	public void Equip(int slot) {
		_Equipable item = coreItems.Get(slot);

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
	public void Equip(_Equipable item) {
		if (item == null) return;

		// If theres already an item equipped
		Unequip();


		if (!item.unlocked && item.targetSlot >= 0) {
			coreItems[item.targetSlot] = item;
			item.unlocked = true;
			hud.UnlockItem(GetItemSlot(item));
			MoveToInventory(item);
		} else {
			// Move it
			MoveToEquipped(item);
		}

		item.OnPickup();
	}

    #endregion

    #region Dropoff at station
    // Dropoff at dropoff-station
    public void Dropoff<Item>(_DropoffStation<Item> station) where Item : _DropoffItem {
		Item item = equipped as Item;
		if (item != null && !equipped.fitsInInv) {
			Unequip();
			station.AddItem(item);
		}
    }
    #endregion

    #region Parenting
	void MoveToEquipped(_Equipable item) {
		// Tell animator
		if (item.fitsInInv) movement.anim.SetTrigger("ArmsHolding");
		else movement.anim.SetTrigger("ArmsLifting");

		equipped = item;

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
