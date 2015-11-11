using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropoffStation : MonoBehaviour {

    public Transform targetTransform;
	public bool valid {
		get {
			return item != null;
		}
	}

	private DropoffItem item;

	void OnInteractStart(PlayerController player) {
		player.inventory.Dropoff(station:this);
	}

    public virtual void OnDropoff(DropoffItem item) {
        this.item = item;

        item.transform.parent = targetTransform != null ? targetTransform : transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }

}
