using UnityEngine;
using System.Collections;

public class _Equipable : MonoBehaviour {

	public string itemName = "Unamned";
	public bool stashable = true;

	protected bool equipped = false;

	public virtual void OnEquip() {
		equipped = true;
		print (itemName + " equipped");
	}

	public virtual void OnUnequip() {
		equipped = false;
		print (itemName + " unequipped");
	}

}
