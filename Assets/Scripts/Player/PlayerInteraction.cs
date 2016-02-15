using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class PlayerInteraction : PlayerSubClass {

	[Header("Electric settings")]

	public Transform electricTransform;
	public ParticleSystem electricParticles;

	[Header("Pickup settings")]

	public Transform pickupPoint;
	[Tooltip("Range in meters")]
	public float pickupRadius;
	[Tooltip("When calculating which item is closest should it ignore the y axis? (Which would count everything as on the same height)")]
	public bool ignoreYAxis = false;

	public Vector3 electricPoint {
		get { return (electricTransform ?? transform).position; }
	}

	private bool electrifying;

#if UNITY_EDITOR
	void OnDrawGizmos() {
		if (pickupPoint == null)
			return;

		bool shouldDraw = false;
		var selected = UnityEditor.Selection.gameObjects;
		foreach (var obj in selected) {
			if (obj.transform.IsChildOf(transform)) {
				shouldDraw = true;
				break;
			}
		}

		if (shouldDraw) {
			if (interaction.ignoreYAxis) {
				UnityEditor.Handles.color = Color.cyan;
				UnityEditor.Handles.DrawLine(pickupPoint.position + new Vector3(pickupRadius, 50), pickupPoint.position + new Vector3(pickupRadius, -50));
				UnityEditor.Handles.DrawLine(pickupPoint.position + new Vector3(-pickupRadius, 50), pickupPoint.position + new Vector3(-pickupRadius, -50));
				UnityEditor.Handles.DrawLine(pickupPoint.position + new Vector3(0, 50, pickupRadius), pickupPoint.position + new Vector3(0, -50, pickupRadius));
				UnityEditor.Handles.DrawLine(pickupPoint.position + new Vector3(0, 50, -pickupRadius), pickupPoint.position + new Vector3(0, -50, -pickupRadius));
				UnityEditor.Handles.DrawWireDisc(pickupPoint.position, Vector3.up, pickupRadius);
			} else {
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(pickupPoint.position, pickupRadius);
			}
		}
	}

	void OnValidate() {
		// Limit values
		pickupRadius = Mathf.Max(pickupRadius, 0f);
	}
#endif

	private _Equipable lastHover;
	void Update() {
		// Visualization
		_Equipable hover = GetItemInRange();

		if (hover != lastHover) {
			// Hover changed
			if (hover) hover.nearbyVisual.SetActive(true);
			if (lastHover) lastHover.nearbyVisual.SetActive(false);
		}
		
		lastHover = hover;

		// Actual grabbing
		if (Input.GetButtonDown("GrabNDrop")) {
			GrabNDrop();
		}
	}

	bool lastInteract;
	void FixedUpdate() { 

		electrifying = false;
		bool interact = Input.GetAxis("Interact") != 0;
		if (interact) {
			ElectrifyNInteract();
		}

		// Interaction changed
		if (interact != lastInteract && inventory.equipped == null) {
			if (electrifying)
				movement.anim.SetTrigger("ArmsHolding");
			else if (!interact) {
				movement.anim.SetTrigger("ArmsEmpty");
				movement.anim.ResetTrigger("ArmsHolding");
			}
		}

		lastInteract = interact;


		// Particles
		//var em = electricParticles.emission;
		//em.enabled = electrifying;
		
		electricParticles.SetActive(electrifying);
	}

	#region Picking up/Dropping items & Interacting

	void ElectrifyNInteract() {
		if (!_ElectricListener.InteractAt(controller, electricPoint)) {
			// Didn't interact with anything... Try to electrify
			if (inventory.equipped != null && inventory.equipped.canBeElectrified) {
				// Electrify the equipped item
				inventory.equipped.SendMessage(ElectricMethods.Electrify, controller, SendMessageOptions.DontRequireReceiver);
				electrifying = true;
			} else if (inventory.equipped == null) {
				// Try to electrify at your fingertips
				_ElectricListener.ElectrifyAllAt(controller, electricPoint);
				electrifying = true;
			}
		}

	}

	public bool IsItemInRange(_Equipable item) {
		return item.GetDistance(pickupPoint.position) <= pickupRadius;
	}

	public _Equipable GetItemInRange() {
		return Searchable.GetClosest<_Equipable>(pickupPoint.position, pickupRadius, controller.characterCenter, ignoreYAxis).obj;
	}

	void GrabNDrop() {
		if (inventory.equipped == null) {
			// No item equipped. Try to grab the nearby pushing point
			if (!pushing.GrabNDrop() && pushing.point == null) {
				// Nothing happened and no pushing point selected
				// Try to grab the nearby item
				_Equipable item = GetItemInRange();
				if (item != null)
					inventory.Equip(item);
			}
		} else {
			// Item equipped. Drop it.
			inventory.Unequip();
		}
	}

	#endregion
}
