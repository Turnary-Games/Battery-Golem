using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System;

public class PlayerInventory : _Inventory<_Equipable>, PlayerSubClass {

	/*

		The players inventory uses slot 0 for the equipped item
		and the rest for custom item slots.

		The player can only hold _Equipable items in its inventory.

	*/

	[Header("Player parent class")]

	public PlayerController parent;
	public PlayerController controller { get { return parent; } }
	public PlayerInventory inventory { get { return this; } }
	public PlayerMovement movement { get { return parent.movement; } }
	public PlayerHealth health { get { return parent.health; } }
	public PlayerInteraction interaction { get { return parent.interaction; } }

	[Header("Object references")]
	
	public Transform equippedParent;
	public PlayerHUD hud;

	//[Header("Inventory settings")]

	public _Equipable equipped {
		get { return slots.Get(0); }
		set { slots[0] = value; }
	}

	// The size of the inventoryList defines the size of the inventory
	private _Equipable[] inventoryList = new _Equipable[4];
	public override _Equipable[] slots { get { return inventoryList; } }

	void Start() {
		hud.UpdateUIElements();
	}

	#region Pickup item (from ground)
	// Unequip the equipped item
	public void Unequip() {
		if (equipped == null)
			return;
		
		if (equipped.slot < 0) {
			RemoveItem(0);
			return;
		}

		if (slots.Get(equipped.slot) != null) {
			// Not enough room! This should never happen; only assign 1 item to 1 slot!
			// If for some reason we have 2 of the same item then just remove 1 of them
			DeleteItem(equipped.slot);
		}

		if (!SwapItems(equipped.slot, 0)) {
			// Can't be stored in inventory
			RemoveItem(0);
		}
	}
    #endregion

    #region Dropoff at station
    // Dropoff at dropoff-station
    public bool Dropoff<Item>(_DropoffStation<Item> station) where Item : _DropoffItem {
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

		hud.UpdateUIElements();
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
		hud.UpdateUIElements();
	}

	public override void OnItemAdded(OnItemAddedEvent _event) {
		// Move it
		if (_event.index == 0) {
			MoveToEquipped(_event.item);
			hud.UnlockSlot(_event.item.slot);
		} else
			MoveToInventory(_event.item);

		hud.UpdateUIElements();
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
