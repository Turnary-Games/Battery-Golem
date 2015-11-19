using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public abstract class _Inventory<Item> : MonoBehaviour where Item : _Item {
	
	/// <summary>
	/// The inventory slots. The size of the inventory is defined by the size of the array.
	/// </summary>
	public abstract Item[] slots { get; }

	// Events
	public abstract void OnItemMoved(OnItemMovedEvent _event);
	public abstract void OnItemAdded(OnItemAddedEvent _event);
	public abstract void OnItemRemoved(OnItemRemovedEvent _event);

	/// <summary>
	/// Should the inventory accept this item?
	/// Returns the slot the item should go into. A value of -1 means it does not accept the item.
	/// </summary>
	/// <param name="item">The item to check</param>
	public virtual int AcceptItem(Item item) {
		return item == null ? -1 : slots.EmptySlot();
	}
	

	public bool AddItem(Item item) {
		int index = AcceptItem(item);

		if (index >= 0) {
			Debug.Log("["+name+"] Add item at index " + index);
			if (slots[index] != null)
				RemoveItem(index);

			slots[index] = item;
			item.OnPickup();
			OnItemAdded(new OnItemAddedEvent(item, index));

			return true;
		} else
			return false;
	}

	public void RemoveItem(Item item) {
		int index = slots.IndexOf(item);
		if (index >= 0) {
			RemoveItem(index);
		}
	}

	public void RemoveItem(int index) {
		Item item = slots[index];

		if (item != null) {
			Debug.Log("[" + name + "] Remove item from " + index);
			slots[index] = null;
			item.OnDropped();
			OnItemRemoved(new OnItemRemovedEvent(item, index));
		}
	}

	public void DeleteItem(int index) {
		Item item = slots[index];

		if (item != null) {
			Debug.Log("[" + name + "] Deleted item from " + index);
			slots[index] = null;
			item.OnDropped();
			OnItemRemoved(new OnItemRemovedEvent(item, index));

			Destroy(item.gameObject);
		}
	}

	public bool SwapItems(int slotA, int slotB) {
		if (slotA == slotB || slotA < 0 || slotB < 0)
			// Invalid input
			return false;

		var itemA = slots[slotA];
		var itemB = slots[slotB];

		if (itemA != null && !itemA.CanLiveInSlot(this, slotB)) {
			// Can't swap
			return false;
		}
		if (itemB != null && !itemB.CanLiveInSlot(this, slotA)) {
			// Can't swap
			return false;
		} 

		// Swap
		slots[slotB] = itemA;
		slots[slotA] = itemB;

		// Send events
		if (itemA != null)
			OnItemMoved(new OnItemMovedEvent(itemA, slotA, slotB));
		if (itemB != null)
			OnItemMoved(new OnItemMovedEvent(itemB, slotB, slotA));

		return true;
	}

	public bool TransferItem<T>(int index, _Inventory<T> other) where T: Item {
		T item = slots[index] as T;
		if (item != null && other.AcceptItem(item) >= 0) {
			// Valid item and the other inventory accepts this item
			RemoveItem(index);
			return other.AddItem(item); // Should take it but just to be sure
		} else {
			return false;
		}
	}

	public override string ToString() {
		return slots.ToFancyString();
	}

	public struct OnItemRemovedEvent {
		public int index;
		public Item item;

		public OnItemRemovedEvent(Item item, int index) {
			this.item = item;
			this.index = index;
		}
	}

	public struct OnItemMovedEvent {
		public int from;
		public int to;
		public Item item;

		public OnItemMovedEvent(Item item, int from, int to) {
			this.item = item;
			this.from = from;
			this.to = to;
		}
	}

	public struct OnItemAddedEvent {
		public int index;
		public Item item;

		public OnItemAddedEvent(Item item, int index) {
			this.item = item;
			this.index = index;
		}
	}

}
