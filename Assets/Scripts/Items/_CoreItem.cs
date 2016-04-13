using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

/*

	Equippable just means that it's equippable by the player

*/

public class _CoreItem : _Item {

	public int slot;
	public bool unlocked;

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
