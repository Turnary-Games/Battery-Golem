using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.SceneManagement;

public interface PlayerSubClass {
	PlayerController controller { get; }
	PlayerInventory inventory { get; }
	PlayerMovement movement { get; }
	PlayerHealth health { get; }
	PlayerInteraction interaction { get; }
}

public class PlayerController : SingletonBase<PlayerController> {

	[Header("Player sub-classes")]

	public PlayerInventory inventory;
	public PlayerMovement movement;
	public PlayerHealth health;
	public PlayerInteraction interaction;

	public Vector3 characterCenter {
		get { return transform.position + (movement.character != null ? movement.character.center : Vector3.zero); }
	}

	void Update() {
		for (int slot = 1; slot <= 3; slot++) {
			if (Input.GetButtonDown("Slot " + slot)) {
				inventory.SwapItems(0, slot);
			}
		}
	}

#if UNITY_EDITOR
	void OnValidate() {
		// Try to find the sub-classes automatically
		inventory = inventory ?? GetComponent<PlayerInventory>();
		movement = movement ?? GetComponent<PlayerMovement>();
		health = health ?? GetComponent<PlayerHealth>();
		interaction = interaction ?? GetComponent<PlayerInteraction>();
	}
#endif

	#region Collisions
	void OnControllerColliderHit(ControllerColliderHit hit) {

		// Collision listener
		GameObject main = hit.collider.attachedRigidbody != null ? hit.collider.attachedRigidbody.gameObject : hit.gameObject;
		if (main != null) {
			main.SendMessage (TouchMethods.Touch, this, SendMessageOptions.DontRequireReceiver);
		}
	}
	#endregion


	// Get all listeners of the touching 
	public List<_TouchListener> GetListeners() {
		return _TouchListener.FindListeners(this);
	}
}
