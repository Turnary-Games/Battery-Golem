using UnityEngine;
using System.Collections;

public sealed class EquipMethods {
	public static string OnEquip = "OnEquip";
	public static string OnUnequip = "OnUnequip";
}

public class _Equipable : MonoBehaviour {

	public string itemName = "Unamned";
	public bool stashable = true;

}
