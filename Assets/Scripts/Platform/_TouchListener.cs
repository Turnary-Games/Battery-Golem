using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class TouchMethods {
	// Called by the listener
	public readonly static string OnTouch = "OnTouch";
	public readonly static string OnTouchStart = "OnTouchStart";
	public readonly static string OnTouchEnd = "OnTouchEnd";

	// Called by anything else to contact the listener
	public readonly static string Touch = "Touch";
}

public class Touch {
	public Object source;
	public bool touchedThisStep;
	public bool touchedLastStep;

	public Touch(Object source) {
		this.source = source;
		touchedThisStep = true;
		touchedLastStep = false;
	}

	public bool FixedUpdate(_TouchListener listener) {
		// Touching stopped
		if (touchedLastStep && !touchedThisStep) {
			listener.SendMessage(TouchMethods.OnTouchEnd, this, SendMessageOptions.DontRequireReceiver);
			return true; // <- remove me
		}

		// Touching continues
		if (touchedThisStep) {
			listener.SendMessage(TouchMethods.OnTouch, this, SendMessageOptions.DontRequireReceiver);
		}

		touchedLastStep = touchedThisStep;
		touchedThisStep = false;

		return false; // <- keep me me
	}
}

public class _TouchListener : MonoBehaviour {

	public static List<_TouchListener> listeners = new List<_TouchListener>();
	protected List<Touch> touches = new List<Touch>();

	void Start() {
		listeners.Add(this);
	}

	void FixedUpdate() {
		touches.RemoveAll(delegate (Touch touch) {
			return touch.FixedUpdate(this);
		});
	}

	public void Touch(Object source) {
		if (source) {
			// Try to find a matching touch
			Touch match = FindTouch(source);

			if (match != null) {
				// Update the touch
				match.touchedThisStep = true;
			} else {
				// Create new touch
				Touch touch = new Touch(source);
				SendMessage(TouchMethods.OnTouchStart, touch, SendMessageOptions.DontRequireReceiver);
				touches.Add(touch);
			}
		}
	}

	// Get a touch with matching source
	public Touch FindTouch(Object source) {
		return touches.Find(delegate (Touch obj) {
			return obj.source.Equals(source);
		});
	}

	// Get the listeners with matching source
	public static List<_TouchListener> FindListeners(Object source) {
		return listeners.FindAll(delegate (_TouchListener listener) {
			return listener.FindTouch(source) != null;
		});
	}

}
