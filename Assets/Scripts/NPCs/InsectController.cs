using UnityEngine;
using System.Collections;

[RequireComponent(typeof(_ElectricListener))]
public class InsectController : MonoBehaviour {
	
	public Animator anim;

	private bool isFlipped;

	void OnInteractStart() {
		if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
			anim.SetTrigger("Shake");
	}

	void OnElectrifyStart() {
		isFlipped = true;
		anim.SetBool("Flipped", isFlipped);
	}

}
