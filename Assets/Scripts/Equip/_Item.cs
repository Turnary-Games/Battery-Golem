using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class _Item : Searchable {

	public string itemName = "Unnamned";
	public Rigidbody rbody;
	public _EffectItem.ActionOnEvent rbodyOnStart;
	public _EffectItem[] effects;
	

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

	public virtual void OnPickup() {
		// Item got picked up from ground

		// Disable all effects
		effects.OnPickup();
		rbody.SetEnabled(false);
	}

	public virtual void OnDropped() {
		// Item dropped on ground

		// Enable all effects
		effects.OnDrop();
		rbody.SetEnabled(true);
	}

	public virtual bool CanLiveInSlot<Item>(Inventory<Item> inv, int slot) where Item : _Item {
		return true;
	}
}
