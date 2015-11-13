using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DropoffStation : Inventory<DropoffItem> {

    public Transform targetTransform;
	public bool valid { get { return item != null; } }

	private DropoffItem item {
		get { return slots.Length > 0 ? slots[0] : null; }
		set { slots[0] = value; }
	}

	// The size of the inventoryList defines the size of the inventory
	private DropoffItem[] inventoryList = new DropoffItem[1];
	public override DropoffItem[] slots { get { return inventoryList; } }

	void OnInteractStart(PlayerController player) {
		player.inventory.Dropoff(station:this);
	}

	public override void OnItemMoved(OnItemMovedEvent _event) {
		throw new NotImplementedException();
	}

	public override void OnItemAdded(OnItemAddedEvent _event) {
		_event.item.transform.parent = targetTransform != null ? targetTransform : transform;
		_event.item.transform.localPosition = Vector3.zero;
		_event.item.transform.localEulerAngles = Vector3.zero;
	}

	public override void OnItemRemoved(OnItemRemovedEvent _event) {
		throw new NotImplementedException();
	}
}
