using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class NPCDialogOverrideCoreItem : NPCDialogOverrideBase {

	[Header("Will check at start of scene")]
	public string inventoryContains = "N/A";
	
	void Start() {
		var coreItems = PlayerController.instance.inventory.coreItems;
		foreach(var item in coreItems) {
			if (item && item.itemName == inventoryContains) {
				Override();
				break;
			}
		}
	}

}
