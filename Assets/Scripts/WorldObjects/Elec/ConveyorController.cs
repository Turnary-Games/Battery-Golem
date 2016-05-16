using UnityEngine;
using System.Collections;

public class ConveyorController : MonoBehaviour {

	public Lever lever;
	public Object[] belts;
	public Vector3 beltMotion;
	public Vector3 scrollMotion;
	public float topSpeedAfter = 1;

	bool elec = false;
	float t = 0;

	int target { get { return elec ? (lever.on ? -1 : 1) : 0; } }

	void Update() {
		// Update the scale
		if (topSpeedAfter <= 0)
			t = target;
		else
			t = Mathf.MoveTowards(t, target, Time.deltaTime);

		// Update each belt
		for (int i = 0; i < belts.Length; i++) {
			Object obj = belts[i];

			if (obj is ConveyorBelt) (obj as ConveyorBelt).motion = beltMotion * Mathf.Sign(t);
			else if (obj is ScrollTexture) (obj as ScrollTexture).motion = scrollMotion * t;
		}
	}

	void OnElectrifyStart() {
		elec = true;
	}

	void OnElectrifyEnd() {
		elec = false;
	}

}
