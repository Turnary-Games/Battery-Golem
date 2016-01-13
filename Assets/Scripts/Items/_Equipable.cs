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

	[Header("_Equipable fields")]

    public Sprite icon;
	public int slot = -1;
	public bool canBeElectrified = false;
	public GameObject nearbyVisual;

	protected PlayerInventory inventory;
	public bool equipped {
		get { return inventory != null; }
	}

	protected virtual void Update() {
		if (nearbyVisual != null)
			nearbyVisual.SetActive(PlayerController.instance.interaction.IsItemInRange(this));
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

	public override bool CanLiveInSlot<Item>(_Inventory<Item> inv, int slot) {
		if (inv is PlayerInventory) {
			return slot == 0 || slot == this.slot;
		}
		return base.CanLiveInSlot(inv, slot);
	}
}
