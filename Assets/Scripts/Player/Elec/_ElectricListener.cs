using UnityEngine;
using System.Collections;

public sealed class ElectricMethods {
	// Called by the listener
	public static string OnElectrify = "OnElectrify";
	public static string OnElectrifyStart = "OnElectrifyStart";
	public static string OnElectrifyEnd = "OnElectrifyEnd";

	// Called by anything else to contact the listener
	public static string Electrify = "Electrify";
}

public class _ElectricListener : MonoBehaviour {

	public Collider col;

	private bool electrifiedThisStep;
	private bool electrifiedLastStep;

	void FixedUpdate() {
		// Electrifying stopped
		if (electrifiedLastStep && !electrifiedThisStep) {
			SendMessage (ElectricMethods.OnElectrifyEnd);
		}

		// Electrifying continues
		if (electrifiedThisStep)
			SendMessage (ElectricMethods.OnElectrify);

		electrifiedLastStep = electrifiedThisStep;
		electrifiedThisStep = false;
	}

	public void Electrify() {
		// Electrifying starts
		if (!electrifiedLastStep && !electrifiedThisStep)
			SendMessage (ElectricMethods.OnElectrifyStart);

		electrifiedThisStep = true;
	}

	public bool IsInside(Vector3 point) {
		if (col == null)
			return false;

		return col.bounds.Contains (point);
	}

	public static _ElectricListener GetObjectAt(Vector3 point) {
		foreach (var obj in FindObjectsOfType<_ElectricListener>()) {
			if (obj.IsInside(point))
				return obj;
		}
		return null;
	}

	// Electrify everything touching this point
	public static bool ElectrifyAllAt(Vector3 point) {
		bool success = false;

		foreach (var obj in FindObjectsOfType<_ElectricListener>()) {
			if (obj.IsInside(point)) {
				success = true;
				obj.Electrify();
			}
		}

		return success;
	}
}
