using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _Effect : MonoBehaviour {
	public virtual void OnPickup() {}
	public virtual void OnDrop() {}

	public virtual void OnActionDisable() {}
	public virtual void OnActionEnable() {}
}

[System.Serializable]
public class _EffectItem {
	// Action type
	public Type type;

	// Effect
	public _Effect effect;
	public ActionOnEvent effectOnPickup;
	public ActionOnEvent effectOnDrop;

    // Toggle gameobject
    public GameObject toggleGameObject;
    public ActionOnEvent toggleOnPickup;
    public ActionOnEvent toggleOnDrop;

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

	void OnEvent(EventType eventType) {
		switch (type) {
			case Type.effect:
                if (eventType == EventType.onDrop) {
                    DoEffectAction(effectOnDrop);
                    effect.OnDrop();
                } else if (eventType == EventType.onPickup) {
                    DoEffectAction(effectOnPickup);
                    effect.OnPickup();
                }
                break;

			case Type.resetTransform:
				DoReset(eventType);
				break;

            case Type.gameObject:
                if (eventType == EventType.onDrop) {
                    DoGameObjectAction(toggleOnDrop);
                } else if (eventType == EventType.onPickup) {
                    DoGameObjectAction(toggleOnPickup);
                }
                break;
		}
	}

    void DoEffectAction(ActionOnEvent action) {
        if (effect == null)
            return;

        switch (action) {
            case ActionOnEvent.doNothing:
                // Do nothing
                break;

            case ActionOnEvent.enable:
                effect.OnActionEnable();
                effect.enabled = true;
                break;

            case ActionOnEvent.disable:
                effect.OnActionDisable();
                effect.enabled = false;
                break;

            case ActionOnEvent.toggle:
                if (effect.enabled) {
                    DoEffectAction(ActionOnEvent.disable);
                } else {
                    DoEffectAction(ActionOnEvent.enable);
                }
                break;
        }
    }

    void DoGameObjectAction(ActionOnEvent action) {
        if (toggleGameObject == null)
            return;

        switch (action) {
            case ActionOnEvent.doNothing:
                // Do nothing
                break;

            case ActionOnEvent.enable:
                toggleGameObject.SetActive(true);
                break;

            case ActionOnEvent.disable:
                toggleGameObject.SetActive(false);
                break;

            case ActionOnEvent.toggle:
                toggleGameObject.SetActive(!toggleGameObject.activeSelf);
                break;
        }
    }

    void DoReset(EventType eventType) {
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
		disable,
        toggle
	}

	public enum Type {
		effect,
        gameObject,
		resetTransform,
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

