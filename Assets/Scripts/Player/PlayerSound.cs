using UnityEngine;
using System.Collections;

public class PlayerSound : PlayerSubClass {

	[Header("Audio")]
	public AudioSource footstep;
	public AudioSource grounded;
	public AudioSource electrify;
	public AudioSource itemEquipped;
	public AudioSource itemUnequipped;

	// Called by the walking animation
	public void OnFootstep() {
		if (movement && movement.grounded)
			footstep.Play();
	}

	void Update() {
		bool elecon = interaction.isElectrifying && interaction.armsUp;
		if (elecon && !electrify.isPlaying) electrify.Play();
		if (!elecon && electrify.isPlaying) electrify.Stop();
	}

	// Called by PlayerInventory
	public void OnItemEquipped() {
		itemEquipped.Play();
	}

	// Called by PlayerInventory
	public void OnItemUnequipped() {
		itemUnequipped.Play();
	}

	// Called by PlayerMovement
	public void OnGrounded() {
		grounded.Play();
	}

}
