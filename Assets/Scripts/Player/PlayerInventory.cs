using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public PlayerController player;
	public Transform equippedParent;

	[Header("Inventory variables")]

	public _Equipable equipped;

	[Header("HUD's / GUI's")]

	public HUD_Equipped hud_equipped;


	void Start() {
		hud_equipped.SetItem(equipped);
	}

	#region Pickup item (from ground)
	public void Pickup(GameObject obj) {
		var item = obj.GetComponent<_Equipable>();

		if (item != null && equipped != item) {
			// Valid object, equip it.

			// First unequip previous item
			Drop ();

			// Equip the item
			item.OnPickupBegin(this);
			Equip (item);
			item.OnPickup(this);
		} else {
			if (equipped == item) {
				//print("TRYING TO PICKUP ALREADY PICKED UP ITEM: " + obj.name);
				// JUST IGNORE
			} else
				print("INVALID PICKUP ITEM: " + obj.name);
		}
	}

	public void Drop() {
		Unequip(false);
	}
	#endregion


	#region Equipping
	public void Equip(_Equipable item) {
		// Equip it
		equipped = item;
		// Move it
		item.transform.parent = equippedParent;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
		// Send the event
		item.OnEquip (this);
		hud_equipped.SetItem(equipped);
	}

	public void Unequip(bool sendToInv) {
		if (equipped != null) {

			if (sendToInv) {
				// Send the item to the players inventory
				SendToInventory(equipped);
			} else {
				// Drop it on the ground
				equipped.OnDroppedBegin(this);
				equipped.transform.parent = null;
				equipped.OnDropped(this);
			}

			equipped.OnUnequip(this);
			equipped = null;

			hud_equipped.SetItem(null);
		}
	}

	public void Unequip() {
		// Send to inventory if possible
		if (equipped != null)
			Unequip(equipped.stashable);
	}
	#endregion

	#region Inventory manager
	public void SendToInventory(_Equipable item) {
		// Sets parent
		item.transform.parent = transform;
	}

	public void TakeFromInventory(_Equipable item) {
		Equip(item);
	}
	#endregion
}
