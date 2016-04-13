using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _DropoffStation : MonoBehaviour {

    public Transform targetTransform;
	public bool valid { get { return item != null; } }

	private _Item item {
		get { return slots.Length > 0 ? slots[0] : null; }
		set { slots[0] = value; }
	}

	// The size of the inventoryList defines the size of the inventory
	[SerializeThis]
	private _Item[] inventoryList = new _Item[1];
	public _Item[] slots { get { return inventoryList; } }

	//public void OnItemMoved(OnItemMovedEvent _event) {
	//	// Only 1 item slot, should never happen
	//	throw new NotImplementedException();
	//}

	public void AddItem(_Item item) {
		item.transform.parent = targetTransform != null ? targetTransform : transform;
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
		item.SendMessage(ItemMethods.OnItemDroppedOff, this, SendMessageOptions.DontRequireReceiver);
	}

	//public void OnItemRemoved(OnItemRemovedEvent _event) {
	//	// Dropoff only, should never happen
	//	throw new NotImplementedException();
	//}

	//public struct OnItemRemovedEvent {
	//	public int index;
	//	public Item item;

	//	public OnItemRemovedEvent(Item item, int index) {
	//		this.item = item;
	//		this.index = index;
	//	}
	//}

	//public struct OnItemMovedEvent {
	//	public int from;
	//	public int to;
	//	public Item item;

	//	public OnItemMovedEvent(Item item, int from, int to) {
	//		this.item = item;
	//		this.from = from;
	//		this.to = to;
	//	}
	//}

	//public struct OnItemAddedEvent {
	//	public int index;
	//	public Item item;

	//	public OnItemAddedEvent(Item item, int index) {
	//		this.item = item;
	//		this.index = index;
	//	}
	//}
}
