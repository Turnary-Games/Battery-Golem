using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

/*

	CoreItems can be stored in the players inventory.

*/

public class _CoreItem : _Item {

	public int targetSlot = -1;
	public GameObject preview;
	[HideInInspector]
	public PlayerInventory inventory;
	public bool equipped { get { return inventory != null && inventory.equipped == this; } }

	public virtual void OnEquip(PlayerInventory inventory) {
		// Item got equipped
		this.inventory = inventory;
		print("Item \"" + itemName + "\" equipped");
	}

	public virtual void OnUnequip(PlayerInventory inventory) {
		// Item got unequipped
		this.inventory = null;
		print("Item \"" + itemName + "\" unequipped");
	}


}
