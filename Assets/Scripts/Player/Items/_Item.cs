using UnityEngine;
using System.Collections;
using ExtensionMethods;

public static class ItemMethods {
	// Basic item messages
	/// <summary>OnPickup()</summary>
	public const string OnPickup = "OnPickup";
	/// <summary>OnDropped()</summary>
	public const string OnDropped = "OnDropped";
	
	// Dropoff specific
	/// <summary>OnItemDroppedOff(_DropoffStation)</summary>
	public const string OnItemDroppedOff = "OnItemDroppedOff";

	// Coreitem specific
	/// <summary>OnEquip(PlayerInventory)</summary>
	public const string OnEquip = "OnEquip";
	/// <summary>OnUnequip(PlayerInventory)</summary>
	public const string OnUnequip = "OnUnequip";
}

[RequireComponent(typeof(Rigidbody))]
public class _Item : Searchable {
	
	[Header("Reference to self")]

	[PrefabOptions]
	public string prefab;

	[Header("_Item fields")]

	public int id = -1;
	public string itemName = "Unnamned";
    //public bool startDisabled = true;

	[Header("_Equipable fields")]
	
	public int targetSlot = -1;
	[HideInInspector]
	public bool canBeElectrified = false;
	public Renderer nearbyVisual;
	
	private Rigidbody _body;
	public Rigidbody body {
		get { if (_body == null) _body = GetComponent<Rigidbody>(); return _body; }
	}
	[System.NonSerialized]
	public Vector3 startPos;
	[System.NonSerialized]
	public Quaternion startRot;
	[System.NonSerialized]
	public bool can_reset = false;

	protected virtual void Start() {
		// disable rigidbody
		//if (startDisabled)
		//   body.SetEnabled(false);

		if (startPos == Vector3.zero && !can_reset) {
			startPos = transform.position;
			startRot = transform.rotation;
			can_reset = true;
		}
	}

	protected virtual void OnTriggerEnter(Collider other) {
		if (!can_reset) return;

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

		body.SetEnabled(false);
	}

	public virtual void OnDropped() {
		// Item dropped on ground
		
		body.SetEnabled(true);
	}

	public virtual void Reset() {
		//print("Reset " + name);
		transform.position = startPos;
		transform.rotation = startRot;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
	}
}
