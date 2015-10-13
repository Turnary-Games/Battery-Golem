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

	public virtual void OnUnequip() {
		equipped = false;
		this.inventory = null;
		print (itemName + " unequipped");
	}

}
