using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _Equipable : MonoBehaviour {

	[Header("Equipable variables")]

	public string itemName = "Unnamned";
	public bool stashable = true;
	public Sprite icon;

	public static List<_Equipable> _ALL = new List<_Equipable>();

	protected PlayerInventory inventory;
	protected bool equipped = false;

	protected virtual void Start() {
		_ALL.Add(this);
	}

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

	public virtual void OnPickupBegin(PlayerInventory inventory) {
		// Item is about to be picked up
	}
	public virtual void OnPickup(PlayerInventory inventory) {
		// Item got picked up from ground
	}

	public virtual void OnDroppedBegin(PlayerInventory inventory) {
		// Item is about to be dropped
	}
    public virtual void OnDropped(PlayerInventory inventory) {
		// Item dropped on ground
	}

	public float GetDistance(Vector3 from, bool ignoreY = false) {
		Vector3 to = transform.position;

		if (ignoreY) {
			to.y = from.y = 0;
		}

		return Vector3.Distance(from, transform.position);
	}

	public static Closest GetClosest(Vector3 from, bool ignoreY = false) {
		return new Closest(from, ignoreY);
	}

	public struct Closest {
		public _Equipable item;
		public float dist;
		public bool valid;

		public Closest(_Equipable item, float dist) {
			this.item = item;
			this.dist = dist;
			this.valid = item != null;
		}

		public Closest(Vector3 from, bool ignoreY = false) {
			_Equipable closestObj = null;
			float closestDist = Mathf.Infinity;

			// Look for the closest one
			_ALL.ForEach(delegate (_Equipable obj) {
				if (obj.inventory != null)
					return;

				float dist = obj.GetDistance(from, ignoreY);

				if (closestObj == null || (closestDist < dist)) {
					closestObj = obj;
					closestDist = dist;
				}
			});

			this = new Closest(closestObj, closestDist);
		}
	}

}
