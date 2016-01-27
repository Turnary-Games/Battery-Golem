using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.SceneManagement;

public class PlayerSubClass : MonoBehaviour {
	public PlayerController parent;
	public PlayerController controller { get { return parent; } }
	public PlayerInventory inventory { get { return parent.inventory; } }
	public PlayerMovement movement { get { return parent.movement; } }
	public PlayerPushing pushing { get { return parent.pushing; } }
	public PlayerHealth health { get { return parent.health; } }
	public PlayerInteraction interaction { get { return parent.interaction; } }
	public PlayerHUD hud { get { return parent.hud; } }
}

public class PlayerController : SingletonBase<PlayerController> {

	[Header("Player sub-classes")]

	public PlayerInventory inventory;
	public PlayerMovement movement;
	public PlayerHealth health;
	public PlayerPushing pushing;
	public PlayerInteraction interaction;
	public PlayerHUD hud;

	public Vector3 characterCenter {
		get { return transform.position + (movement.character != null ? movement.character.center : Vector3.zero); }
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