using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(_TouchListener))]
public class _Equipable : MonoBehaviour {

    [Header("Equipable variables")]

    public string itemName = "Unnamned";
    public Sprite icon;
    [Space]
    public bool stashable = true;
    public Rigidbody rbody;
    public _EffectItem.ActionOnEvent rbodyOnStart;
    public _EffectItem[] effects;

    public static List<_Equipable> _ALL = new List<_Equipable>();

	protected PlayerInventory inventory;
	protected bool equipped = false;


	protected virtual void Start() {
		_ALL.Add(this);

        // do stuff to rigidbody
        switch (rbodyOnStart) {
            case _EffectItem.ActionOnEvent.doNothing:
                // Do nothing
                break;

            case _EffectItem.ActionOnEvent.enable:
                EnableRigidbody();
                break;

            case _EffectItem.ActionOnEvent.disable:
                DisableRigidbody();
                break;

            case _EffectItem.ActionOnEvent.toggle:
                if (IsRigidbodyEnabled())
                    DisableRigidbody();
                else
                    EnableRigidbody();
                break;
        }
	}

    protected virtual void OnCollisionStay(Collision collision) {
        GameObject main = collision.collider.attachedRigidbody != null ? collision.collider.attachedRigidbody.gameObject : collision.gameObject;
        main.SendMessage(TouchMethods.Touch, this, SendMessageOptions.DontRequireReceiver);
    }

    public virtual void OnEquip(PlayerInventory inventory) {
        // Item got equipped
		equipped = true;
		this.inventory = inventory;
		print ("Item \"" + itemName + "\" equipped");
	}

	public virtual void OnUnequip(PlayerInventory inventory) {
        // Item got unequipped
		equipped = false;
		this.inventory = null;
		print ("Item \"" + itemName + "\" unequipped");
	}

	public virtual void OnPickupBegin(PlayerInventory inventory) {
        // Item is about to be picked up

        // Disable rigidbody
        DisableRigidbody();
    }

	public virtual void OnPickup(PlayerInventory inventory) {
        // Item got picked up from ground

        // Disable all effects
        effects.OnPickup();
    }

	public virtual void OnDroppedBegin(PlayerInventory inventory) {
        // Item is about to be dropped

        // Reactivate rigidbody
        EnableRigidbody();
    }
    public virtual void OnDropped(PlayerInventory inventory) {
        // Item dropped on ground

        // Enable all effects
        effects.OnDrop();
    }

    public void DisableRigidbody() {
        rbody.detectCollisions = false;
        rbody.useGravity = false;
        rbody.velocity = Vector3.zero;
    }

    public void EnableRigidbody() {
        rbody.detectCollisions = true;
        rbody.useGravity = true;
    }

    public bool IsRigidbodyEnabled() {
        return rbody.detectCollisions && rbody.useGravity;
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
                // Ignore all that is in an inventory
				if (obj.inventory != null)
					return;

				float dist = obj.GetDistance(from, ignoreY);

				if (closestObj == null || (dist < closestDist)) {
					closestObj = obj;
					closestDist = dist;
				}
			});

			this = new Closest(closestObj, closestDist);
		}
	}

}
