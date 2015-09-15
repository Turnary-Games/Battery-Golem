using UnityEngine;
using System.Collections;

public class ElectricListener : MonoBehaviour {

	public Collider col;

	private bool electrifiedThisStep;
	private bool electrifiedLastStep;

	void FixedUpdate() {
		// Electrifying stopped
		if (electrifiedLastStep && !electrifiedThisStep) {
			SendMessage ("OnElectrifyEnd");
		}

		// Electrifying continues
		if (electrifiedThisStep)
			SendMessage ("OnElectrify");

		electrifiedLastStep = electrifiedThisStep;
		electrifiedThisStep = false;
	}

	public void Electrify() {
		// Electrifying starts
		if (!electrifiedLastStep && !electrifiedThisStep)
			SendMessage ("OnElectrifyStart");

		electrifiedThisStep = true;
	}

	public bool IsInside(Vector3 point) {
		if (col == null)
			return false;

		return col.bounds.Contains (point);
	}

	public static ElectricListener GetObjectAt(Vector3 point) {
		foreach (ElectricListener obj in FindObjectsOfType<ElectricListener>()) {
			if (obj.IsInside(point))
				return obj;
		}
		return null;
	}

	// Electrify everything touching this point
	public static bool ElectrifyAllAt(Vector3 point) {
		bool success = false;

		foreach (ElectricListener obj in FindObjectsOfType<ElectricListener>()) {
			if (obj.IsInside(point)) {
				success = true;
				obj.Electrify();
			}
		}

		return success;
	}
}
