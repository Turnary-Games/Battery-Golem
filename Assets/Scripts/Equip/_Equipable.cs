using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

/*

	Equippable just means that it's equippable by the player

*/

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(_TouchListener))]
public abstract class _Equipable : _Item {

    public Sprite icon;

	protected PlayerInventory inventory;
	public bool equipped {
		get { return inventory != null; }
	}

    public virtual void OnEquip(PlayerInventory inventory) {
        // Item got equipped
		this.inventory = inventory;
		print ("Item \"" + itemName + "\" equipped");
	}

	public virtual void OnUnequip(PlayerInventory inventory) {
        // Item got unequipped
		this.inventory = null;
		print ("Item \"" + itemName + "\" unequipped");
	}

}
