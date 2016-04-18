using UnityEngine;
using System.Collections;

[RequireComponent(typeof(_ElectricListener))]
public class BasicItemDropoff : MonoBehaviour {

	public int filterID = -1;
	public Renderer activate;
	public bool activated;
	[Space]
	public GameObject sendMessageTo;
	public string message = "OnDropoff";

	void OnInteractStart(PlayerController source) {
		if (activated) return;

		_Item item = source.inventory.equipped;
		if (item == null) return;

		if (filterID == -1 || item.id == filterID) {
			Destroy(item.gameObject);
			activated = true;

			if (activate != null)
				activate.enabled = true;

			if (sendMessageTo != null)
				sendMessageTo.SendMessage(message, SendMessageOptions.DontRequireReceiver);
		}
	}

}
