using UnityEngine;
using System.Collections;

public class ConveyorController : MonoBehaviour {

	public Lever lever;
	public ConveyorBelt[] belts;
	private Vector3[] motion;
	public float topSpeedAfter = 1;

	bool elec = false;
	float t = 0;

	void Awake() {
		motion = new Vector3[belts.Length];
		for (int i=0; i<belts.Length; i++) {
			motion[i] = belts[i].motion;
		}
	}

	void Update() {
		if (topSpeedAfter <= 0)
			t = elec ? 1 : 0;
		else
			t = Mathf.MoveTowards(t, elec ? 1 : 0, Time.deltaTime);

		for (int i = 0; i < belts.Length; i++) {
			ConveyorBelt belt = belts[i];
			belt.motion = Vector3.Lerp(Vector3.zero, motion[i], t);
		}
	}

	void OnElectricStart() {
		elec = true;
	}

	void OnElectricEnd() {
		elec = true;
	}

}
