using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _Effect : MonoBehaviour {}

public class EffectItem {
	public _Effect effect;
	public ActionOnEvent onPickup;
	public ActionOnEvent onDrop;

	public void OnPickup() {
		OnEvent (onPickup);
	}

	public void OnDrop() {
		OnEvent (onDrop);
	}

	public void OnEvent(ActionOnEvent action) {
		switch (action) {
		case ActionOnEvent.pass:
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

	public enum ActionOnEvent {
		pass,
		enable,
		disable
	}
}

public static class EffectItemExtension {

	// Array extension
	public static void OnPickup(this EffectItem[] array) {
		foreach (EffectItem effect in array) {
			effect.OnPickup();
		}
	}

	public static void OnDrop(this EffectItem[] array) {
		foreach (EffectItem effect in array) {
			effect.OnDrop();
		}
	}

	// List extension
	public static void OnPickup(this List<EffectItem> list) {
		foreach (EffectItem effect in list) {
			effect.OnPickup();
		}
	}
	
	public static void OnDrop(this List<EffectItem> list) {
		foreach (EffectItem effect in list) {
			effect.OnDrop();
		}
	}
}

