using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

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

	void OnDrawGizmos() {
		bool shouldDraw = false;
		var selected = UnityEditor.Selection.gameObjects;
		foreach (var obj in selected) {
			if (obj.GetComponent<PlayerController>() != null
				|| obj.GetComponent<PlayerInventory>() != null)
				shouldDraw = true;
		}

		if (shouldDraw) {
			if (inventory.ignoreYAxis) {
				UnityEditor.Handles.color = Color.cyan;
				UnityEditor.Handles.DrawLine(characterCenter + new Vector3(inventory.pickupRadius, 50), characterCenter + new Vector3(inventory.pickupRadius, -50));
				UnityEditor.Handles.DrawLine(characterCenter + new Vector3(-inventory.pickupRadius, 50), characterCenter + new Vector3(-inventory.pickupRadius, -50));
				UnityEditor.Handles.DrawLine(characterCenter + new Vector3(0, 50, inventory.pickupRadius), characterCenter + new Vector3(0, -50, inventory.pickupRadius));
				UnityEditor.Handles.DrawLine(characterCenter + new Vector3(0, 50, -inventory.pickupRadius), characterCenter + new Vector3(0, -50, -inventory.pickupRadius));
				UnityEditor.Handles.DrawWireDisc(characterCenter, Vector3.up, inventory.pickupRadius);
			} else {
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(characterCenter, inventory.pickupRadius);
			}
		}
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
