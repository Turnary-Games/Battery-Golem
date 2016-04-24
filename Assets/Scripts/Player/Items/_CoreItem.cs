using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

/*

	Equippable just means that it's equippable by the player

*/

public class _CoreItem : _Item {

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
