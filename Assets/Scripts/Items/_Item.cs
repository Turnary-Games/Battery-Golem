using UnityEngine;
using System.Collections;
using ExtensionMethods;

public abstract class _Item : Searchable {

	[Header("_Item fields")]

	public string itemName = "Unnamned";
	public Rigidbody rbody;
	public PickupAction[] effects;

	private Vector3 startPos;
	private Quaternion startRot;

	protected virtual void Start() {
		// disable rigidbody
		rbody.SetEnabled(false);

		startPos = transform.position;
		startRot = transform.rotation;
	}

	protected virtual void OnTriggerEnter(Collider other) {
		if (other.tag == "Water") {
			// In case you drop it into the water
			Reset();
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

	public virtual void Reset() {
		transform.position = startPos;
		transform.rotation = startRot;
		rbody.SetEnabled(false);
	}

	public virtual bool CanLiveInSlot<Item>(_Inventory<Item> inv, int slot) where Item : _Item {
		return true;
	}
}
