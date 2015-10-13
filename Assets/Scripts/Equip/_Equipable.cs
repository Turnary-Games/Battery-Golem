using UnityEngine;
using System.Collections;

public class _Equipable : MonoBehaviour {

	public string itemName = "Unamned";
	public bool stashable = true;

	protected PlayerInventory inventory;
	protected bool equipped = false;

	public virtual void OnEquip(PlayerInventory inventory) {
		equipped = true;
		this.inventory = inventory;
		print (itemName + " equipped");
	}

	public virtual void OnUnequip(PlayerInventory inventory) {
		equipped = false;
		this.inventory = null;
		print (itemName + " unequipped");
	}

	public virtual void OnPickup(PlayerInventory inventory) {
		// Item got picked up from ground
	}

	public virtual void OnDropped(PlayerInventory inventory) {
		// Item dropped on ground
	}

}
