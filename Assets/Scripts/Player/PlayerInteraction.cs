using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

	[Header("Player sub-classes")]
	public PlayerController controller;
	public PlayerInventory inventory { get { return controller.inventory; } }
	public PlayerMovement movement { get { return controller.movement; } }
	public PlayerHealth health { get { return controller.health; } }
	public PlayerInteraction interaction { get { return this; } }

	[Header("Electric settings")]

	public Transform electricTransform;
	public ParticleSystem electricParticles;

	public Vector3 electricPoint {
		get { return (electricTransform ?? transform).position; }
	}

	private bool electrifying;

	void Update() {
		if (Input.GetButtonDown("GrabNDrop")) {
			GrabNDrop();
		}

		electrifying = false;
		if (Input.GetAxis("Interact") != 0) {
			ElectrifyNInteract();
		}
		if (electrifying && !electricParticles.isPlaying)
			electricParticles.Play();
		if (!electrifying && !electricParticles.isStopped)
			electricParticles.Stop();
	}


	#region Picking up/Dropping items & Interacting

	void ElectrifyNInteract() {
		if (!_ElectricListener.InteractAt(this, electricPoint)) {
			// Didn't interact with anything... Try to electrify
			if (inventory.equipped != null && inventory.equipped.canBeElectrified) {
				// Electrify the equipped item
				inventory.equipped.SendMessage(ElectricMethods.Electrify, this, SendMessageOptions.DontRequireReceiver);
				electrifying = true;
			} else if (inventory.equipped == null) {
				// Try to electrify at your fingertips
				_ElectricListener.ElectrifyAllAt(this, electricPoint);
				electrifying = true;
			}
		}

	}

	public _Equipable GetItemInRange() {
		var closest = Searchable.GetClosest<_Equipable>(controller.characterCenter, inventory.ignoreYAxis);
		return closest.valid && closest.dist <= inventory.pickupRadius ? closest.obj : null;
	}

	void GrabNDrop() {
		if (inventory.equipped == null) {
			// No item equipped. Try to grab the nearby item
			var item = GetItemInRange();
			if (item != null)
				inventory.AddItem(item);
		} else {
			// Item equipped. Drop it.
			inventory.Unequip();
		}
	}

	#endregion
}
