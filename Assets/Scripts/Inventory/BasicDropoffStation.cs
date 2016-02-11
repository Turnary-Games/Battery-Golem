using UnityEngine;
using System.Collections;

public class BasicDropoffStation : _DropoffStation<_DropoffItem> {

	public GameObject sendMessageTo;
	public string message = "OnDropoff";

	void OnInteractStart(PlayerController player) {
		player.inventory.Dropoff(station: this);
	}

	public override void OnItemAdded(OnItemAddedEvent _event) {
		base.OnItemAdded(_event);
		if (sendMessageTo != null)
			sendMessageTo.SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
}
