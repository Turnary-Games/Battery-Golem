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

[DisallowMultipleComponent]
public class _ElectricListener : Searchable {

	public Collider col;
	public bool ignoreY = false;
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

			if (col is SphereCollider) {
				var s = col as SphereCollider;

				if (ignoreY) {
					Vector3 pos = s.transform.TransformPoint(s.center);
					float rad = s.radius * s.transform.lossyScale.MaxValue();

					UnityEditor.Handles.color = Color.cyan;
					UnityEditor.Handles.DrawLine(pos + new Vector3(rad, 50), pos + new Vector3(rad, -50));
					UnityEditor.Handles.DrawLine(pos + new Vector3(-rad, 50), pos + new Vector3(-rad, -50));
					UnityEditor.Handles.DrawLine(pos + new Vector3(0, 50, rad), pos + new Vector3(0, -50, rad));
					UnityEditor.Handles.DrawLine(pos + new Vector3(0, 50, -rad), pos + new Vector3(0, -50, -rad));
					UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, rad);
					UnityEditor.Handles.DrawWireDisc(pos + Vector3.up * 50, Vector3.up, rad);
					UnityEditor.Handles.DrawWireDisc(pos + Vector3.down * 50, Vector3.up, rad);
				} else {
					Gizmos.color = Color.cyan;
					Gizmos.DrawWireSphere(s.transform.TransformPoint(s.center), s.radius * s.transform.lossyScale.MaxValue());
				}
			} else {
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(ignoreY ? col.bounds.center.SetY(0) : col.bounds.center, ignoreY ? col.bounds.size.SetY(100f) : col.bounds.size);
				if (ignoreY && col.bounds.size.y != 0) {
					Gizmos.DrawWireCube(col.bounds.center.SetY(0), col.bounds.size.SetY(0));
				}
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

		// Custom algorithm for sphere colliders
		if (col is SphereCollider) {
			var s = col as SphereCollider;
			if (ignoreY) point.y = s.transform.TransformPoint(s.center).y;
			return Vector3.Distance(point, s.transform.TransformPoint(s.center)) <= s.radius * s.transform.lossyScale.MaxValue();
		}

		// Otherwise just use the collision bounds
		if (ignoreY) point.y = col.bounds.center.y;
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
