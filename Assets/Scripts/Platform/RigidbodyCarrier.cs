using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(_TouchListener))]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyCarrier : MonoBehaviour {

	[HideInInspector]
	public Rigidbody body;
	[DoNotSerialize]
	public HashSet<Rigidbody> bodies = new HashSet<Rigidbody>();

	void Start() {
		body = GetComponent<Rigidbody>();
	}

	void OnTouchStart(Touch touch) {
		if (touch == null || touch.source == null) return;

		if (touch.source is _Equipable) {
			var item = touch.source as _Equipable;
			bodies.Add(item.body);
		} else if (touch.source is TouchTransmitter) {
			var transmitter = touch.source as TouchTransmitter;
			bodies.Add(transmitter.body);
		}
    }

    void OnTouchEnd(Touch touch) {
        var item = touch.source as _Equipable;
        if (item != null && bodies.Contains(item.body)) {
            bodies.Remove(item.body);
            item.body.velocity = Vector3.zero;
			return;
        }

		var transmitter = touch.source as TouchTransmitter;
		if (transmitter != null && bodies.Contains(transmitter.body)) {
			bodies.Remove(transmitter.body);
			//transmitter.body.velocity = Vector3.zero;
			return;
		}
	}

    void OnTouch(Touch touch) {
        var item = touch.source as _Equipable;
		if (item != null && bodies.Contains(item.body)) {
			item.body.velocity = body.velocity;
			return;
		}

		var transmitter = touch.source as TouchTransmitter;
		if (transmitter != null && bodies.Contains(transmitter.body)) {
			if (transmitter.body.velocity.sqrMagnitude < body.velocity.sqrMagnitude) {
				if (transmitter.body.isKinematic) {
					transmitter.transform.Translate(body.velocity * Time.fixedDeltaTime, Space.World);
				} else
					transmitter.body.velocity = body.velocity;
			}			
			return;
		}
	}

}
