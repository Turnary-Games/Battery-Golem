using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public sealed class ElectricMethods {
	// Called by the listener
	public readonly static string OnElectrify = "OnElectrify"; // args: PlayerController source
	public readonly static string OnElectrifyStart = "OnElectrifyStart"; // args: PlayerController source
	public readonly static string OnElectrifyEnd = "OnElectrifyEnd"; // args: PlayerController source

	public readonly static string OnInteract = "OnInteract"; // args: PlayerController source
	public readonly static string OnInteractStart = "OnInteractStart"; // args: PlayerController source
	public readonly static string OnInteractEnd = "OnInteractEnd"; // args: PlayerController source

	// Called by anything else to contact the listener
	public readonly static string Electrify = "Electrify"; // args: PlayerController source
	public readonly static string Interact = "Interact"; // args: PlayerController source
}

public class _ElectricListener : Searchable {

	public Collider col;
	public bool acceptElectrifying = true;
	public bool acceptInteraction;

	private bool electrifiedThisStep;
	private bool electrifiedLastStep;
	private PlayerController electrifiedSource;

	private bool interactThisStep;
	private bool interactLastStep;
	private PlayerController interactSource;

	public override bool valid {
		get {
			return col != null;
		}
	}

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (col)
        {
            Gizmos.color = Color.cyan;

			if (col is SphereCollider) {
				var s = col as SphereCollider;
				Gizmos.DrawWireSphere(s.transform.TransformPoint(s.center), s.radius * s.transform.lossyScale.MaxValue());
			} else {
				Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
			}

        }
    }
#endif

    void FixedUpdate() {
		/*
			Electrifying
		*/

		// Electrifying stopped
		if (electrifiedLastStep && !electrifiedThisStep) {
			SendMessage(ElectricMethods.OnElectrifyEnd, electrifiedSource, SendMessageOptions.DontRequireReceiver);
			electrifiedSource = null;
		}

		// Electrifying continues
		if (electrifiedThisStep)
			SendMessage(ElectricMethods.OnElectrify, electrifiedSource, SendMessageOptions.DontRequireReceiver);

		electrifiedLastStep = electrifiedThisStep;
		electrifiedThisStep = false;


		/*
			Interacting
		*/

		// Interacting stopped
		if (interactLastStep && !interactThisStep) {
			SendMessage(ElectricMethods.OnInteractEnd, interactSource, SendMessageOptions.DontRequireReceiver);
			interactSource = null;
		}

		// Interacting continues
		if (interactThisStep)
			SendMessage(ElectricMethods.OnInteract, interactSource, SendMessageOptions.DontRequireReceiver);

		interactLastStep = interactThisStep;
		interactThisStep = false;
	}

	public void Electrify(PlayerController source) {
		if (!acceptElectrifying)
			return;

		// Electrifying starts
		if (!electrifiedLastStep && !electrifiedThisStep)
			SendMessage(ElectricMethods.OnElectrifyStart, source, SendMessageOptions.DontRequireReceiver);

		electrifiedThisStep = true;
		electrifiedSource = source;
	}

	public void Interact(PlayerController source) {
		if (!acceptInteraction)
			return;

		// Interacting starts
		if (!interactLastStep && !interactThisStep)
			SendMessage(ElectricMethods.OnInteractStart, source, SendMessageOptions.DontRequireReceiver);

		interactThisStep = true;
		interactSource = source;
	}

	public bool IsInside(Vector3 point) {
		if (col == null)
			return false;

		if (col is SphereCollider) {
			var s = col as SphereCollider;
			return Vector3.Distance(point, s.transform.TransformPoint(s.center)) <= s.radius * s.transform.lossyScale.MaxValue();
		}
		
		return col.bounds.Contains (point);
	}

	public static _ElectricListener GetListenerAt(Vector3 point) {
		foreach (var obj in FindObjectsOfType<_ElectricListener>()) {
			if (obj.IsInside(point))
				return obj;
		}
		return null;
	}

	public static List<_ElectricListener> GetListenersAt(Vector3 point) {
		var list = new List<_ElectricListener>();

		foreach (var obj in FindObjectsOfType<_ElectricListener>()) {
			if (obj.IsInside(point))
				list.Add(obj);
		}

		return list;
	}

	// Electrify everything touching this point
	public static bool ElectrifyAllAt(PlayerController source, Vector3 point) {
		bool success = false;

		foreach (var obj in FindObjectsOfType<_ElectricListener>()) {
			if (obj.acceptElectrifying && obj.IsInside(point)) {
				success = true;
				obj.Electrify(source);
			}
		}

		return success;
	}

	// Interact with the closest thing at point
	public static bool InteractAt(PlayerController source, Vector3 point) {
		var list = GetListenersAt(point);
		list.RemoveAll(delegate (_ElectricListener obj) {
			return !obj.acceptInteraction;
		});

		var closest = new Closest<_ElectricListener>(list, point);

		if (closest.valid)
			closest.obj.Interact(source);

		return closest.valid;
	}

	public override float GetDistance(Vector3 from, bool ignoreY = false) {
		Vector3 to = col.transform.position;

		if (ignoreY)
			to.y = from.y = 0;

		return Vector3.Distance(from, to);
	}
}
