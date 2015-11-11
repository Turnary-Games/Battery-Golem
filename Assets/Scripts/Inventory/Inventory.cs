using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public abstract class Inventory : MonoBehaviour {

	/// <summary>
	/// The inventory slots. The size of the inventory is defined by the size variable.
	/// </summary>
	public List<_Item> slots = new List<_Item>();
	/// <summary>
	/// The size of the inventory.
	/// </summary>
	public abstract int capacity { get; }

	// Events
	public abstract void OnItemMoved(_Item item);
	public abstract void OnItemAdded(_Item item);
	public abstract void OnItemRemoved(_Item item);

	/// <summary>
	/// Should the inventory accept this item?
	/// Returns the slot the item should go into. A value of -1 means it does not accept the item.
	/// </summary>
	/// <param name="item">The item to check</param>
	public virtual int AcceptItem(_Item item) {
		return slots.EmptySlot(capacity);
	}
	

	public bool AddItem(_Item item) {
		int index = AcceptItem(item);

		if (index >= 0) {
			if (slots[index] != null)
				RemoveItem(index);

			slots[index] = item;
			item.OnPickup();
			OnItemAdded(item);

			return true;
		} else
			return false;
	}

	public void RemoveItem(_Item item) {
		int index = slots.IndexOf(item);
		if (index >= 0) {
			RemoveItem(index);
		}
	}

	public void RemoveItem(int index) {
		_Item item = slots[index];

		if (item != null) {
			slots.RemoveAt(index);
			item.OnDropped();
			OnItemRemoved(item);
		}
	}

	public void SwapItems(int slotA, int slotB) {
		var itemA = slots[slotA];
		var itemB = slots[slotB];

		// Swap
		slots[slotA] = itemB;
		slots[slotB] = itemA;

		// Send events
		if (itemA != null)
			OnItemMoved(itemA);
		if (itemB != null)
			OnItemMoved(itemB);
	}

}
