using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

/*

	Equippable just means that it's equippable by the player

*/

[RequireComponent(typeof(Rigidbody))]
public abstract class _Equipable : _Item {

	[Header("_Equipable fields")]

    public Sprite icon;
	public int targetSlot = -1;
	[HideInInspector] public bool unlocked = false;
	public bool canBeElectrified = false;
	public GameObject nearbyVisual;

	protected PlayerInventory inventory;
	public bool equipped {
		get { return inventory != null && inventory.equipped == this; }
	}
	public bool fitsInInv {
		get { return targetSlot >= 0; }
	}

	protected virtual void Update() {
		if (nearbyVisual != null)
			nearbyVisual.SetActive(PlayerController.instance != null && PlayerController.instance.interaction.IsItemInRange(this));
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
		return base.CanLiveInSlot(inv, slot);
	}
}
