using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerInventory : Inventory {

	/*

		The players inventory uses slot 0 for the equipped item
		and the rest for custom item slots.

		The player can only hold _Equipable items in its inventory.

	*/

	[Header("Variables (DONT ALTER)")]

	public PlayerController player;
	public Transform equippedParent;
	public HUD_Equipped hud_equipped;

	[HideInInspector]
	public _Equipable equipped;

	public override int capacity { get { return 4; } }

	void Start() {
		hud_equipped.SetItem(equipped);
	}

	/*
	#region Pickup item (from ground)
	public void Pickup(_Equipable item) {
		if ((equipped != null && !equipped.Equals(item as _Equipable)) || item == null)
			return;
		
		if (AddItem(item)) {
			// Equip the item
			Equip(item);
			item.OnPickup();
		}
	}

	public void Pickup(GameObject obj) {
		var item = obj.GetComponent<_Equipable>();

		if (item != null) {
			// Valid object, equip it.
			Pickup(item);
		} else {
			print("INVALID PICKUP ITEM: " + obj.name);
		}
	}
    #endregion
	*/

    #region Dropoff at station
    // Dropoff at dropoff-station
    public bool Dropoff(DropoffStation station) {
        DropoffItem item = equipped as DropoffItem;
		if (item != null) {
			RemoveItem(0);
			station.OnDropoff(item);

			return true;
		}
		return false;
    }
    #endregion

	/*
    #region Equipping
	private void EquipRaw(_Equipable item) {
		// Equip it
		equipped = item;
		// Move it
		item.transform.parent = equippedParent;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
		// Update the HUD
		hud_equipped.SetItem(equipped);
	}

    public void Equip(_Equipable item) {
		EquipRaw(item);

		// Send the event
		item.OnEquip (this);
	}

    private void UnequipRaw() {
        equipped = null;
		equipped.transform.parent = null;

        hud_equipped.SetItem(null);
    }

	public _Equipable Unequip() {
		if (equipped != null) {
			var item = equipped;
            UnequipRaw();
			
			// Send the events
			item.OnUnequip(this);
			return item;
		}
		return null;
	}
	#endregion
	*/

	#region Inventory manager
	public override void OnItemRemoved(_Item item) {
		//
	}

	public override void OnItemMoved(_Item item) {
		//
	}

	public override void OnItemAdded(_Item item) {
		//
	}

	// Algorithm for deciding which slot the item goes into
	public override int AcceptItem(_Item item) {
		// Only accept _Equipable items
		if (!(item is _Equipable))
			return -1;

		// If there is an equipped item then no
		if (slots[0] != null)
			return -1;

		// TODO
		return 0;
	}
	#endregion
}
