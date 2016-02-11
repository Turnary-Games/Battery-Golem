using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	public Animator anim;
	public string multiplier = "Direction";

	public bool on = false;

	void Start() {
		if (anim != null)
			anim.SetFloat(multiplier, on ? 1 : -1);
	}

	void OnElectricStart() {
		on = !on;
		if (anim != null)
			anim.SetFloat(multiplier, on ? 1 : -1);
	}

}
