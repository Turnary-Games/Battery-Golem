using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(_TouchListener))]
public class _Equipable : Searchable {

    [Header("Equipable variables")]

    public string itemName = "Unnamned";
    public Sprite icon;
    [Space]
    public bool stashable = true;
    public Rigidbody rbody;
    public _EffectItem.ActionOnEvent rbodyOnStart;
    public _EffectItem[] effects;

	protected PlayerInventory inventory;
	protected bool equipped = false;

	protected virtual void Start() {
        // do stuff to rigidbody
        switch (rbodyOnStart) {
            case _EffectItem.ActionOnEvent.doNothing:
                // Do nothing
                break;

            case _EffectItem.ActionOnEvent.enable:
                rbody.SetEnabled(true);
                break;

            case _EffectItem.ActionOnEvent.disable:
                rbody.SetEnabled(false);
                break;

            case _EffectItem.ActionOnEvent.toggle:
                rbody.SetEnabled(!rbody.IsEnabled());
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
        rbody.SetEnabled(false);
    }

	public virtual void OnPickup(PlayerInventory inventory) {
        // Item got picked up from ground

        // Disable all effects
        effects.OnPickup();
    }

	public virtual void OnDroppedBegin(PlayerInventory inventory) {
        // Item is about to be dropped

        // Reactivate rigidbody
        rbody.SetEnabled(true);
    }
    public virtual void OnDropped(PlayerInventory inventory) {
        // Item dropped on ground

        // Enable all effects
        effects.OnDrop();
    }

	

}
