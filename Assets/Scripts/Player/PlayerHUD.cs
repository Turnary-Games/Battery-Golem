using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExtensionMethods;

public class PlayerHUD : PlayerSubClass {

	[Header("UI elements")]
	public Animator[] leaves;

	public bool isOpen { get {
		return leaves.Length > 0 && leaves[0] != null ? leaves[0].gameObject.activeSelf : false;
	} }

	void Start() {
		SetHUDVisable(false);
	}

	void Update() {
		// Read input
		if (Input.GetButtonDown("Inventory")) {
			SetHUDVisable(true);
		}
		if (Input.GetButtonUp("Inventory")) {
			SetHUDVisable(false);
		}

		
	}

	/// <summary>
	/// Unlock a slot. This would happen if you picked up an (inventory) item for the first time.
	/// </summary>
	public void UnlockItem(int slot) {
		Animator anim = leaves.Get(slot);
		if (anim != null) {
			SetHUDVisable(true);
			anim.SetBool("Slow", true);
		}
	}

	void SetHUDVisable(bool state) {
		for (int slot = 0; slot < inventory.coreItems.Length; slot++) {
			Animator anim = leaves.Get(slot);
			_Equipable item = inventory.coreItems.Get(slot);

			anim.gameObject.SetActive(state);

			if (state) {
				// When activating/deactivating the animator it gets confused and "un-initialized"
				if (!anim.isInitialized) {
					// Reset the animator
					anim.Rebind();
				}

				anim.SetBool("Open", item != null && item.unlocked);
				anim.SetBool("Slow", false);
			}
		}
	}
	
}
