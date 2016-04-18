using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine.SceneManagement;

public class PlayerSubClass : MonoBehaviour {
	public PlayerController parent;
	public PlayerController controller { get { return parent; } }
	public PlayerInventory inventory { get { return parent != null ? parent.inventory : null; } }
	public PlayerMovement movement { get { return parent != null ? parent.movement : null; } }
	public PlayerPushing pushing { get { return parent != null ? parent.pushing : null; } }
	public PlayerHealth health { get { return parent != null ? parent.health : null; } }
	public PlayerInteraction interaction { get { return parent != null ? parent.interaction : null; } }
	public PlayerHUD hud { get { return parent != null ? parent.hud : null; } }
	public PlayerAnimation anim { get { return parent != null ? parent.anim : null; } }
}

public class PlayerController : SingletonBase<PlayerController> {

	[Header("Player sub-classes")]

	public PlayerInventory inventory;
	public PlayerMovement movement;
	public PlayerHealth health;
	public PlayerPushing pushing;
	public PlayerInteraction interaction;
	public PlayerHUD hud;
	public PlayerAnimation anim;

	public Vector3 characterCenter {
		get { return transform.position + (movement && movement.capsule ? movement.transform.TransformVector(movement.capsule.center) : Vector3.zero); }
	}

	public Vector3 characterTop {
		get { return transform.position + (movement && movement.capsule ? movement.transform.TransformVector(movement.capsule.center + Vector3.up * movement.capsule.height / 2) : Vector3.zero); }
	}
	
	[HideInInspector]
	public int exitID;

#if UNITY_EDITOR
	void OnValidate() {
		// Try to find the sub-classes automatically
		inventory = inventory ?? GetComponent<PlayerInventory>();
		movement = movement ?? GetComponent<PlayerMovement>();
		health = health ?? GetComponent<PlayerHealth>();
		interaction = interaction ?? GetComponent<PlayerInteraction>();
	}
#endif

	//void Update() {
	//	if (Input.GetKeyDown(KeyCode.Home) && LevelSerializer.CanResume) {
	//		print("LOAD CHECKPOINT");
	//		LevelSerializer.Resume();
	//	}
	//	if (Input.GetKeyDown(KeyCode.End)) {
	//		print("SAVE CHECKPOINT");
	//		LevelSerializer.Checkpoint();
	//	}
	//}

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
	
	
	protected override void Awake() {
		base.Awake();
		DontDestroyOnLoad(transform.root.gameObject);
	}

	void OnRoomWasLoaded() {
		SpawnPoint exit = null;

		if (exitID >= 0) {
			exit = SpawnPoint.GetFromID(exitID);
			//exitID = -1;
			if (exit)
				goto UseSpawnPoint;
			else
				goto UseDefault;

		} else goto UseDefault;

	UseSpawnPoint:
		print("TAKE " + exit.name + " (id=" + exit.ID + "): " + exit.transform.position);
		transform.position = exit.transform.position;
		transform.rotation = exit.transform.rotation;
		movement.body.velocity = Vector3.zero;
		movement.body.angularVelocity = Vector3.zero;
		return;	

	UseDefault:
		var def = FindObjectOfType<DefaultSpawnPoint>();
		if (def) {
			print("TAKE DEFAULT: " + def.transform.position);
			transform.position = def.transform.position;
			transform.rotation = def.transform.rotation;
		}
	}
}