using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public Transform equippedParent;

	[Header("Inventory variables")]

	public _Equipable equipped;


	#region Pickup item (from ground)
	public void Pickup(GameObject obj) {
		var item = obj.GetComponent<_Equipable>();

		if (item != null) {
			// Valid object, equip it.

			// First unequip previous item
			Drop();

			// Equip the item
			Equip(item);
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
		// Send the event
		item.SendMessage(EquipMethods.OnEquip);
	}

	public void Unequip(bool sendToInv) {
		if (equipped != null) {

			if (sendToInv) {
				// Send the item to the players inventory
				SendToInventory(equipped);
			} else { 
				// Drop it on the ground
				equipped.transform.parent = null;
			}

			equipped.SendMessage(EquipMethods.OnUnequip);
			equipped = null;
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
