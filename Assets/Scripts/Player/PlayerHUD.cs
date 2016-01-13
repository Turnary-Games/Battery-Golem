using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExtensionMethods;

public class PlayerHUD : MonoBehaviour, PlayerSubClass {

	[Header("Player parent class")]

	public PlayerController parent;
	public PlayerController controller { get { return parent; } }
	public PlayerInventory inventory { get { return parent.inventory; } }
	public PlayerMovement movement { get { return parent.movement; } }
	public PlayerHealth health { get { return parent.health; } }
	public PlayerInteraction interaction { get { return parent.interaction; } }

	public Sprite placeholderIcon;

	[Header("UI elements")]
	public UIElement[] elements;

	void Awake() {
		UIElement.placeholder = placeholderIcon;
	}

#if UNITY_EDITOR
	void OnValidate() {

		if (inventory != null && elements.Length != inventory.slots.Length) {
			UIElement[] old = elements;
			elements = new UIElement[inventory.slots.Length];

			int slots = Mathf.Min(inventory.slots.Length, old.Length);

			for (int slot = 0; slot < slots; slot++) {
				elements[slot] = old[slot];
			}
		}
		
	}
#endif

	public void UpdateUIElements() {

        for (int slot=0; slot < elements.Length; slot++) {
			var item = inventory.slots.Get(slot);
			var element = elements.Get(slot);

			if (element != null) {
				element.UpdateElement(this, slot, item);
			}
		}

	}

	public bool UnlockSlot(int slot) {
		var e = elements.Get(slot);
		if (e != null && !e.unlocked) {
			e.unlocked = true;
			return true;
		}
		return false;
	}

	public void OnClick(int slot) {
		// The UI Element for the slot got clicked

		if (slot == 0) {
			inventory.Unequip();
		} else {
			inventory.SwapItems(0, slot);
		}
	}

	// Called by clicking on the HUD
	public void DropItem() {
		print("HUD sais drop it!");
		inventory.Unequip();
	}
	
}
