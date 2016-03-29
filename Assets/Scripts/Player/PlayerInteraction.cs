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

	[HideInInspector]
	public NPCController talkingTo;

	[SerializeThis]
	private _Equipable lastHover;
	private bool elecDown;
	private bool inteDown;

	[System.NonSerialized]
	public bool isElectrifying;

	public Vector3 electricPoint {
		get { return (electricTransform ?? transform).position; }
	}

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
				UnityEditor.Handles.DrawWireDisc(pickupPoint.position + Vector3.up * 50, Vector3.up, pickupRadius);
				UnityEditor.Handles.DrawWireDisc(pickupPoint.position + Vector3.down * 50, Vector3.up, pickupRadius);
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

	void Update() {
		// Visualization
		_Equipable hover = GetItemInRange();

		if (hover != lastHover) {
			// Hover changed
			if (hover && hover.nearbyVisual) hover.nearbyVisual.enabled = true;
			if (lastHover && lastHover.nearbyVisual) lastHover.nearbyVisual.enabled = false;
		}
		lastHover = hover;

		// Read input
		if (!inteDown && Input.GetButtonDown("Interact")) inteDown = true;
	}
	
	void FixedUpdate() { 

		if (inteDown) {
			inteDown = false;
			// Interacting priority order:
			// - NPC dialog
			// - release
			// - pickup (if no item)
			// - interact
			// - drop
			// - grab

			if (talkingTo) {
				// Continue talking
				talkingTo.GetComponent<_ElectricListener>().Interact(controller);
			} else if (pushing && pushing.hasPoint) {
				// Release grabbed object
				pushing.point = null;
			} else {
				// Try to grab the nearest item
				_Equipable item = GetItemInRange();
				if (!inventory.equipped && item != null)
					inventory.Equip(item);
				else {
					// Try to interact
					if (!_ElectricListener.InteractAt(controller, electricPoint)) {
						// Didn't interact with anything
						if (inventory.equipped)
							// Drop equipped item
							inventory.Unequip();
						else
							// Grab an object
							pushing.TryToGrab();
					}
				}
			}
		}

		isElectrifying = Input.GetAxis("Electrify") != 0;
		foreach(ParticleSystem ps in electricParticles.GetComponentsInChildren<ParticleSystem>()) {
			var em = ps.emission;
			em.enabled = isElectrifying;
		}
		if (isElectrifying) {
			// Electrifying priority order:
			// - electrify held item
			// - electrify grabbed object
			// - electrify point in world
			
			if (inventory.equipped) {
				// Electrify the held item
				inventory.equipped.SendMessage(ElectricMethods.Electrify, controller, SendMessageOptions.DontRequireReceiver);
			} else if (pushing.hasPoint) {
				// Try to electrify grabbed object
				throw new System.NotImplementedException();
			} else {
				// Try to electrify at your fingertips
				_ElectricListener.ElectrifyAllAt(controller, electricPoint);
			}
		}
	}

	#region Picking up/Dropping items & Interacting

	public bool IsItemInRange(_Equipable item) {
		return item.GetDistance(pickupPoint.position) <= pickupRadius;
	}

	public _Equipable GetItemInRange() {
		return Searchable.GetClosest<_Equipable>(pickupPoint.position, pickupRadius, controller.characterCenter, ignoreYAxis).obj;
	}

	#endregion
}
