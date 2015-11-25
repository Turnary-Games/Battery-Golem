using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class _DropoffStation<Item> : _Inventory<Item> where Item : _DropoffItem {

    public Transform targetTransform;
	public bool valid { get { return item != null; } }

	private Item item {
		get { return slots.Length > 0 ? slots[0] : null; }
		set { slots[0] = value; }
	}

	// The size of the inventoryList defines the size of the inventory
	private Item[] inventoryList = new Item[1];
	public override Item[] slots { get { return inventoryList; } }

	public override void OnItemMoved(OnItemMovedEvent _event) {
		// Only 1 item slot, should never happen
		throw new NotImplementedException();
	}

	public override void OnItemAdded(OnItemAddedEvent _event) {
		_event.item.transform.parent = targetTransform != null ? targetTransform : transform;
		_event.item.transform.localPosition = Vector3.zero;
		_event.item.transform.localEulerAngles = Vector3.zero;
		_event.item.OnItemDroppedOff(this);
	}

	public override void OnItemRemoved(OnItemRemovedEvent _event) {
		// Dropoff only, should never happen
		throw new NotImplementedException();
	}
}
