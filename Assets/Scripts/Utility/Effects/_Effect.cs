using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _Effect : MonoBehaviour {}

[System.Serializable]
public class _EffectItem {
	// Action type
	public Type type;

	// Effect
	public _Effect effect;
	public ActionOnEvent onPickup;
	public ActionOnEvent onDrop;

	// Reset transform
	public Transform resetTransform;
	public Space resetRelativeTo;
	public ResetTransform resetOnPickup;
	public ResetTransform resetOnDrop;

	public void OnPickup() {
		OnEvent(EventType.onPickup);
	}

	public void OnDrop() {
		OnEvent(EventType.onDrop);
	}

	public void OnEvent(EventType eventType) {
		switch (type) {
			case Type.effect:
				DoEffect(eventType);
				break;

			case Type.reset:
				DoReset(eventType);
				break;
		}
	}

	public void DoEffect(EventType eventType) {
		if (eventType == EventType.onDrop)
			DoEventAction(onDrop);
		else if (eventType == EventType.onPickup)
			DoEventAction(onPickup);
	}

	public void DoEventAction(ActionOnEvent action) {
		if (effect == null)
			return;

		switch (action) {
			case ActionOnEvent.doNothing:
				// Do nothing
				break;

			case ActionOnEvent.enable:
				effect.enabled = true;
				break;

			case ActionOnEvent.disable:
				effect.enabled = false;
				break;
		}
	}

	public void DoReset(EventType eventType) {
		if (eventType == EventType.onDrop)
			resetOnDrop.Reset(resetTransform, Space.Self);
		else if (eventType == EventType.onPickup)
			resetOnPickup.Reset(resetTransform, Space.Self);
	}

	public enum EventType {
		onPickup,
		onDrop
	}

	public enum ActionOnEvent {
		doNothing,
		enable,
		disable
	}

	public enum Type {
		effect,
		reset,
	}

	[System.Serializable]
	public struct ResetTransform {
		public bool position;
		public bool rotation;
		public bool scale;

		public ResetTransform(bool position, bool rotation, bool scale) {
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		public void Reset(Transform transform, Space relativeTo) {
			if (relativeTo == Space.Self) {

				if (position)
					transform.localPosition = Vector3.zero;

				if (rotation)
					transform.localRotation = Quaternion.identity;

				if (scale)
					transform.localScale = Vector3.one;

			} else if (relativeTo == Space.World) {

				if (position)
					transform.position = Vector3.zero;

				if (rotation)
					transform.rotation = Quaternion.identity;

				if (scale) {
					// This is weird but whatev
					var parent = transform.parent;

					transform.SetParent(null, true);
					transform.localScale = Vector3.one;
					transform.SetParent(parent, true);
				}

			}
		}
	}
}

public static class EffectItemExtension {

	// Array extension
	public static void OnPickup(this _EffectItem[] array) {
		foreach (_EffectItem effect in array) {
			effect.OnPickup();
		}
	}

	public static void OnDrop(this _EffectItem[] array) {
		foreach (_EffectItem effect in array) {
			effect.OnDrop();
		}
	}

	// List extension
	public static void OnPickup(this List<_EffectItem> list) {
		foreach (_EffectItem effect in list) {
			effect.OnPickup();
		}
	}
	
	public static void OnDrop(this List<_EffectItem> list) {
		foreach (_EffectItem effect in list) {
			effect.OnDrop();
		}
	}
}

