using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerInventory : Inventory<_Equipable> {

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
	public _Equipable equipped {
		get { return slots.Length > 0 ? slots[0] as _Equipable : null; }
		set { slots[0] = value; }
	}

	// The size of the inventoryList defines the size of the inventory
	private _Equipable[] inventoryList = new _Equipable[4];
	public override _Equipable[] slots { get { return inventoryList; } }

	void Start() {
		hud_equipped.SetItem(equipped);
	}
	
	#region Pickup item (from ground)
	public void Pickup(_Equipable item) {
		if (equipped != null)
			return;

		if (item == null)
			return;
		
		if (AddItem(item)) {
			// Equip the item
			item.OnPickup();
		}
	}

	public void Pickup(GameObject obj) {
		Pickup(obj.GetComponent<_Equipable>());
	}
    #endregion

    #region Dropoff at station
    // Dropoff at dropoff-station
    public bool Dropoff(DropoffStation station) {
		return TransferItem(0, station);
    }
    #endregion

    #region Equipping
	void MoveToEquipped(_Equipable item) {
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
	#endregion

	#region Inventory manager
	public override void OnItemRemoved(OnItemRemovedEvent _event) {
		// Set it free
		_event.item.transform.parent = null;
		if (_event.index == 0)
			_event.item.OnUnequip(this);
		hud_equipped.SetItem(equipped);
	}

	public override void OnItemMoved(OnItemMovedEvent _event) {
		if (_event.to == 0) {
			// Moved to equipped slot
			MoveToEquipped(_event.item);
		} else if (_event.from == 0) {
			// Moved from equipped slot
			MoveToInventory(_event.item);
		}

		// Update the HUD
		hud_equipped.SetItem(equipped);
	}

	public override void OnItemAdded(OnItemAddedEvent _event) {
		// Move it
		if (_event.index == 0) {
			MoveToEquipped(_event.item);
		} else
			MoveToInventory(_event.item);
	}

	// Algorithm for deciding which slot the item goes into
	public override int AcceptItem(_Equipable item) {
		// Only accept _Equipable items
		if (item == null)
			return -1;

		// If there is an equipped item then no
		if (equipped != null)
			return -1;

		// TODO
		return 0;
	}
	#endregion
}
